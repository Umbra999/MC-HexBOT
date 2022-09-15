using System.Net.Sockets;
using MCHexBOT.Pakets;
using MCHexBOT.Pakets.Client.Login;
using MCHexBOT.Utils;
using Ionic.Zlib;
using MCHexBOT.Protocol;

namespace MCHexBOT.Network
{
    public class MinecraftConnection
    {
        private TcpClient Tcp { get; set; }
        public MinecraftStream ReadStream { get; private set; }
        public MinecraftStream WriteStream { get; private set; }
        public CancellationToken CancellationToken { get; set; }

        public ConnectionState State { get; set; }

        public Queue<PacketQueueItem> PaketQueue { get; set; } = new Queue<PacketQueueItem>();
        public PaketRegistry WriterRegistry { get; set; }
        public PaketRegistry ReaderRegistry { get; set; }

        public Thread ReadThread { get; private set; }
        public Thread WriteThread { get; private set; }

        public bool CompressionEnabled { get; set; } = false;
        public int CompressionThreshold { get; set; } = 256;

        public IPaketHandler Handler { get; set; }

        private bool TerminateThreads = false;

        public MinecraftConnection(TcpClient tcp)
        {
            Tcp = tcp;

            State = ConnectionState.Handshaking;

            ReadStream = new MinecraftStream(Tcp.GetStream(), CancellationToken.None);
            WriteStream = new MinecraftStream(Tcp.GetStream(), CancellationToken.None);
        }

        public void Start()
        {
            ReadThread = new Thread(ProcessNetworkRead);
            WriteThread = new Thread(ProcessNetworkWrite);

            ReadThread.Name = "MCHexBOT - Read";
            WriteThread.Name = "MCHexBOT - Write";

            ReadThread.Start();
            WriteThread.Start();
        }

        public void EnableEncryption(byte[] key)
        {
            WriteStream.InitEncryption(key);
            ReadStream.InitEncryption(key);
        }

        public void ProcessNetworkRead()
        {
            try
            {
                SpinWait sw = new();

                while (!CancellationToken.IsCancellationRequested && !TerminateThreads)
                {
                    if (CancellationToken.IsCancellationRequested) break;

                    if (ReadStream.DataAvailable)
                    {
                        IPaket paket = TryReadPacket(ReadStream, out var lastPaketId);

                        if (paket != null && Handler != null)
                        {
                            switch (State)
                            {
                                case ConnectionState.Handshaking:
                                    Handler.Handshake(paket);
                                    break;
                                case ConnectionState.Status:
                                    Handler.Status(paket);
                                    break;
                                case ConnectionState.Login:
                                    Handler.Login(paket);
                                    break;
                                case ConnectionState.Play:
                                    Handler.Play(paket);
                                    break;
                            }
                        }
                    }
                    else sw.SpinOnce();
                }
            }
            catch (Exception e)
            {
                Logger.LogError($"Error reading paket: (State {State}) {e.Message}");
            }
        }

        public void ProcessNetworkWrite()
        {
            try
            {
                SpinWait sw = new();

                while (!CancellationToken.IsCancellationRequested && !TerminateThreads)
                {
                    if (CancellationToken.IsCancellationRequested) break;

                    IPaket toSend = null;
                    ConnectionState state = ConnectionState.Handshaking;

                    lock (PaketQueue)
                    {
                        if (PaketQueue.Count > 0)
                        {
                            var mcs = PaketQueue.Dequeue();
                            toSend = mcs.Paket;
                            state = mcs.State;
                        }
                    }

                    if (toSend != null)
                    {
                        byte[] data = EncodePaket(toSend, state);

                        WriteStream.WriteVarInt(data.Length);
                        WriteStream.Write(data);

                        if (toSend is SetCompressionPaket) CompressionEnabled = true;
                    }

                    sw.SpinOnce();
                }
            }
            catch (Exception e)
            {
                Logger.LogError($"Error writing paket: (State: {State}) {e.Message}");
            }
        }

        public byte[] EncodePaket(IPaket paket, ConnectionState state)
        {
            byte[] encodedPacket;

            using (MemoryStream ms = new MemoryStream())
            {
                using (MinecraftStream mc = new MinecraftStream(ms, CancellationToken))
                {
                    int id = WriterRegistry.GetPaketId(paket, state);
                    mc.WriteVarInt(id);
                    paket.Encode(mc);
                }

                encodedPacket = ms.ToArray();
            }

            if (CompressionEnabled)
            {
                using MemoryStream ms = new();
                using (MinecraftStream mc = new(ms, CancellationToken))
                {

                    if (encodedPacket.Length >= CompressionThreshold)
                    {
                        //byte[] compressed;
                        //CompressData(encodedPacket, out compressed);

                        mc.WriteVarInt(encodedPacket.Length);

                        using ZlibStream outZStream = new ZlibStream(mc, Ionic.Zlib.CompressionMode.Compress, true);
                        outZStream.Write(encodedPacket, 0, encodedPacket.Length);
                        //mc.Write(compressed);
                    }
                    else //Uncompressed
                    {
                        mc.WriteVarInt(0);
                        mc.Write(encodedPacket);
                    }
                }

                encodedPacket = ms.ToArray();
            }

            return encodedPacket;
        }

        public IPaket TryReadPacket(MinecraftStream stream, out int lastPacketId)
        {
            IPaket paket = null;
            int paketId = -1;
            byte[] paketData;

            if (!CompressionEnabled)
            {
                //Logger.LogWarning("Using default reader");

                int lenght = stream.ReadVarInt();
                paketId = stream.ReadVarInt(out int paketIdLenght);

                int dataLenght = lenght - paketIdLenght;

                if (dataLenght > 0) paketData = stream.Read(dataLenght);
                else
                {
                    paketData = new byte[0];
                    //Logger.LogWarning("Received Empty paket");
                }
            }
            else
            {
                int paketLenght = stream.ReadVarInt();
                int dataLenght = stream.ReadVarInt(out int dataLenghtLenght);

                if (dataLenght == 0)
                {
                    //Logger.LogWarning("Using reader without compressing -> threshold");

                    paketId = stream.ReadVarInt(out int paketIdLenght);
                    paketData = stream.Read(paketLenght - dataLenghtLenght - paketIdLenght);
                }
                else
                {
                    //Logger.LogWarning("Using compress reader");

                    var cache = stream.Read(paketLenght - dataLenghtLenght);

                    using MinecraftStream a = new(CancellationToken);

                    using (ZlibStream zstream = new(a, CompressionMode.Decompress, true))
                    {
                        zstream.Write(cache);
                    }

                    a.Seek(0, SeekOrigin.Begin);

                    int paketIdLenght;
                    paketId = a.ReadVarInt(out paketIdLenght);

                    int dataSize = paketLenght - dataLenghtLenght - paketIdLenght;
                    paketData = a.Read(dataSize);
                }
            }

            lastPacketId = paketId;

            if (ReaderRegistry.Pakets[State].ContainsKey((byte)paketId))
            {
                paket = ReaderRegistry.Pakets[State][(byte)paketId];
            }

            //Logger.LogWarning($"Got packet: {paket} (0x{paketId:X2})");

            try
            {
                if (paket == null) return null;

                using (var memoryStream = new MemoryStream(paketData))
                {
                    using MinecraftStream minecraftStream = new(memoryStream, CancellationToken);
                    paket.Decode(minecraftStream);
                }

                if (paket is SetCompressionPaket setCompressionPaket)
                {
                    CompressionThreshold = setCompressionPaket.Threshold;
                    CompressionEnabled = true;
                }

                return paket;
            }
            catch (Exception e)
            {
                //Logger.LogError("EXCEPTION IN READ: " + e.Message);
                return null;
            }
        }

        public void SendPaket(IPaket paket)
        {
            lock (PaketQueue)
            {
                PaketQueue.Enqueue(new PacketQueueItem()
                {
                    State = State,
                    Paket = paket
                });
            }
        }
    }
}
