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
        public LabyClient LabyClient; 

        public List<Player> Players = new();
        private Player LocalPlayer;

        public string ServerAddress;

        public Player GetLocalPlayer()
        {
            if (LocalPlayer == null)
            {
                LocalPlayer = new() { IsLocal = true };
                Players.Add(LocalPlayer);
            }
            return LocalPlayer;
        }

        public MinecraftClient(APIClient WebClient, LabyClient LabySession)
        {
            APIClient = WebClient;
            LabyClient = LabySession;

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

            ServerAddress = Host + ":" + Port;

            TcpClient Client = new(Host, Port);

            MCConnection = new MinecraftConnection(Client, true);

            PacketRegistry writer = new();
            PacketRegistry.RegisterServerPackets(writer, ProtocolVersion);

            PacketRegistry reader = new();
            PacketRegistry.RegisterClientPackets(reader, ProtocolVersion);

            MCConnection.WriterRegistry = writer;
            MCConnection.ReaderRegistry = reader;

            MCConnection.Handler = new MinecraftHandler(this)
            {
                Connection = MCConnection
            };

            MCConnection.Start();
            MCConnection.State = ConnectionState.Handshaking;

            MCConnection.SendPacket(new HandshakePacket()
            {
                NextState = HandshakeType.Login,
                ProtocolVersion = ProtocolVersion,
                ServerAddress = Host,
                ServerPort = (ushort)Port
            });

            MCConnection.State = ConnectionState.Login;

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

            GetLocalPlayer().Health = 20;
            GetLocalPlayer().Saturation = 5;
            GetLocalPlayer().Food = 20;
        }

        public void SendEntityAction(PlayerAction Action)
        {
            MCConnection.SendPacket(new EntityActionPacket()
            {
                EntityId = GetLocalPlayer().EntityID,
                ActionId = (int)Action,
                JumpBoost = Action == PlayerAction.StartHorseJump ? 100 : 0,
            });

            switch (Action)
            {
                case PlayerAction.StartSneaking:
                    GetLocalPlayer().IsSneaking = true;
                    break;

                case PlayerAction.StopSneaking:
                    GetLocalPlayer().IsSneaking = false;
                    break;

                case PlayerAction.StartSprinting:
                    GetLocalPlayer().IsSprinting = true;
                    break;

                case PlayerAction.StopSprinting:
                    GetLocalPlayer().IsSprinting = false;
                    break;
            }
        }

        public void SendEntityInteraction(int EntityID, bool Sneaking, EntityInteractType Interact, EntityInteractHandType Hand)
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

            GetLocalPlayer().HeldItemSlot = Slot;
        }
    }
}
