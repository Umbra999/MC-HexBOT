using MCHexBOT.Utils;
using MCHexBOT.Network;
using MCHexBOT.Pakets.Server.Handshake;
using MCHexBOT.Pakets.Server.Login;
using System.Net.Sockets;
using MCHexBOT.Pakets.Server.Play;
using static MCHexBOT.Utils.GameTypes;

namespace MCHexBOT.Core
{
    public class MinecraftClient
    {
        public readonly APIClient APIClient;
        public MinecraftConnection MCConnection;
        public Player CurrentPlayer;

        public MinecraftClient(APIClient WebClient)
        {
            APIClient = WebClient;
            CurrentPlayer = new();

            Logger.Log($"{APIClient.CurrentUser.name} connected as Bot");
        }

        public void Connect(string Version, string Host, int Port)
        {
            TcpClient cl = new(Host, Port);

            MCConnection = new MinecraftConnection(cl);

            int ProtocolVersion = 757;

            PaketRegistry writer = new();
            PaketRegistry.RegisterServerPakets(writer, ProtocolVersion);

            PaketRegistry reader = new();
            PaketRegistry.RegisterClientPakets(reader, ProtocolVersion);

            MCConnection.WriterRegistry = writer;
            MCConnection.ReaderRegistry = reader;

            MCConnection.Handler = new PaketHandler(APIClient, this)
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

        public void SendChat(string Message)
        {
            MCConnection.SendPaket(new ChatMessagePaket()
            {
                Message = Message
            });
        }

        public void SendRespawn()
        {
            MCConnection.SendPaket(new ClientStatusPaket()
            {
                ActionID = 0
            });
        }

        public void SendMovement()
        {
            for (; ; )
            {
                MCConnection.SendPaket(new Pakets.Server.Play.PlayerPositionPaket()
                {
                    X = 1143,
                    FeetY = 92,
                    Z = -518,
                    OnGround = true,
                });

                Thread.Sleep(50);
            }
        }
    }
}
