using MCHexBOT.Utils;
using MCHexBOT.Network;
using MCHexBOT.Packets.Server.Handshake;
using MCHexBOT.Packets.Server.Login;
using System.Net.Sockets;
using MCHexBOT.Packets.Server.Play;
using MCHexBOT.Protocol;
using MCHexBOT.Features;

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

            PacketRegistry writer = new();
            PacketRegistry.RegisterServerPackets(writer, ProtocolVersion);

            PacketRegistry reader = new();
            PacketRegistry.RegisterClientPackets(reader, ProtocolVersion);

            MCConnection.WriterRegistry = writer;
            MCConnection.ReaderRegistry = reader;

            MCConnection.Handler = new PacketHandler(this)
            {
                Connection = MCConnection
            };

            MCConnection.Start();
            MCConnection.State = ConnectionState.Handshaking;

            Logger.LogWarning("Sending Handshake");

            MCConnection.SendPacket(new HandshakePacket()
            {
                NextState = HandshakeType.Login,
                ProtocolVersion = ProtocolVersion,
                ServerAddress = Host,
                ServerPort = (ushort)Port
            });

            MCConnection.State = ConnectionState.Login;

            Logger.LogWarning("Sending Login");

            MCConnection.SendPacket(new LoginStartPacket()
            {
                Username = APIClient.CurrentUser.name
            });

            Movement.MovementLoop(this);
            Combat.CombatLoop(this);
        }

        public void SendChat(string Message)
        {
            MCConnection.SendPacket(new ChatMessagePacket()
            {
                Message = Message
            });
        }

        public void SendRespawn()
        {
            MCConnection.SendPacket(new ClientStatusPacket()
            {
                ActionID = 0
            });
        }

        public void SendEntityAction(PlayerAction Action)
        {
            MCConnection.SendPacket(new EntityActionPacket()
            {
                EntityId = GetLocalPlayer().EntityID,
                ActionId = (int)Action,
                JumpBoost = Action == PlayerAction.StartHorseJump ? 100 : 0,
            });
        }

        public void SendEntityInteraction(int EntityID, bool Sneaking, EntityInteractHandType Hand, EntityInteractType Interact)
        {
            MCConnection.SendPacket(new InteractEntityPacket()
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
            MCConnection.SendPacket(new ClientSettingsPacket()
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

            MCConnection.SendPacket(new HeldItemChangePacket()
            {
                Slot = Slot
            });
        }
    }
}
