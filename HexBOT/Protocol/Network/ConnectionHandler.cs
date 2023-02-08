using System.Net.Sockets;
using HexBOT.Packets;
using HexBOT.Packets.Client.Login;
using HexBOT.Utils;
using Ionic.Zlib;
using HexBOT.Protocol;
using System.Collections.Concurrent;

namespace HexBOT.Network
{
    public class ConnectionHandler
    {
        public TcpClient Tcp { get; }
        public MinecraftStream ReadStream { get; private set; }
        public MinecraftStream WriteStream { get; private set; }

        private Protocol.ProtocolType ProtocolType { get; set; }

        public ConnectionState State { get; set; }

        public ConcurrentQueue<PacketQueueItem> PacketQueue { get; set; } = new ConcurrentQueue<PacketQueueItem>();
        public PacketRegistry WriterRegistry { get; set; }
        public PacketRegistry ReaderRegistry { get; set; }

        public bool CompressionEnabled { get; set; } = false;
        public int CompressionThreshold { get; set; } = 256;

        public IPacketHandler Handler { get; set; }

        public ConnectionHandler(TcpClient tcp, Protocol.ProtocolType Type)
        {
            Tcp = tcp;

            State = ConnectionState.Handshaking;
            ProtocolType = Type;

            ReadStream = new MinecraftStream(Tcp.GetStream(), ProtocolType);
            WriteStream = new MinecraftStream(Tcp.GetStream(), ProtocolType);
        }

        public void Start()
        {
            Task.Run(() => ProcessNetworkRead());
            Task.Run(() => ProcessNetworkWrite());
        }

        public void Stop()
        {
            Tcp.Client.Close();
            Tcp.Close();
        }

        public void EnableWriteEncryption(byte[] key)
        {
            WriteStream.InitEncryption(key);
        }

        public void EnableReadEncryption(byte[] key)
        {
            ReadStream.InitEncryption(key);
        }

        public async Task ProcessNetworkRead()
        {
            while (Tcp.Connected)
            {
                try
                {
                    if (ReadStream.DataAvailable)
                    {
                        IPacket Packet = TryReadPacket(ReadStream, out var lastPacketId);

                        if (Packet != null && Handler != null)
                        {
                            switch (State)
                            {
                                case ConnectionState.Handshaking:
                                    Handler.Handshake(Packet);
                                    break;
                                case ConnectionState.Status:
                                    Handler.Status(Packet);
                                    break;
                                case ConnectionState.Login:
                                    Handler.Login(Packet);
                                    break;
                                case ConnectionState.Play:
                                    Handler.Play(Packet);
                                    break;
                            }
                        }
                    }
                    else await Task.Delay(1);
                }
                catch (Exception e)
                {
                    Logger.LogError($"Error reading Packet: (State {State}) {e.Message}");
                }
            }
        }

        public async Task ProcessNetworkWrite()
        {
            while (Tcp.Connected)
            {
                try
                {
                    if (PacketQueue.Count > 0)
                    {
                        bool Status = PacketQueue.TryDequeue(out PacketQueueItem item);
                        if (Status)
                        {
                            IPacket toSend = item.Packet;
                            ConnectionState state = item.State;

                            byte[] data = EncodePacket(toSend, state);

                            WriteStream.WriteVarInt(data.Length);
                            WriteStream.Write(data);

                            if (toSend is SetCompressionPacket) CompressionEnabled = true;
                        }
                    }
                    else await Task.Delay(1);
                }
                catch (Exception e)
                {
                    Logger.LogError($"Error writing Packet: (State: {State}) {e.Message}");
                }
            }
        }

        public byte[] EncodePacket(IPacket Packet, ConnectionState state)
        {
            byte[] encodedPacket;

            using (MemoryStream ms = new())
            {
                using (MinecraftStream mc = new(ms, ProtocolType))
                {
                    int id = WriterRegistry.GetPacketId(Packet, state);
                    mc.WriteVarInt(id);
                    Packet.Encode(mc);
                }

                encodedPacket = ms.ToArray();
            }

            if (CompressionEnabled)
            {
                using MemoryStream ms = new();
                using (MinecraftStream mc = new(ms, ProtocolType))
                {

                    if (encodedPacket.Length >= CompressionThreshold)
                    {
                        //byte[] compressed;
                        //CompressData(encodedPacket, out compressed);

                        mc.WriteVarInt(encodedPacket.Length);

                        using ZlibStream outZStream = new(mc, CompressionMode.Compress, true);
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

        public IPacket TryReadPacket(MinecraftStream stream, out int lastPacketId)
        {
            IPacket Packet = null;
            int PacketId = -1;
            byte[] PacketData;

            if (!CompressionEnabled)
            {
                //Logger.LogWarning("Using default reader");

                int lenght = stream.ReadVarInt();
                PacketId = stream.ReadVarInt(out int PacketIdLenght);

                int dataLenght = lenght - PacketIdLenght;

                if (dataLenght > 0) PacketData = stream.Read(dataLenght);
                else
                {
                    PacketData = new byte[0];
                    //Logger.LogWarning("Received Empty Packet");
                }
            }
            else
            {
                int PacketLenght = stream.ReadVarInt();
                int dataLenght = stream.ReadVarInt(out int dataLenghtLenght);

                if (dataLenght == 0)
                {
                    //Logger.LogWarning("Using reader without compressing -> threshold");

                    PacketId = stream.ReadVarInt(out int PacketIdLenght);
                    PacketData = stream.Read(PacketLenght - dataLenghtLenght - PacketIdLenght);
                }
                else
                {
                    //Logger.LogWarning("Using compress reader");

                    var cache = stream.Read(PacketLenght - dataLenghtLenght);

                    using MinecraftStream a = new(ProtocolType);

                    using (ZlibStream zstream = new(a, CompressionMode.Decompress, true))
                    {
                        zstream.Write(cache);
                    }

                    a.Seek(0, SeekOrigin.Begin);

                    PacketId = a.ReadVarInt(out int PacketIdLenght);

                    int dataSize = PacketLenght - dataLenghtLenght - PacketIdLenght;
                    PacketData = a.Read(dataSize);
                }
            }

            lastPacketId = PacketId;

            if (ReaderRegistry.Packets[State].ContainsKey((byte)PacketId))
            {
                Packet = ReaderRegistry.Packets[State][(byte)PacketId];
            }

            //Logger.LogWarning($"Got packet: {Packet} (0x{PacketId:X2})");

            try
            {
                if (Packet == null) return null;

                using (var memoryStream = new MemoryStream(PacketData))
                {
                    using MinecraftStream minecraftStream = new(memoryStream, ProtocolType);
                    Packet.Decode(minecraftStream);
                }

                if (Packet is SetCompressionPacket setCompressionPacket)
                {
                    CompressionThreshold = setCompressionPacket.Threshold;
                    CompressionEnabled = true;
                }

                return Packet;
            }
            catch (Exception e)
            {
                Logger.LogError("EXCEPTION IN READ: " + e.Message);
                return null;
            }
        }

        public void SendPacket(IPacket Packet)
        {
            PacketQueue.Enqueue(new PacketQueueItem()
            {
                State = State,
                Packet = Packet
            });
        }
    }
}
