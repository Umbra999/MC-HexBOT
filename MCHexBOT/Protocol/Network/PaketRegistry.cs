using System.Collections.Generic;
using MCHexBOT.Packets;
using MCHexBOT.Protocol;

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

        public void AddPacket(byte b, IPacket p, ConnectionState state)
        {
            Packets[state].Add(b, p);
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
            registry.AddPacket(0x00, new Packets.Client.Login.DisconnectPacket(), ConnectionState.Login);
            registry.AddPacket(0x01, new Packets.Client.Login.EncryptionRequestPacket(), ConnectionState.Login);
            registry.AddPacket(0x02, new Packets.Client.Login.LoginSuccessPacket(), ConnectionState.Login);
            registry.AddPacket(0x03, new Packets.Client.Login.SetCompressionPacket(), ConnectionState.Login);

            // Status
            registry.AddPacket(0x00, new Packets.Client.Status.StatusResponsePacket(), ConnectionState.Status);
            registry.AddPacket(0x01, new Packets.Client.Status.PongPacket(), ConnectionState.Status);

            // Play
            registry.AddPacket(0x00, new Packets.Client.Play.SpawnEntityPacket(), ConnectionState.Play);
            registry.AddPacket(0x02, new Packets.Client.Play.SpawnLivingEntity(), ConnectionState.Play);
            registry.AddPacket(0x26, new Packets.Client.Play.JoinGamePacket(), ConnectionState.Play);
            registry.AddPacket(0x21, new Packets.Client.Play.KeepAlivePacket(), ConnectionState.Play);
            registry.AddPacket(0x04, new Packets.Client.Play.SpawnPlayerPacket(), ConnectionState.Play);
            registry.AddPacket(0x08, new Packets.Client.Play.AcknowledgePlayerDiggingPacket(), ConnectionState.Play);
            registry.AddPacket(0x0C, new Packets.Client.Play.BlockChangePacket(), ConnectionState.Play);
            registry.AddPacket(0x30, new Packets.Client.Play.PingPacket(), ConnectionState.Play);
            registry.AddPacket(0x36, new Packets.Client.Play.PlayerInfoPacket(), ConnectionState.Play);
            registry.AddPacket(0x52, new Packets.Client.Play.UpdateHealthPacket(), ConnectionState.Play);
            registry.AddPacket(0x38, new Packets.Client.Play.PlayerPositionAndLookPacket(), ConnectionState.Play);
            registry.AddPacket(0x0F, new Packets.Client.Play.ChatMessagePacket(), ConnectionState.Play);
            registry.AddPacket(0x29, new Packets.Client.Play.EntityPositionPacket(), ConnectionState.Play);
            registry.AddPacket(0x2A, new Packets.Client.Play.EntityPositionAndRotationPacket(), ConnectionState.Play);
            registry.AddPacket(0x49, new Packets.Client.Play.UpdateViewPositionPacket(), ConnectionState.Play);
            registry.AddPacket(0x1D, new Packets.Client.Play.UnloadChunkPacket(), ConnectionState.Play);
            registry.AddPacket(0x4B, new Packets.Client.Play.SpawnPositionPacket(), ConnectionState.Play);
            registry.AddPacket(0x1A, new Packets.Client.Play.DisconnectPacket(), ConnectionState.Play);
            registry.AddPacket(0x35, new Packets.Client.Play.DeathCombatPacket(), ConnectionState.Play);
            registry.AddPacket(0x2B, new Packets.Client.Play.EntityRotationPacket(), ConnectionState.Play);
            registry.AddPacket(0x62, new Packets.Client.Play.EntityTeleportPacket(), ConnectionState.Play);
            registry.AddPacket(0x4F, new Packets.Client.Play.EntityVelocityPacket(), ConnectionState.Play);
            registry.AddPacket(0x3E, new Packets.Client.Play.EntityHeadLookPacket(), ConnectionState.Play);
            registry.AddPacket(0x18, new Packets.Client.Play.PluginMessagePacket(), ConnectionState.Play);
            registry.AddPacket(0x48, new Packets.Client.Play.SlotSelectionPacket(), ConnectionState.Play);
        }

        // Packets the server can understand
        public static void RegisterServerPackets(PacketRegistry registry, int ProtocolVersion)
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
            registry.AddPacket(0x0F, new Packets.Server.Play.KeepAlivePacket(), ConnectionState.Play);
            registry.AddPacket(0x00, new Packets.Server.Play.TeleportConfirmPacket(), ConnectionState.Play);
            registry.AddPacket(0x05, new Packets.Server.Play.ClientSettingsPacket(), ConnectionState.Play);
            registry.AddPacket(0x04, new Packets.Server.Play.ClientStatusPacket(), ConnectionState.Play);
            registry.AddPacket(0x03, new Packets.Server.Play.ChatMessagePacket(), ConnectionState.Play);
            registry.AddPacket(0x11, new Packets.Server.Play.PlayerPositionPacket(), ConnectionState.Play);
            registry.AddPacket(0x12, new Packets.Server.Play.PlayerPositionAndRotationPacket(), ConnectionState.Play);
            registry.AddPacket(0x1B, new Packets.Server.Play.EntityActionPacket(), ConnectionState.Play);
            registry.AddPacket(0x14, new Packets.Server.Play.PlayerMovementPacket(), ConnectionState.Play);
            registry.AddPacket(0x13, new Packets.Server.Play.PlayerRotationPacket(), ConnectionState.Play);
            registry.AddPacket(0x0D, new Packets.Server.Play.InteractEntityPacket(), ConnectionState.Play);
            registry.AddPacket(0x25, new Packets.Server.Play.HeldItemChangePacket(), ConnectionState.Play);
            registry.AddPacket(0x0A, new Packets.Server.Play.PluginMessagePacket(), ConnectionState.Play);
            registry.AddPacket(0x1D, new Packets.Server.Play.PongPacket(), ConnectionState.Play);
            registry.AddPacket(0x2C, new Packets.Server.Play.AnimationPacket(), ConnectionState.Play);
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
