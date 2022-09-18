using MCHexBOT.Network;
using MCHexBOT.Protocol;
using MCHexBOT.Protocol.Packets.LabyServer;
using MCHexBOT.Utils;
using System.Net.Sockets;

namespace MCHexBOT.Core
{
    internal class LabyClient
    {
        public MinecraftConnection MCConnection;
        public APIClient APIClient;

        public LabyClient(APIClient WebClient)
        {
            APIClient = WebClient;

            Logger.Log($"{APIClient.CurrentUser.name} connected as Bot");
        }

        public async void Connect(string Host, int Port)
        {
            TcpClient cl = new(Host, Port)
            {
                NoDelay = true,
            };

            MCConnection = new MinecraftConnection(cl);

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
            MCConnection.State = ConnectionState.Handshaking;

            MCConnection.SendPacket(new HelloPacket()
            {
                TickTime = Environment.TickCount,
                Type = 27,
            });
        }
    }
}
