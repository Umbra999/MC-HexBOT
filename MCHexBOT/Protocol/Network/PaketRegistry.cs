using System.Collections.Generic;

using MCHexBOT.Enums;
using MCHexBOT.Pakets;
using MCHexBOT.Pakets.Client.Login;
using MCHexBOT.Pakets.Client.Play;
using MCHexBOT.Pakets.Client.Status;
using MCHexBOT.Pakets.Server.Handshake;
using MCHexBOT.Pakets.Server.Login;
using MCHexBOT.Pakets.Server.Play;
using MCHexBOT.Pakets.Server.Status;

namespace MCHexBOT.Network
{
    public class PaketRegistry
    {
        public Dictionary<MinecraftState, Dictionary<byte, IPaket>> Pakets = new();

        public PaketRegistry()
        {
            Pakets.Add(MinecraftState.Handshaking, new Dictionary<byte, IPaket>());
            Pakets.Add(MinecraftState.Status, new Dictionary<byte, IPaket>());
            Pakets.Add(MinecraftState.Login, new Dictionary<byte, IPaket>());
            Pakets.Add(MinecraftState.Play, new Dictionary<byte, IPaket>());
        }

        public void AddPaket(byte b, IPaket p, MinecraftState state)
        {
            Pakets[state].Add(b, p);
        }

        public byte GetPaketId(IPaket paket, MinecraftState state)
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
            registry.AddPaket(0x00, new Pakets.Client.Login.DisconnectPaket(), MinecraftState.Login);
            registry.AddPaket(0x01, new Pakets.Client.Login.EncryptionRequestPaket(), MinecraftState.Login);
            registry.AddPaket(0x02, new Pakets.Client.Login.LoginSuccessPaket(), MinecraftState.Login);
            registry.AddPaket(0x03, new Pakets.Client.Login.SetCompressionPaket(), MinecraftState.Login);

            // Status
            registry.AddPaket(0x00, new Pakets.Client.Status.StatusResponsePaket(), MinecraftState.Status);
            registry.AddPaket(0x01, new Pakets.Client.Status.PongPaket(), MinecraftState.Status);

            // Play
            registry.AddPaket(0x00, new Pakets.Client.Play.SpawnEntityPaket(), MinecraftState.Play);
            registry.AddPaket(0x26, new Pakets.Client.Play.JoinGamePaket(), MinecraftState.Play);
            registry.AddPaket(0x21, new Pakets.Client.Play.KeepAlivePaket(), MinecraftState.Play);
            registry.AddPaket(0x04, new Pakets.Client.Play.SpawnPlayerPaket(), MinecraftState.Play);
            registry.AddPaket(0x08, new Pakets.Client.Play.AcknowledgePlayerDiggingPaket(), MinecraftState.Play);
            registry.AddPaket(0x0C, new Pakets.Client.Play.BlockChangePaket(), MinecraftState.Play);
            registry.AddPaket(0x36, new Pakets.Client.Play.PlayerInfoPaket(), MinecraftState.Play);
            registry.AddPaket(0x52, new Pakets.Client.Play.UpdateHealthPaket(), MinecraftState.Play);
            registry.AddPaket(0x22, new Pakets.Client.Play.ChunkDataUpdateLightPaket(), MinecraftState.Play);
            registry.AddPaket(0x38, new Pakets.Client.Play.PlayerPositionAndLookPaket(), MinecraftState.Play);
            registry.AddPaket(0x0F, new Pakets.Client.Play.ChatMessagePaket(), MinecraftState.Play);
            registry.AddPaket(0x29, new Pakets.Client.Play.EntityPositionPaket(), MinecraftState.Play);
            registry.AddPaket(0x2A, new Pakets.Client.Play.EntityPositionAndRotationPaket(), MinecraftState.Play);
            registry.AddPaket(0x49, new Pakets.Client.Play.UpdateViewPositionPaket(), MinecraftState.Play);
            registry.AddPaket(0x1D, new Pakets.Client.Play.UnloadChunkPaket(), MinecraftState.Play);
            registry.AddPaket(0x4B, new Pakets.Client.Play.SpawnPositionPaket(), MinecraftState.Play);
            registry.AddPaket(0x1A, new Pakets.Client.Play.DisconnectPaket(), MinecraftState.Play);
        }

        // Pakets the server can understand
        public static void RegisterServerPakets(PaketRegistry registry, int ProtocolVersion)
        {
            // Handshake
            registry.AddPaket(0x00, new Pakets.Server.Handshake.HandshakePaket(), MinecraftState.Handshaking);

            // Login
            registry.AddPaket(0x00, new Pakets.Server.Login.LoginStartPaket(), MinecraftState.Login);
            registry.AddPaket(0x01, new Pakets.Server.Login.EncryptionResponsePaket(), MinecraftState.Login);

            // Status
            registry.AddPaket(0x00, new Pakets.Server.Status.StatusRequestPaket(), MinecraftState.Status);
            registry.AddPaket(0x01, new Pakets.Server.Status.PingPaket(), MinecraftState.Status);

            // Play
            registry.AddPaket(0x0F, new Pakets.Server.Play.KeepAlivePaket(), MinecraftState.Play);
            registry.AddPaket(0x00, new Pakets.Server.Play.TeleportConfirmPaket(), MinecraftState.Play);
            registry.AddPaket(0x05, new Pakets.Server.Play.ClientSettingsPaket(), MinecraftState.Play);
            registry.AddPaket(0x04, new Pakets.Server.Play.ClientStatusPaket(), MinecraftState.Play);
            registry.AddPaket(0x03, new Pakets.Server.Play.ChatMessagePaket(), MinecraftState.Play);
            registry.AddPaket(0x11, new Pakets.Server.Play.PlayerPositionPaket(), MinecraftState.Play);
            registry.AddPaket(0x12, new Pakets.Server.Play.PlayerPositionAndRotationPaket(), MinecraftState.Play);
            registry.AddPaket(0x1B, new Pakets.Server.Play.EntityActionPaket(), MinecraftState.Play);
            registry.AddPaket(0x14, new Pakets.Server.Play.PlayerMovementPaket(), MinecraftState.Play);
            registry.AddPaket(0x13, new Pakets.Server.Play.PlayerRotationPaket(), MinecraftState.Play);
        }
    }
}
