using MCHexBOT.Utils;
using MCHexBOT.Network;
using MCHexBOT.Pakets.Server.Handshake;
using MCHexBOT.Pakets.Server.Login;
using System.Net.Sockets;

namespace MCHexBOT.Core
{
    public class MinecraftClient
    {
        public readonly APIClient APIClient;
        MinecraftConnection MCConnection;

        public MinecraftClient(APIClient WebClient)
        {
            APIClient = WebClient;

            Logger.Log($"{APIClient.CurrentUser.name} connected as Bot");
        }

        public bool Disconnect()
        {
            if (MCConnection == null) return false;
            MCConnection.Dispose();
            MCConnection = null;

            Logger.LogWarning($"{APIClient.CurrentUser.name} disconnected");
            return true;
        }

        public void Connect(string Version, string Host, int Port)
        {
            if (MCConnection != null) Disconnect();

            TcpClient cl = new(Host, Port);

            MCConnection = new MinecraftConnection(cl);

            int ProtocolVersion = 757;

            PaketRegistry writer = new();
            PaketRegistry.RegisterServerPakets(writer, ProtocolVersion);

            PaketRegistry reader = new();
            PaketRegistry.RegisterClientPakets(reader, ProtocolVersion);

            MCConnection.WriterRegistry = writer;
            MCConnection.ReaderRegistry = reader;

            MCConnection.Handler = new PaketHandler(APIClient)
            {
                Connection = MCConnection
            };

            MCConnection.Start();
            MCConnection.State = Enums.MinecraftState.Handshaking;

            Logger.LogWarning("Sending Handshake");

            MCConnection.SendPaket(new HandshakePaket()
            {
                NextState = 2,
                ProtocolVersion = ProtocolVersion,
                ServerAddress = Host,
                ServerPort = (ushort)Port
            });

            MCConnection.State = Enums.MinecraftState.Login;

            Logger.LogWarning("Sending Login");

            MCConnection.SendPaket(new LoginStartPaket()
            {
                Username = APIClient.CurrentUser.name
            });
        }
    }
}
