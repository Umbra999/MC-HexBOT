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

        public MinecraftClient(APIClient WebClient)
        {
            APIClient = WebClient;
            LabyClient = new LabyClient(this);

            Logger.Log($"{APIClient.CurrentUser.name} connected as Bot");
        }

        public async Task<bool> Connect(string Host, int Port)
        {
            if (MCConnection != null)
            {
                Logger.LogError($"Bot is already connected");
                return false;
            }

            int ProtocolVersion = 757;

            Serverstats Stats = await APIClient.GetServerStats($"{Host}:{Port}");
            if (Stats == null)
            {
                Logger.LogError($"Failed to fetch Server");
                return false;
            }

            if (Stats.status != "success")
            {
                Logger.LogError($"Server was unable to Ping");
                return false;
            }

            ServerAddress = Host + ":" + Port;
            Logger.LogWarning($"{ServerAddress} using Protocol {Stats.server.protocol}");

            TcpClient Client = new(Host, Port);

            MCConnection = new MinecraftConnection(Client, Protocol.ProtocolType.Minecraft);

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

            MCConnection.SendPacket(new HandshakePacket()
            {
                NextState = HandshakePacket.HandshakeType.Login,
                ProtocolVersion = ProtocolVersion,
                ServerAddress = Host,
                ServerPort = (ushort)Port
            });

            MCConnection.State = ConnectionState.Login;

            MCConnection.SendPacket(new LoginStartPacket()
            {
                Username = APIClient.CurrentUser.name
            });

            StartLoop();

            return true;
        }

        private void StartLoop()
        {
            //Task.Run(() => Movement.MovementLoop(this));
            Task.Run(() => Combat.CombatLoop(this));
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

        public void SendEntityAction(EntityActionPacket.Action Action)
        {
            MCConnection.SendPacket(new EntityActionPacket()
            {
                EntityId = GetLocalPlayer().EntityID,
                ActionId = Action,
                JumpBoost = Action == EntityActionPacket.Action.StartHorseJump ? 100 : 0,
            });

            switch (Action)
            {
                case EntityActionPacket.Action.StartSneaking:
                    GetLocalPlayer().IsSneaking = true;
                    break;

                case EntityActionPacket.Action.StopSneaking:
                    GetLocalPlayer().IsSneaking = false;
                    break;

                case EntityActionPacket.Action.StartSprinting:
                    GetLocalPlayer().IsSprinting = true;
                    break;

                case EntityActionPacket.Action.StopSprinting:
                    GetLocalPlayer().IsSprinting = false;
                    break;
            }
        }

        public void SendEntityInteraction(int EntityID, bool Sneaking, InteractEntityPacket.EntityInteractType Interact, InteractEntityPacket.EntityInteractHandType Hand)
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

        public void SendPlayerSetings(bool ServerListing, bool ChatColors, ClientSettingsPacket.ChatType Chatmode, byte Skinparts, ClientSettingsPacket.MainHandType MainHand, bool TextFiltering, string LanguageTag, byte ViewDistance)
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

        public void SendAnimation(AnimationPacket.HandType Hand)
        {
            MCConnection.SendPacket(new AnimationPacket()
            {
                Hand = Hand
            });
        }
    }
}
