using MCHexBOT.Core.Minecraft;
using MCHexBOT.Features;
using MCHexBOT.Network;
using MCHexBOT.Protocol.Packets.LabyServer.Handshake;
using MCHexBOT.Utils;
using System.Net.Sockets;

namespace MCHexBOT.Core.Laby
{
    public class LabyClient
    {
        public ConnectionHandler MCConnection;
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
            try
            {
                Disconnect();

                TcpClient Client = new(Host, Port);

                MCConnection = new ConnectionHandler(Client, Protocol.ProtocolType.Labymod);

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
            catch { }
        }

        public void Disconnect()
        {
            if (MCConnection != null)
            {
                MCConnection.Stop();
                Logger.LogError($"{MinecraftClient.APIClient.CurrentUser.name} disconnected from Labymod");
            }
        }

        public void OnReceivedPin()
        {
            VoiceClient = new VoiceClient(this);
            Task.Run(() => LabyFeatures.CollectCoins(this, DashboadPin));
        }
    }
}
