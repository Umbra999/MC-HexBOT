using HexBOT.Packets;
using HexBOT.Utils;

namespace HexBOT.Protocol.Network
{
    internal class PacketMapping
    {
        public static readonly int DefaultProtocol = 757;
        public static readonly int[] SupportedProtocols = new int[] { 340, 757 };

        public static byte? GetServerPacketID(int Protocol, IPacket packet)
        {
            switch (packet)
            {
                case HexBOT.Packets.Server.Handshake.HandshakePacket:
                    if (Protocol == 757 || Protocol == 340) return 0x00;
                    break;

                case HexBOT.Packets.Server.Login.LoginStartPacket:
                    if (Protocol == 757 || Protocol == 340) return 0x00;
                    break;

                case HexBOT.Packets.Server.Login.EncryptionResponsePacket:
                    if (Protocol == 757 || Protocol == 340 ) return 0x01;
                    break;

                case HexBOT.Packets.Server.Status.StatusRequestPacket:
                    if (Protocol == 757 || Protocol == 340 ) return 0x00;
                    break;

                case HexBOT.Packets.Server.Status.PingPacket:
                    if (Protocol == 757 || Protocol == 340) return 0x01;
                    break;

                case HexBOT.Packets.Server.Play.KeepAlivePacket:
                    if (Protocol == 757) return 0x0F;
                    if (Protocol == 340) return 0x0B;
                    break;

                case HexBOT.Packets.Server.Play.TeleportConfirmPacket:
                    if (Protocol == 757 || Protocol == 340 ) return 0x00;
                    break;

                case HexBOT.Packets.Server.Play.ClientSettingsPacket:
                    if (Protocol == 757) return 0x05;
                    if (Protocol == 340) return 0x04;
                    break;

                case HexBOT.Packets.Server.Play.ClientStatusPacket:
                    if (Protocol == 757) return 0x04;
                    if (Protocol == 340) return 0x03;
                    break;

                case HexBOT.Packets.Server.Play.ChatMessagePacket:
                    if (Protocol == 757) return 0x03;
                    if (Protocol == 340) return 0x02;
                    break;

                case HexBOT.Packets.Server.Play.PlayerPositionPacket:
                    if (Protocol == 757) return 0x11;
                    if (Protocol == 340) return 0x0D;
                    break;

                case HexBOT.Packets.Server.Play.PlayerPositionAndRotationPacket:
                    if (Protocol == 757) return 0x12;
                    if (Protocol == 340) return 0x0E;
                    break;

                case HexBOT.Packets.Server.Play.EntityActionPacket:
                    if (Protocol == 757) return 0x1B;
                    if (Protocol == 340) return 0x15;
                    break;

                case HexBOT.Packets.Server.Play.PlayerMovementPacket:
                    if (Protocol == 757) return 0x14;
                    if (Protocol == 340) return 0x0C;
                    break;

                case HexBOT.Packets.Server.Play.PlayerRotationPacket:
                    if (Protocol == 757) return 0x13;
                    if (Protocol == 340) return 0x0F;
                    break;

                case HexBOT.Packets.Server.Play.InteractEntityPacket:
                    if (Protocol == 757) return 0x0D;
                    if (Protocol == 340) return 0x0A;
                    break;

                case HexBOT.Packets.Server.Play.HeldItemChangePacket:
                    if (Protocol == 757) return 0x25;
                    if (Protocol == 340) return 0x1A;
                    break;

                case HexBOT.Packets.Server.Play.PluginMessagePacket:
                    if (Protocol == 757) return 0x0A;
                    if (Protocol == 340) return 0x09;
                    break;

                case HexBOT.Packets.Server.Play.PongPacket:
                    if (Protocol == 757) return 0x1D;
                    if (Protocol == 340) return null;
                    break;

                case HexBOT.Packets.Server.Play.AnimationPacket:
                    if (Protocol == 757) return 0x2C;
                    if (Protocol == 340) return 0x1D;
                    break;
            }

            Logger.LogError($"{packet} for Protocol {Protocol} not Implemented");
            return null;
        }

        public static byte? GetClientPacketID(int Protocol, IPacket packet)
        {
            switch (packet)
            {
                case HexBOT.Packets.Client.Login.DisconnectPacket:
                    if (Protocol == 757 || Protocol == 340) return 0x00;
                    break;

                case HexBOT.Packets.Client.Login.EncryptionRequestPacket:
                    if (Protocol == 757 || Protocol == 340) return 0x01;
                    break;

                case HexBOT.Packets.Client.Login.LoginSuccessPacket:
                    if (Protocol == 757 || Protocol == 340) return 0x02;
                    break;

                case HexBOT.Packets.Client.Login.SetCompressionPacket:
                    if (Protocol == 757 || Protocol == 340) return 0x03;
                    break;

                case HexBOT.Packets.Client.Status.StatusResponsePacket:
                    if (Protocol == 757 || Protocol == 340) return 0x00;
                    break;

                case HexBOT.Packets.Client.Status.PongPacket:
                    if (Protocol == 757 || Protocol == 340) return 0x01;
                    break;

                case HexBOT.Packets.Client.Play.SpawnEntityPacket:
                    if (Protocol == 757 || Protocol == 340) return 0x00;
                    break;

                case HexBOT.Packets.Client.Play.SpawnLivingEntity:
                    if (Protocol == 757) return 0x02;
                    if (Protocol == 340) return 0x03;
                    break;

                case HexBOT.Packets.Client.Play.JoinGamePacket:
                    if (Protocol == 757) return 0x26;
                    if (Protocol == 340) return 0x23;
                    break;

                case HexBOT.Packets.Client.Play.KeepAlivePacket:
                    if (Protocol == 757) return 0x21;
                    if (Protocol == 340) return 0x1F;
                    break;

                case HexBOT.Packets.Client.Play.SpawnPlayerPacket:
                    if (Protocol == 757) return 0x04;
                    if (Protocol == 340) return 0x05;
                    break;

                case HexBOT.Packets.Client.Play.AcknowledgePlayerDiggingPacket:
                    if (Protocol == 757) return 0x08;
                    if (Protocol == 340) return 0x14;
                    break;

                case HexBOT.Packets.Client.Play.BlockChangePacket:
                    if (Protocol == 757) return 0x0C;
                    if (Protocol == 340) return 0x0B;
                    break;

                case HexBOT.Packets.Client.Play.PingPacket:
                    if (Protocol == 757) return 0x30;
                    if (Protocol == 340) return null;
                    break;

                case HexBOT.Packets.Client.Play.PlayerInfoPacket:
                    if (Protocol == 757) return 0x36;
                    if (Protocol == 340) return 0x2E;
                    break;

                case HexBOT.Packets.Client.Play.UpdateHealthPacket:
                    if (Protocol == 757) return 0x52;
                    if (Protocol == 340) return 0x41;
                    break;

                case HexBOT.Packets.Client.Play.PlayerPositionAndLookPacket:
                    if (Protocol == 757) return 0x38;
                    if (Protocol == 340) return 0x2F;
                    break;

                case HexBOT.Packets.Client.Play.ChatMessagePacket:
                    if (Protocol == 757 || Protocol == 340) return 0x0F;
                    break;

                case HexBOT.Packets.Client.Play.EntityPositionPacket:
                    if (Protocol == 757) return 0x29;
                    if (Protocol == 340) return 0x26;
                    break;

                case HexBOT.Packets.Client.Play.EntityPositionAndRotationPacket:
                    if (Protocol == 757) return 0x2A;
                    if (Protocol == 340) return 0x27;
                    break;

                case HexBOT.Packets.Client.Play.UpdateViewPositionPacket:
                    if (Protocol == 757) return 0x49;
                    if (Protocol == 340) return null;
                    break;

                case HexBOT.Packets.Client.Play.UnloadChunkPacket:
                    if (Protocol == 757 || Protocol == 340) return 0x1D;
                    break;

                case HexBOT.Packets.Client.Play.SpawnPositionPacket:
                    if (Protocol == 757) return 0x4B;
                    if (Protocol == 340) return 0x46;
                    break;

                case HexBOT.Packets.Client.Play.DisconnectPacket:
                    if (Protocol == 757 || Protocol == 340) return 0x1A;
                    break;

                case HexBOT.Packets.Client.Play.DeathCombatPacket:
                    if (Protocol == 757) return 0x35;
                    if (Protocol == 340) return null;
                    break;

                case HexBOT.Packets.Client.Play.EntityRotationPacket:
                    if (Protocol == 757) return 0x2B;
                    if (Protocol == 340) return 0x28;
                    break;

                case HexBOT.Packets.Client.Play.EntityTeleportPacket:
                    if (Protocol == 757) return 0x62;
                    if (Protocol == 340) return 0x41;
                    break;

                case HexBOT.Packets.Client.Play.EntityVelocityPacket:
                    if (Protocol == 757) return 0x4F;
                    if (Protocol == 340) return 0x3E;
                    break;

                case HexBOT.Packets.Client.Play.EntityHeadLookPacket:
                    if (Protocol == 757) return 0x3E;
                    if (Protocol == 340) return 0x36;
                    break;

                case HexBOT.Packets.Client.Play.PluginMessagePacket:
                    if (Protocol == 757 || Protocol == 340) return 0x18;
                    break;

                case HexBOT.Packets.Client.Play.SlotSelectionPacket:
                    if (Protocol == 757) return 0x48;
                    if (Protocol == 340) return 0x3A;
                    break;

                case HexBOT.Packets.Client.Play.EntityAnimationPacket:
                    if (Protocol == 757 || Protocol == 340) return 0x06;
                    break;

                case HexBOT.Packets.Client.Play.EntityMetadataPacket:
                    if (Protocol == 757) return 0x4D;
                    if (Protocol == 340) return 0x3C;
                    break;
            }

            Logger.LogError($"{packet} for Protocol {Protocol} not Implemented");
            return null;
        }
    }
}
