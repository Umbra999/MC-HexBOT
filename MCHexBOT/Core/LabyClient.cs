using MCHexBOT.Features;
using MCHexBOT.Network;
using MCHexBOT.Protocol;
using MCHexBOT.Protocol.Packets.LabyServer.Handshake;
using System.Net.Sockets;

namespace MCHexBOT.Core
{
    public class LabyClient
    {
        public MinecraftConnection MCConnection;
        public MinecraftClient MinecraftClient;
        public VoiceClient VoiceClient;
        public int ProtocolVersion = 27;
        public string DashboadPin;

        public LabyClient(MinecraftClient Client)
        {
            MinecraftClient = Client;
            Connect("chat.labymod.net", 30336);
        }

        public void Connect(string Host, int Port)
        {
            TcpClient Client = new(Host, Port);

            MCConnection = new MinecraftConnection(Client, Protocol.ProtocolType.Labymod);

            PacketRegistry writer = new();
            PacketRegistry.RegisterLabyServerPackets(writer);

            PacketRegistry reader = new();
            PacketRegistry.RegisterLabyClientPackets(reader);

            MCConnection.WriterRegistry = writer;
            MCConnection.ReaderRegistry = reader;

            MCConnection.Handler = new LabyHandler(this)
            {
                Connection = MCConnection
            };

            MCConnection.Start();

            MCConnection.SendPacket(new HelloPacket()
            {
                TickTime = Environment.TickCount,
                Type = ProtocolVersion,
            });
        }

        public void OnReceivedPin()
        {
            VoiceClient = new VoiceClient(this);
            Task.Run(() => LabyFeatures.CollectCoinLoop(this, DashboadPin));
        }
    }
}
