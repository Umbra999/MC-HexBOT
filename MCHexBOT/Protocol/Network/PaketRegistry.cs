using System.Collections.Generic;
using MCHexBOT.Pakets;
using MCHexBOT.Protocol;

namespace MCHexBOT.Network
{
    public class PaketRegistry
    {
        public Dictionary<ConnectionState, Dictionary<byte, IPaket>> Pakets = new();

        public PaketRegistry()
        {
            Pakets.Add(ConnectionState.Handshaking, new Dictionary<byte, IPaket>());
            Pakets.Add(ConnectionState.Status, new Dictionary<byte, IPaket>());
            Pakets.Add(ConnectionState.Login, new Dictionary<byte, IPaket>());
            Pakets.Add(ConnectionState.Play, new Dictionary<byte, IPaket>());
        }

        public void AddPaket(byte b, IPaket p, ConnectionState state)
        {
            Pakets[state].Add(b, p);
        }

        public byte GetPaketId(IPaket paket, ConnectionState state)
        {
            byte res = 0x00;

            var csd = Pakets[state];

            foreach(var b in csd.Keys)
            {
                var pt = csd[b].GetType();

                if(pt.Name == paket.GetType().Name)
                {
                    res = b;
                    break;
                }
            }

            return res;
        }

        // Pakets the client can understand
        public static void RegisterClientPakets(PaketRegistry registry, int ProtocolVersion)
        {
            // Login
            registry.AddPaket(0x00, new Pakets.Client.Login.DisconnectPaket(), ConnectionState.Login);
            registry.AddPaket(0x01, new Pakets.Client.Login.EncryptionRequestPaket(), ConnectionState.Login);
            registry.AddPaket(0x02, new Pakets.Client.Login.LoginSuccessPaket(), ConnectionState.Login);
            registry.AddPaket(0x03, new Pakets.Client.Login.SetCompressionPaket(), ConnectionState.Login);

            // Status
            registry.AddPaket(0x00, new Pakets.Client.Status.StatusResponsePaket(), ConnectionState.Status);
            registry.AddPaket(0x01, new Pakets.Client.Status.PongPaket(), ConnectionState.Status);

            // Play
            registry.AddPaket(0x00, new Pakets.Client.Play.SpawnEntityPaket(), ConnectionState.Play);
            registry.AddPaket(0x02, new Pakets.Client.Play.SpawnLivingEntity(), ConnectionState.Play);
            registry.AddPaket(0x26, new Pakets.Client.Play.JoinGamePaket(), ConnectionState.Play);
            registry.AddPaket(0x21, new Pakets.Client.Play.KeepAlivePaket(), ConnectionState.Play);
            registry.AddPaket(0x04, new Pakets.Client.Play.SpawnPlayerPaket(), ConnectionState.Play);
            registry.AddPaket(0x08, new Pakets.Client.Play.AcknowledgePlayerDiggingPaket(), ConnectionState.Play);
            registry.AddPaket(0x0C, new Pakets.Client.Play.BlockChangePaket(), ConnectionState.Play);
            registry.AddPaket(0x30, new Pakets.Client.Play.PingPaket(), ConnectionState.Play);
            registry.AddPaket(0x36, new Pakets.Client.Play.PlayerInfoPaket(), ConnectionState.Play);
            registry.AddPaket(0x52, new Pakets.Client.Play.UpdateHealthPaket(), ConnectionState.Play);
            registry.AddPaket(0x38, new Pakets.Client.Play.PlayerPositionAndLookPaket(), ConnectionState.Play);
            registry.AddPaket(0x0F, new Pakets.Client.Play.ChatMessagePaket(), ConnectionState.Play);
            registry.AddPaket(0x29, new Pakets.Client.Play.EntityPositionPaket(), ConnectionState.Play);
            registry.AddPaket(0x2A, new Pakets.Client.Play.EntityPositionAndRotationPaket(), ConnectionState.Play);
            registry.AddPaket(0x49, new Pakets.Client.Play.UpdateViewPositionPaket(), ConnectionState.Play);
            registry.AddPaket(0x1D, new Pakets.Client.Play.UnloadChunkPaket(), ConnectionState.Play);
            registry.AddPaket(0x4B, new Pakets.Client.Play.SpawnPositionPaket(), ConnectionState.Play);
            registry.AddPaket(0x1A, new Pakets.Client.Play.DisconnectPaket(), ConnectionState.Play);
            registry.AddPaket(0x35, new Pakets.Client.Play.DeathCombatPaket(), ConnectionState.Play);
            registry.AddPaket(0x2B, new Pakets.Client.Play.EntityRotationPaket(), ConnectionState.Play);
            registry.AddPaket(0x62, new Pakets.Client.Play.EntityTeleportPaket(), ConnectionState.Play);
            registry.AddPaket(0x4F, new Pakets.Client.Play.EntityVelocityPaket(), ConnectionState.Play);
            registry.AddPaket(0x3E, new Pakets.Client.Play.EntityHeadLookPaket(), ConnectionState.Play);
            registry.AddPaket(0x18, new Pakets.Client.Play.PluginMessagePaket(), ConnectionState.Play);
            registry.AddPaket(0x48, new Pakets.Client.Play.SlotSelectionPaket(), ConnectionState.Play);
        }

        // Pakets the server can understand
        public static void RegisterServerPakets(PaketRegistry registry, int ProtocolVersion)
        {
            // Handshake
            registry.AddPaket(0x00, new Pakets.Server.Handshake.HandshakePaket(), ConnectionState.Handshaking);

            // Login
            registry.AddPaket(0x00, new Pakets.Server.Login.LoginStartPaket(), ConnectionState.Login);
            registry.AddPaket(0x01, new Pakets.Server.Login.EncryptionResponsePaket(), ConnectionState.Login);

            // Status
            registry.AddPaket(0x00, new Pakets.Server.Status.StatusRequestPaket(), ConnectionState.Status);
            registry.AddPaket(0x01, new Pakets.Server.Status.PingPaket(), ConnectionState.Status);

            // Play
            registry.AddPaket(0x0F, new Pakets.Server.Play.KeepAlivePaket(), ConnectionState.Play);
            registry.AddPaket(0x00, new Pakets.Server.Play.TeleportConfirmPaket(), ConnectionState.Play);
            registry.AddPaket(0x05, new Pakets.Server.Play.ClientSettingsPaket(), ConnectionState.Play);
            registry.AddPaket(0x04, new Pakets.Server.Play.ClientStatusPaket(), ConnectionState.Play);
            registry.AddPaket(0x03, new Pakets.Server.Play.ChatMessagePaket(), ConnectionState.Play);
            registry.AddPaket(0x11, new Pakets.Server.Play.PlayerPositionPaket(), ConnectionState.Play);
            registry.AddPaket(0x12, new Pakets.Server.Play.PlayerPositionAndRotationPaket(), ConnectionState.Play);
            registry.AddPaket(0x1B, new Pakets.Server.Play.EntityActionPaket(), ConnectionState.Play);
            registry.AddPaket(0x14, new Pakets.Server.Play.PlayerMovementPaket(), ConnectionState.Play);
            registry.AddPaket(0x13, new Pakets.Server.Play.PlayerRotationPaket(), ConnectionState.Play);
            registry.AddPaket(0x0D, new Pakets.Server.Play.InteractEntityPaket(), ConnectionState.Play);
            registry.AddPaket(0x25, new Pakets.Server.Play.HeldItemChangePaket(), ConnectionState.Play);
            registry.AddPaket(0x0A, new Pakets.Server.Play.PluginMessagePaket(), ConnectionState.Play);
            registry.AddPaket(0x1D, new Pakets.Server.Play.PongPaket(), ConnectionState.Play);
        }
    }
}
