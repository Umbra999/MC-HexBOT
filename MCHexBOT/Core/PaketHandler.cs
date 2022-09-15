using MCHexBOT.Features;
using MCHexBOT.Network;
using MCHexBOT.Pakets;
using MCHexBOT.Pakets.Client.Login;
using MCHexBOT.Pakets.Client.Play;
using MCHexBOT.Protocol;
using MCHexBOT.Utils;
using MCHexBOT.Utils.Math;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace MCHexBOT.Core
{
    internal class PaketHandler : IPaketHandler
    {
        public MinecraftConnection Connection { get; set; }
        private APIClient APIClient { get; set; }
        private MinecraftClient MinecraftClient { get; set; }

        private bool IsReady = false;

        public PaketHandler(APIClient Client, MinecraftClient minecraft)
        {
            APIClient = Client;
            MinecraftClient = minecraft;
        }

        public void Handshake(IPaket paket)
        {

        }

        public void Login(IPaket paket)
        {
            if (paket is EncryptionRequestPaket encryptionPaket)
            {
                Logger.LogDebug("Received Encryption");
                HandleEncryptionAuth(encryptionPaket);
            }

            if (paket is SetCompressionPaket compressionPaket)
            {
                Logger.LogDebug("Enabled compression");
            }

            if (paket is LoginSuccessPaket loginSuccessPaket)
            {
                Connection.State = ConnectionState.Play;
                Logger.LogSuccess($"Authenticated as {loginSuccessPaket.Username} [{loginSuccessPaket.Uuid}]");
            }

            if (paket is Pakets.Client.Login.DisconnectPaket disconnectPaket)
            {
                Logger.LogError("Disconnected [JOIN]: " + disconnectPaket.Message);
            }
        }

        public void Play(IPaket paket)
        {
            if (paket is JoinGamePaket joinGamePaket)
            {
                if (!IsReady)
                {
                    Logger.LogSuccess($"{APIClient.CurrentUser.name} joined the game");
                    Logger.LogDebug("Gamemode: " + joinGamePaket.Gamemode);
                    Logger.LogDebug("Sim Dist: " + joinGamePaket.SimulationDistance);
                    Logger.LogDebug("EntityID: " + joinGamePaket.EntityId);

                    MinecraftClient.CurrentPlayer.EntityID = joinGamePaket.EntityId;
                    MinecraftClient.CurrentPlayer.Gamemode = joinGamePaket.Gamemode;

                    MinecraftClient.SendPlayerSetings(true, true, ChatMode.Enabled, byte.MaxValue, MainHandType.Left, false, "en_us", 64);
                }
            }

            if (paket is KeepAlivePaket keepAlivePaket)
            {
                Connection.SendPaket(new Pakets.Server.Play.KeepAlivePaket()
                {
                    Payload = keepAlivePaket.Payload
                });
            }

            if (paket is SpawnPlayerPaket spawnPlayerPaket)
            {
                //Logger.LogDebug($"Spawning player at {spawnPlayerPaket.X} {spawnPlayerPaket.Y} {spawnPlayerPaket.Z} [{spawnPlayerPaket.Yaw} / {spawnPlayerPaket.Pitch}]");
            }

            if (paket is DeathCombatPaket deathPaket)
            {
                Logger.LogDebug($"{deathPaket.KillerEntityID} killed {deathPaket.EntityID}: {deathPaket.Message}");
                MinecraftClient.SendRespawn();
            }

            if (paket is AcknowledgePlayerDiggingPaket acknowledgePlayerDigging)
            {
                var pos = acknowledgePlayerDigging.Location;
                //Logger.LogDebug($"Player is digging at {pos.X} {pos.Y} {pos.Z}");
            }

            if (paket is BlockChangePaket blockChangePaket)
            {
                var pos = blockChangePaket.Location;
                //Logger.LogDebug($"Block changed at {pos.X} {pos.Y} {pos.Z} to {blockChangePaket.BlockId}");
            }

            if (paket is PlayerInfoPaket playerInfoPaket)
            {
                //foreach (var p in playerInfoPaket.Players)
                //{
                //    Logger.LogDebug($"PLAYER: {p.Name} > Gamemode: {p.GameMode} Ping: {p.Ping}");
                //}
            }

            if (paket is UpdateHealthPaket updateHealthPaket)
            {
                MinecraftClient.CurrentPlayer.Food = updateHealthPaket.Food;
                MinecraftClient.CurrentPlayer.Health = updateHealthPaket.Health;
                MinecraftClient.CurrentPlayer.FoodSaturation = updateHealthPaket.Saturation;
                //Logger.LogDebug($"Health update: Health: {updateHealthPaket.Health} Food: {updateHealthPaket.Food} Saturation: {updateHealthPaket.Saturation}");
            }

            if (paket is ChunkDataUpdateLightPaket chunk)
            {
                //Logger.LogDebug($"Received chunk: {chunk.ChunkX} {chunk.ChunkZ} with {chunk.TileEntities.Count} entities");
            }

            if (paket is PlayerPositionAndLookPaket positionPaket)
            {
                if (positionPaket.TeleportID != 0)
                {
                    Connection.SendPaket(new Pakets.Server.Play.TeleportConfirmPaket()
                    {
                        TeleportID = positionPaket.TeleportID
                    });
                }

                if (!IsReady)
                {
                    IsReady = true;
                    Connection.SendPaket(new Pakets.Server.Play.PlayerPositionAndRotationPaket()
                    {
                        X = positionPaket.X,
                        FeetY = positionPaket.Y,
                        Z = positionPaket.Z,
                        OnGround = true,
                        Pitch = positionPaket.Pitch,
                        Yaw = positionPaket.Yaw
                    });
                }
            }

            if (paket is Pakets.Client.Play.DisconnectPaket disconnectPaket)
            {
                Logger.LogError("Disconnected [PLAY]:: " + disconnectPaket.Message);
            }
        }

        public void Status(IPaket paket)
        {

        }

        private async void HandleEncryptionAuth(EncryptionRequestPaket Paket)
        {
            Aes aes = Aes.Create();
            aes.KeySize = 128;
            aes.GenerateKey();

            byte[] hash = SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(Paket.ServerId).Concat(aes.Key).Concat(Paket.PublicKey!).ToArray());
            Array.Reverse(hash);
            BigInteger b = new(hash);
            string Hex;
            if (b < 0) Hex = "-" + (-b).ToString("x").TrimStart('0');
            else Hex = b.ToString("x").TrimStart('0');

            await APIClient.JoinServer(Hex);

            RSA rsa = RsaHelper.DecodePublicKey(Paket.PublicKey!);
            if (rsa == null)
            {
                throw new Exception("Could not decode public key");
            }
            byte[] encrypted = rsa.Encrypt(aes.Key, RSAEncryptionPadding.Pkcs1);
            byte[] encVerTok = rsa.Encrypt(Paket.VerifyToken!, RSAEncryptionPadding.Pkcs1);

            Connection.SendPaket(new Pakets.Server.Login.EncryptionResponsePaket()
            {
                SharedKey = encrypted,
                VerifyToken = encVerTok,
                SharedKeyLenght = encrypted.Length,
                VerifyTokenLenght = encVerTok.Length
            });

            Connection.EnableEncryption(aes.Key);
        }
    }
}
