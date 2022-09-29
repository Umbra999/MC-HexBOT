using MCHexBOT.Packets;
using MCHexBOT.Protocol;
using MCHexBOT.Protocol.Network;

namespace MCHexBOT.Network
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
        public static void RegisterClientPackets(PacketRegistry registry, int ProtocolVersion)
        {
            // Login
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Login.DisconnectPacket()), new Packets.Client.Login.DisconnectPacket(), ConnectionState.Login);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Login.EncryptionRequestPacket()), new Packets.Client.Login.EncryptionRequestPacket(), ConnectionState.Login);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Login.LoginSuccessPacket()), new Packets.Client.Login.LoginSuccessPacket(), ConnectionState.Login);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Login.SetCompressionPacket()), new Packets.Client.Login.SetCompressionPacket(), ConnectionState.Login);

            // Status
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Status.StatusResponsePacket()), new Packets.Client.Status.StatusResponsePacket(), ConnectionState.Status);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Status.PongPacket()), new Packets.Client.Status.PongPacket(), ConnectionState.Status);

            // Play
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Play.SpawnEntityPacket()), new Packets.Client.Play.SpawnEntityPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Play.SpawnLivingEntity()), new Packets.Client.Play.SpawnLivingEntity(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Play.JoinGamePacket()), new Packets.Client.Play.JoinGamePacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Play.KeepAlivePacket()), new Packets.Client.Play.KeepAlivePacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Play.SpawnPlayerPacket()), new Packets.Client.Play.SpawnPlayerPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Play.AcknowledgePlayerDiggingPacket()), new Packets.Client.Play.AcknowledgePlayerDiggingPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Play.BlockChangePacket()), new Packets.Client.Play.BlockChangePacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Play.PingPacket()), new Packets.Client.Play.PingPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Play.PlayerInfoPacket()), new Packets.Client.Play.PlayerInfoPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Play.UpdateHealthPacket()), new Packets.Client.Play.UpdateHealthPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Play.PlayerPositionAndLookPacket()), new Packets.Client.Play.PlayerPositionAndLookPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Play.ChatMessagePacket()), new Packets.Client.Play.ChatMessagePacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Play.EntityPositionPacket()), new Packets.Client.Play.EntityPositionPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Play.EntityPositionAndRotationPacket()), new Packets.Client.Play.EntityPositionAndRotationPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Play.UpdateViewPositionPacket()), new Packets.Client.Play.UpdateViewPositionPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Play.UnloadChunkPacket()), new Packets.Client.Play.UnloadChunkPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Play.SpawnPositionPacket()), new Packets.Client.Play.SpawnPositionPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Play.DisconnectPacket()), new Packets.Client.Play.DisconnectPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Play.DeathCombatPacket()), new Packets.Client.Play.DeathCombatPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Play.EntityRotationPacket()), new Packets.Client.Play.EntityRotationPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Play.EntityTeleportPacket()), new Packets.Client.Play.EntityTeleportPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Play.EntityVelocityPacket()), new Packets.Client.Play.EntityVelocityPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Play.EntityHeadLookPacket()), new Packets.Client.Play.EntityHeadLookPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Play.PluginMessagePacket()), new Packets.Client.Play.PluginMessagePacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Play.SlotSelectionPacket()), new Packets.Client.Play.SlotSelectionPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Play.EntityAnimationPacket()), new Packets.Client.Play.EntityAnimationPacket(), ConnectionState.Play);
            //registry.AddPacket(PacketMapping.GetClientPacketID(ProtocolVersion, new Packets.Client.Play.EntityMetadataPacket()), new Packets.Client.Play.EntityMetadataPacket(), ConnectionState.Play);
        }

        // Packets the server can understand
        public static void RegisterServerPackets(PacketRegistry registry, int ProtocolVersion)
        {
            // Handshake
            registry.AddPacket(PacketMapping.GetServerPacketID(ProtocolVersion, new Packets.Server.Handshake.HandshakePacket()), new Packets.Server.Handshake.HandshakePacket(), ConnectionState.Handshaking);

            // Login
            registry.AddPacket(PacketMapping.GetServerPacketID(ProtocolVersion, new Packets.Server.Login.LoginStartPacket()), new Packets.Server.Login.LoginStartPacket(), ConnectionState.Login);
            registry.AddPacket(PacketMapping.GetServerPacketID(ProtocolVersion, new Packets.Server.Login.EncryptionResponsePacket()), new Packets.Server.Login.EncryptionResponsePacket(), ConnectionState.Login);

            // Status
            registry.AddPacket(PacketMapping.GetServerPacketID(ProtocolVersion, new Packets.Server.Status.StatusRequestPacket()), new Packets.Server.Status.StatusRequestPacket(), ConnectionState.Status);
            registry.AddPacket(PacketMapping.GetServerPacketID(ProtocolVersion, new Packets.Server.Status.PingPacket()), new Packets.Server.Status.PingPacket(), ConnectionState.Status);

            // Play
            registry.AddPacket(PacketMapping.GetServerPacketID(ProtocolVersion, new Packets.Server.Play.KeepAlivePacket()), new Packets.Server.Play.KeepAlivePacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetServerPacketID(ProtocolVersion, new Packets.Server.Play.TeleportConfirmPacket()), new Packets.Server.Play.TeleportConfirmPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetServerPacketID(ProtocolVersion, new Packets.Server.Play.ClientSettingsPacket()), new Packets.Server.Play.ClientSettingsPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetServerPacketID(ProtocolVersion, new Packets.Server.Play.ClientStatusPacket()), new Packets.Server.Play.ClientStatusPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetServerPacketID(ProtocolVersion, new Packets.Server.Play.ChatMessagePacket()), new Packets.Server.Play.ChatMessagePacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetServerPacketID(ProtocolVersion, new Packets.Server.Play.PlayerPositionPacket()), new Packets.Server.Play.PlayerPositionPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetServerPacketID(ProtocolVersion, new Packets.Server.Play.PlayerPositionAndRotationPacket()), new Packets.Server.Play.PlayerPositionAndRotationPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetServerPacketID(ProtocolVersion, new Packets.Server.Play.EntityActionPacket()), new Packets.Server.Play.EntityActionPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetServerPacketID(ProtocolVersion, new Packets.Server.Play.PlayerMovementPacket()), new Packets.Server.Play.PlayerMovementPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetServerPacketID(ProtocolVersion, new Packets.Server.Play.PlayerRotationPacket()), new Packets.Server.Play.PlayerRotationPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetServerPacketID(ProtocolVersion, new Packets.Server.Play.InteractEntityPacket()), new Packets.Server.Play.InteractEntityPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetServerPacketID(ProtocolVersion, new Packets.Server.Play.HeldItemChangePacket()), new Packets.Server.Play.HeldItemChangePacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetServerPacketID(ProtocolVersion, new Packets.Server.Play.PluginMessagePacket()), new Packets.Server.Play.PluginMessagePacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetServerPacketID(ProtocolVersion, new Packets.Server.Play.PongPacket()), new Packets.Server.Play.PongPacket(), ConnectionState.Play);
            registry.AddPacket(PacketMapping.GetServerPacketID(ProtocolVersion, new Packets.Server.Play.AnimationPacket()), new Packets.Server.Play.AnimationPacket(), ConnectionState.Play);
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
