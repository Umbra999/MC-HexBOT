using MCHexBOT.Utils;
using MCHexBOT.Network;
using MCHexBOT.Pakets.Server.Handshake;
using MCHexBOT.Pakets.Server.Login;
using System.Net.Sockets;
using MCHexBOT.Pakets.Server.Play;
using MCHexBOT.Protocol;

namespace MCHexBOT.Core
{
    public class MinecraftClient
    {
        public readonly APIClient APIClient;
        public MinecraftConnection MCConnection;
        public List<Player> Players = new();

        public Player GetLocalPlayer()
        {
            foreach (Player player in Players)
            {
                if (player.PlayerInfo?.Name == APIClient.CurrentUser.name) return player;
            }

            Player Local = new() { IsLocal = true };
            Players.Add(Local);
            return Local;
        }

        public MinecraftClient(APIClient WebClient)
        {
            APIClient = WebClient;

            Logger.Log($"{APIClient.CurrentUser.name} connected as Bot");
        }

        public async void Connect(string Version, string Host, int Port)
        {
            if (MCConnection != null)
            {
                Logger.LogError($"Bot is already connected");
                return;
            }

            if (!Misc.ProtocolVersions.TryGetValue(Version, out int ProtocolVersion))
            {
                Logger.LogError($"{Version} is not Supported"); // replace by Server stats protocol once done
                return; 
            }

            Serverstats Stats = await APIClient.GetServerStats($"{Host}:{Port}");
            if (Stats == null)
            {
                Logger.LogError($"Failed to fetch Server");
                return;
            }

            if (Stats.status != "success")
            {
                Logger.LogError($"Server was unable to Ping");
                return;
            }

            TcpClient cl = new(Host, Port);

            MCConnection = new MinecraftConnection(cl);

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
                EntityId = GetLocalPlayer().EntityID,
                ActionId = (int)Action,
                JumpBoost = Action == PlayerAction.StartHorseJump ? 100 : 0,
            });
        }

        public void SendEntityInteraction(int EntityID, bool Sneaking, EntityInteractHandType Hand, EntityInteractType Interact)
        {
            MCConnection.SendPaket(new InteractEntityPaket()
            {
                EntityID = EntityID,
                Sneaking = Sneaking,
                InteractType = Interact,
                HandType = Hand,
                TargetX = 0,
                TargetY = 0,
                TargetZ = 0,
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

        public void SendHeldItemSlotSwitch(short Slot)
        {
            if (Slot < 0) Slot = 0;
            else if (Slot > 8) Slot = 8;

            MCConnection.SendPaket(new HeldItemChangePaket()
            {
                Slot = Slot
            });
        }

        public void SendMovement(bool X, bool Y, bool Z)
        {
            var x = GetLocalPlayer().Position.X + (X ? 0.25 : 0);
            var y = GetLocalPlayer().Position.Y + (Y ? 0.25 : 0);
            var z = GetLocalPlayer().Position.Z + (Z ? 0.25 : 0);

            MCConnection.SendPaket(new PlayerPositionAndRotationPaket()
            {
                X = x,
                Y = y,
                Z = z,
                Pitch = GetLocalPlayer().Rotation.Y,
                Yaw = GetLocalPlayer().Rotation.X,
                OnGround = GetLocalPlayer().IsOnGround,
            });

            GetLocalPlayer().Position = new System.Numerics.Vector3((float)x, (float)y, (float)z);
        }
    }
}
