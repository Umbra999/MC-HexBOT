using MCHexBOT.Network;
using MCHexBOT.Protocol;
using MCHexBOT.Protocol.Packets.LabyServer.Handshake;
using System.Net.Sockets;

namespace MCHexBOT.Core
{
    public class LabyClient
    {
        public MinecraftConnection MCConnection;
        public APIClient APIClient;
        public int ProtocolVersion = 27;

        public LabyClient(APIClient WebClient)
        {
            APIClient = WebClient;
            Connect("chat.labymod.net", 30336);
        }

        public void Connect(string Host, int Port)
        {
            TcpClient Client = new(Host, Port);

            MCConnection = new MinecraftConnection(Client, false);

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
                Type = ProtocolVersion,
            });
        }
    }
}
