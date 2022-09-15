using MCHexBOT.Utils;
using MCHexBOT.Network;
using MCHexBOT.Pakets.Server.Handshake;
using MCHexBOT.Pakets.Server.Login;
using System.Net.Sockets;
using MCHexBOT.Pakets.Server.Play;
using static MCHexBOT.Utils.GameTypes;
using MCHexBOT.Protocol;

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
            MCConnection.State = ConnectionState.Handshaking;

            Logger.LogWarning("Sending Handshake");

            MCConnection.SendPaket(new HandshakePaket()
            {
                NextState = HandshakeType.Login,
                ProtocolVersion = ProtocolVersion,
                ServerAddress = Host,
                ServerPort = (ushort)Port
            });

            MCConnection.State = ConnectionState.Login;

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

        public void SendEntityAction(PlayerAction Action)
        {
            MCConnection.SendPaket(new EntityActionPaket()
            {
                EntityId = CurrentPlayer.EntityID,
                ActionId = (int)Action,
                JumpBoost = Action == PlayerAction.StartHorseJump ? 100 : 0,
            });
        }

        public void SendPlayerSetings(bool ServerListing, bool ChatColors, ChatMode Chatmode, byte Skinparts, MainHandType MainHand, bool TextFiltering, string LanguageTag, byte ViewDistance)
        {
            MCConnection.SendPaket(new ClientSettingsPaket()
            {
                AllowServerListings = ServerListing,
                ChatColors = ChatColors,
                ChatMode = Chatmode,
                DisplayedSkinParts = Skinparts,
                MainHand = MainHand,
                EnableTextFiltering = TextFiltering,
                Locale = LanguageTag,
                ViewDistance = ViewDistance
            });
        }
    }
}
