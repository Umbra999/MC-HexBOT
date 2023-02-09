using HexBOT.Packets;
using HexBOT.Protocol;

namespace HexBOT.Network
{
    public class PacketRegistry
    {
        public Dictionary<ConnectionState, Dictionary<byte, IPacket>> Packets = new();

        public PacketRegistry()
        {
            Packets.Add(ConnectionState.Handshaking, new Dictionary<byte, IPacket>());
            Packets.Add(ConnectionState.Status, new Dictionary<byte, IPacket>());
            Packets.Add(ConnectionState.Login, new Dictionary<byte, IPacket>());
            Packets.Add(ConnectionState.Play, new Dictionary<byte, IPacket>());
        }

        public void AddPacket(byte? b, IPacket p, ConnectionState state)
        {
            if (b == null) return;
            Packets[state].Add((byte)b, p);
        }

        public byte GetPacketId(IPacket Packet, ConnectionState state)
        {
            byte res = 0x00;

            var csd = Packets[state];

            foreach(var b in csd.Keys)
            {
                var pt = csd[b].GetType();

                if(pt.Name == Packet.GetType().Name)
                {
                    res = b;
                    break;
                }
            }

            return res;
        }

        // Packets the client can understand
        public static void RegisterClientPackets(PacketRegistry registry)
        {
            // Login
            registry.AddPacket(0x00, new Packets.Client.Login.DisconnectPacket(), ConnectionState.Login);
            registry.AddPacket(0x01, new Packets.Client.Login.EncryptionRequestPacket(), ConnectionState.Login);
            registry.AddPacket(0x02, new Packets.Client.Login.LoginSuccessPacket(), ConnectionState.Login);
            registry.AddPacket(0x03, new Packets.Client.Login.SetCompressionPacket(), ConnectionState.Login);

            // Status
            registry.AddPacket(0x00, new Packets.Client.Status.StatusResponsePacket(), ConnectionState.Status);
            registry.AddPacket(0x01, new Packets.Client.Status.PongPacket(), ConnectionState.Status);

            // Play
            registry.AddPacket(0x00, new Packets.Client.Play.KeepAlivePacket(), ConnectionState.Play);
            registry.AddPacket(0x01, new Packets.Client.Play.JoinGamePacket(), ConnectionState.Play);
            registry.AddPacket(0x02, new Packets.Client.Play.ChatMessagePacket(), ConnectionState.Play);
            registry.AddPacket(0x03, new Packets.Client.Play.TimeUpdatePacket(), ConnectionState.Play);
            //registry.AddPacket(0x04, new Packets.Client.Play.EntityEquipmentPacket(), ConnectionState.Play);
            registry.AddPacket(0x05, new Packets.Client.Play.SpawnPositionPacket(), ConnectionState.Play);
            registry.AddPacket(0x06, new Packets.Client.Play.UpdateHealthPacket(), ConnectionState.Play);
            registry.AddPacket(0x07, new Packets.Client.Play.RespawnPacket(), ConnectionState.Play);
            registry.AddPacket(0x08, new Packets.Client.Play.PlayerPositionAndLookPacket(), ConnectionState.Play);
            registry.AddPacket(0x09, new Packets.Client.Play.HeldItemChangePacket(), ConnectionState.Play);
            registry.AddPacket(0x0B, new Packets.Client.Play.AnimationPacket(), ConnectionState.Play);
            registry.AddPacket(0x0C, new Packets.Client.Play.SpawnPlayerPacket(), ConnectionState.Play);
            registry.AddPacket(0x0D, new Packets.Client.Play.CollectItemPacket(), ConnectionState.Play);
            registry.AddPacket(0x0E, new Packets.Client.Play.SpawnObjectPacket(), ConnectionState.Play);
            registry.AddPacket(0x0F, new Packets.Client.Play.SpawnMobPacket(), ConnectionState.Play);
            registry.AddPacket(0x12, new Packets.Client.Play.EntityVelocityPacket(), ConnectionState.Play);
            registry.AddPacket(0x13, new Packets.Client.Play.DestroyEntitiesPacket(), ConnectionState.Play);
            registry.AddPacket(0x14, new Packets.Client.Play.EntityPacket(), ConnectionState.Play);
            registry.AddPacket(0x15, new Packets.Client.Play.EntityRelativeMovementPacket(), ConnectionState.Play);
            registry.AddPacket(0x16, new Packets.Client.Play.EntityRotationPacket(), ConnectionState.Play);
            registry.AddPacket(0x17, new Packets.Client.Play.EntityRelativeMovementAndRotationPacket(), ConnectionState.Play);
            registry.AddPacket(0x18, new Packets.Client.Play.EntityTeleportPacket(), ConnectionState.Play);
            registry.AddPacket(0x19, new Packets.Client.Play.EntityHeadLookPacket(), ConnectionState.Play);

            registry.AddPacket(0x23, new Packets.Client.Play.BlockChangePacket(), ConnectionState.Play);

            registry.AddPacket(0x3F, new Packets.Client.Play.PluginMessagePacket(), ConnectionState.Play);
            registry.AddPacket(0x40, new Packets.Client.Play.DisconnectPacket(), ConnectionState.Play);

            //registry.AddPacket(0x38, new Packets.Client.Play.PlayerListItemPacket(), ConnectionState.Play);
        }

        // Packets the server can understand
        public static void RegisterServerPackets(PacketRegistry registry)
        {
            // Handshake
            registry.AddPacket(0x00, new Packets.Server.Handshake.HandshakePacket(), ConnectionState.Handshaking);

            // Login
            registry.AddPacket(0x00, new Packets.Server.Login.LoginStartPacket(), ConnectionState.Login);
            registry.AddPacket(0x01, new Packets.Server.Login.EncryptionResponsePacket(), ConnectionState.Login);

            // Status
            registry.AddPacket(0x00, new Packets.Server.Status.StatusRequestPacket(), ConnectionState.Status);
            registry.AddPacket(0x01, new Packets.Server.Status.PingPacket(), ConnectionState.Status);

            // Play
            registry.AddPacket(0x00, new Packets.Server.Play.KeepAlivePacket(), ConnectionState.Play);
            registry.AddPacket(0x01, new Packets.Server.Play.ChatMessagePacket(), ConnectionState.Play);
            registry.AddPacket(0x02, new Packets.Server.Play.UseEntityPacket(), ConnectionState.Play);
            registry.AddPacket(0x03, new Packets.Server.Play.PlayerMovementPacket(), ConnectionState.Play);
            registry.AddPacket(0x04, new Packets.Server.Play.PlayerPositionPacket(), ConnectionState.Play);
            registry.AddPacket(0x05, new Packets.Server.Play.PlayerRotationPacket(), ConnectionState.Play);
            registry.AddPacket(0x06, new Packets.Server.Play.PlayerPositionAndRotationPacket(), ConnectionState.Play);
            registry.AddPacket(0x09, new Packets.Server.Play.HeldItemChangePacket(), ConnectionState.Play);
            registry.AddPacket(0x0A, new Packets.Server.Play.AnimationPacket(), ConnectionState.Play);
            registry.AddPacket(0x0B, new Packets.Server.Play.EntityActionPacket(), ConnectionState.Play);
            registry.AddPacket(0x15, new Packets.Server.Play.ClientSettingsPacket(), ConnectionState.Play);
            registry.AddPacket(0x16, new Packets.Server.Play.ClientStatusPacket(), ConnectionState.Play);
            registry.AddPacket(0x17, new Packets.Server.Play.PluginMessagePacket(), ConnectionState.Play);
        }

        public static void RegisterLabyClientPackets(PacketRegistry registry)
        {
            // Handshake
            registry.AddPacket(0x01, new Protocol.Packets.LabyClient.Handshake.HelloPacket(), ConnectionState.Handshaking);

            // Login
            registry.AddPacket(0x0A, new Protocol.Packets.LabyClient.Login.EncryptionRequestPacket(), ConnectionState.Login);
            registry.AddPacket(0x07, new Protocol.Packets.LabyClient.Login.LoginCompletePacket(), ConnectionState.Login);

            // Play
            registry.AddPacket(0x3E, new Protocol.Packets.LabyClient.Play.PingPacket(), ConnectionState.Play);
        }

        public static void RegisterLabyServerPackets(PacketRegistry registry)
        {
            // Handshake
            registry.AddPacket(0x00, new Protocol.Packets.LabyServer.Handshake.HelloPacket(), ConnectionState.Handshaking);

            // Login
            registry.AddPacket(0x02, new Protocol.Packets.LabyServer.Login.LoginStartPacket(), ConnectionState.Login);
            registry.AddPacket(0x03, new Protocol.Packets.LabyServer.Login.LoginDataPacket(), ConnectionState.Login);
            registry.AddPacket(0x06, new Protocol.Packets.LabyServer.Login.LoginOptionPacket(), ConnectionState.Login);
            registry.AddPacket(0x09, new Protocol.Packets.LabyServer.Login.LoginVersionPacket(), ConnectionState.Login);
            registry.AddPacket(0xB,  new Protocol.Packets.LabyServer.Login.EncryptionResponsePacket(), ConnectionState.Login);

            // Play
            registry.AddPacket(0x3F, new Protocol.Packets.LabyServer.Play.PongPacket(), ConnectionState.Play);
            registry.AddPacket(0x44, new Protocol.Packets.LabyServer.Play.PlayServerPacket(), ConnectionState.Play);
            registry.AddPacket(0x10, new Protocol.Packets.LabyServer.Play.FriendRequestPacket(), ConnectionState.Play);
            registry.AddPacket(0x12, new Protocol.Packets.LabyServer.Play.FriendRemovePacket(), ConnectionState.Play);
        }

        public static void RegisterVoiceClientPackets(PacketRegistry registry)
        {
            // Handshake

            // Login

            // Play
        }

        public static void RegisterVoiceServerPackets(PacketRegistry registry)
        {
            // Handshake
            registry.AddPacket(0x00, new Protocol.Packets.LabyVoiceServer.HandshakePacket(), ConnectionState.Handshaking);

            // Login

            // Play
        }
    }
}
