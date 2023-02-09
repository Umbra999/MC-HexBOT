using HexBOT.Utils;
using HexBOT.Network;
using HexBOT.Packets.Server.Handshake;
using System.Net.Sockets;
using HexBOT.Packets.Server.Play;
using HexBOT.Protocol;
using HexBOT.Core.API;
using HexBOT.Core.Laby;
using HexBOT.Packets.Server.Login;
using HexBOT.Core.Minecraft.Helper;

namespace HexBOT.Core.Minecraft
{
    public class MinecraftClient
    {
        public ConnectionHandler MCConnection;

        public readonly APIClient APIClient;
        public LabyClient LabyClient;

        public EntityManager EntityManager;
        public NetworkManager NetworkManager;
        public ChatManager ChatManager;
        public MovementManager MovementManager;

        public MinecraftClient(APIClient WebClient)
        {
            APIClient = WebClient;
            LabyClient = new LabyClient(this);
            EntityManager = new EntityManager(this);
            NetworkManager = new NetworkManager(this);
            ChatManager = new ChatManager(this);
            MovementManager = new MovementManager(this);

            Logger.Log($"{APIClient.CurrentUser.name} connected as Bot");
        }

        public bool Connect(string Host, int Port)
        {
            try
            {
                Disconnect();

                TcpClient Client = new(Host, Port);

                MCConnection = new ConnectionHandler(Client, Protocol.ProtocolType.Minecraft);

                PacketRegistry writer = new();
                PacketRegistry.RegisterServerPackets(writer);

                PacketRegistry reader = new();
                PacketRegistry.RegisterClientPackets(reader);

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
                    ProtocolVersion = 47, // 1.8.X
                    ServerAddress = Host,
                    ServerPort = Convert.ToUInt16(Port)
                });

                MCConnection.State = ConnectionState.Login;

                MCConnection.SendPacket(new LoginStartPacket()
                {
                    Username = APIClient.CurrentUser.name
                });

                return true;
            }
            catch 
            {
                Logger.LogError($"Failed connecting to {Host}:{Port}");
                return false; 
            }
        }

        public void Disconnect()
        {
            if (MCConnection != null)
            {
                MCConnection.Stop();
                Logger.LogError($"{APIClient.CurrentUser.name} disconnected from Server");
                MCConnection = null;
            }
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
                ActionID = ClientStatusPacket.Action.Respawn
            });

            EntityManager.LocalPlayer.Health = 20;
            EntityManager.LocalPlayer.Saturation = 5;
            EntityManager.LocalPlayer.Food = 20;
        }

        public void SendEntityAction(EntityActionPacket.Action Action)
        {
            MCConnection.SendPacket(new EntityActionPacket()
            {
                EntityId = EntityManager.LocalPlayer.EntityID,
                ActionId = Action,
                HorseJumpBoost = Action == EntityActionPacket.Action.JumpWithHorse ? 100 : 0,
            });

            switch (Action)
            {
                case EntityActionPacket.Action.StartSneaking:
                    EntityManager.LocalPlayer.IsSneaking = true;
                    break;

                case EntityActionPacket.Action.StopSneaking:
                    EntityManager.LocalPlayer.IsSneaking = false;
                    break;

                case EntityActionPacket.Action.StartSprinting:
                    EntityManager.LocalPlayer.IsSprinting = true;
                    break;

                case EntityActionPacket.Action.StopSprinting:
                    EntityManager.LocalPlayer.IsSprinting = false;
                    break;
            }
        }

        public void SendEntityInteraction(int EntityID, UseEntityPacket.EntityInteractType Interact)
        {
            MCConnection.SendPacket(new UseEntityPacket()
            {
                EntityID = EntityID,
                InteractType = Interact,
                TargetX = 0,
                TargetY = 0,
                TargetZ = 0,
            });
        }

        public void SendPlayerSetings(bool ChatColors, ClientSettingsPacket.ChatType Chatmode, byte Skinparts, string LanguageTag, byte ViewDistance)
        {
            MCConnection.SendPacket(new ClientSettingsPacket()
            {
                ChatColors = ChatColors,
                ChatMode = Chatmode,
                DisplayedSkinParts = Skinparts,
                Locale = LanguageTag,
                ViewDistance = ViewDistance,
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

            EntityManager.LocalPlayer.HeldItemSlot = Slot;
        }

        public void SendAnimation()
        {
            MCConnection.SendPacket(new AnimationPacket()
            {
                
            });
        }
    }
}
