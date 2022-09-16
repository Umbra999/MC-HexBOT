using MCHexBOT.Features;
using MCHexBOT.Network;
using MCHexBOT.Pakets;
using MCHexBOT.Pakets.Client.Login;
using MCHexBOT.Pakets.Client.Play;
using MCHexBOT.Protocol;
using MCHexBOT.Utils;
using MCHexBOT.Utils.Math;
using Newtonsoft.Json;
using Org.BouncyCastle.Bcpg;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace MCHexBOT.Core
{
    internal class PaketHandler : IPaketHandler
    {
        public MinecraftConnection Connection { get; set; }
        private MinecraftClient MinecraftClient { get; set; }

        private bool IsReady = false;

        public PaketHandler(APIClient Client, MinecraftClient minecraft)
        {
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
                Logger.LogError($"{MinecraftClient.APIClient.CurrentUser.name} Disconnected: {disconnectPaket.Message}");
            }
        }

        public void Play(IPaket paket)
        {
            if (paket is JoinGamePaket joinGamePaket)
            {
                if (!IsReady)
                {
                    Logger.LogSuccess($"{MinecraftClient.APIClient.CurrentUser.name} joined the game");
                    Logger.LogDebug("Gamemode: " + joinGamePaket.Gamemode);
                    Logger.LogDebug("Sim Dist: " + joinGamePaket.SimulationDistance);
                    Logger.LogDebug("EntityID: " + joinGamePaket.EntityId);

                    MinecraftClient.LocalPlayer.EntityID = joinGamePaket.EntityId;

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
                foreach (Player player in MinecraftClient.Players)
                {
                    if (player.PlayerInfo.UUID.ToString() == spawnPlayerPaket.UUID.ToString())
                    {
                        player.Position = new Vector3((float)spawnPlayerPaket.X, (float)spawnPlayerPaket.Y, (float)spawnPlayerPaket.Z);
                        player.Rotation = new Vector2(spawnPlayerPaket.Yaw, spawnPlayerPaket.Pitch);
                        player.EntityID = spawnPlayerPaket.EntityId;
                    }
                }
                //Logger.LogDebug($"Spawning player at {spawnPlayerPaket.X} {spawnPlayerPaket.Y} {spawnPlayerPaket.Z} [{spawnPlayerPaket.Yaw} / {spawnPlayerPaket.Pitch}]");
            }

            if (paket is DeathCombatPaket deathPaket)
            {
                Logger.LogDebug($"{deathPaket.KillerEntityID} killed {deathPaket.EntityID}: {deathPaket.Message}");
                MinecraftClient.SendRespawn();
            }

            if (paket is PlayerInfoPaket playerInfoPaket)
            {
                switch (playerInfoPaket.Action)
                {
                    case 0:
                        foreach (PlayerInfo player in playerInfoPaket.Players)
                        {
                            if (MinecraftClient.Players.Where(x => x.PlayerInfo.UUID.ToString() == player.UUID.ToString()).ToArray().Length < 1)
                            {
                                MinecraftClient.Players.Add(new Player()
                                {
                                    PlayerInfo = player
                                });
                            }
                        }
                        break;

                    case 1:
                        foreach (PlayerInfo player in playerInfoPaket.Players)
                        {
                            foreach (Player Cache in MinecraftClient.Players.Where(x => x.PlayerInfo.UUID.ToString() == player.UUID.ToString()))
                            {
                                Cache.PlayerInfo.GameMode = player.GameMode;
                            }
                        }
                        break;

                    case 2:
                        foreach (PlayerInfo player in playerInfoPaket.Players)
                        {
                            foreach (Player Cache in MinecraftClient.Players.Where(x => x.PlayerInfo.UUID.ToString() == player.UUID.ToString()))
                            {
                                Cache.PlayerInfo.Ping = player.Ping;
                            }
                        }
                        break;

                    case 3:
                        foreach (PlayerInfo player in playerInfoPaket.Players)
                        {
                            foreach (Player Cache in MinecraftClient.Players.Where(x => x.PlayerInfo.UUID.ToString() == player.UUID.ToString()))
                            {
                                Cache.PlayerInfo.HasDisplayName = player.HasDisplayName;
                                Cache.PlayerInfo.DisplayName = player.DisplayName;
                            }
                        }
                        break;

                    case 4:
                        foreach (PlayerInfo player in playerInfoPaket.Players)
                        {
                            var List = MinecraftClient.Players.Where(x => x.PlayerInfo.UUID.ToString() == player.UUID.ToString()).ToList();
                            foreach (Player Cache in List)
                            {
                                MinecraftClient.Players.Remove(Cache);
                            }
                        }
                        break;
                }
            }

            if (paket is UpdateHealthPaket updateHealthPaket)
            {
                MinecraftClient.LocalPlayer.Food = updateHealthPaket.Food;
                MinecraftClient.LocalPlayer.Health = updateHealthPaket.Health;
                MinecraftClient.LocalPlayer.Saturation = updateHealthPaket.Saturation;
                //Logger.LogDebug($"Health update: Health: {updateHealthPaket.Health} Food: {updateHealthPaket.Food} Saturation: {updateHealthPaket.Saturation}");
            }

            if (paket is ChatMessagePaket messagePaket)
            {
                //ChatCommands.HandleChat(MinecraftClient, messagePaket.JsonData);
            }

            if (paket is PlayerPositionAndLookPaket positionPaket)
            {
                //Logger.LogImportant($"X: {positionPaket.X}");
                //Logger.LogImportant($"Y: {positionPaket.Y}");
                //Logger.LogImportant($"Z: {positionPaket.Z}");
                //Logger.LogImportant($"PITCH: {positionPaket.Pitch}");
                //Logger.LogImportant($"YAW: {positionPaket.Yaw}");

                Vector3 NewPos = MinecraftClient.LocalPlayer.Position;
                Vector2 NewRot = MinecraftClient.LocalPlayer.Rotation;

                if ((positionPaket.Flags & 0x01) == 0x01) NewPos.X += (float)positionPaket.X;
                else NewPos.X = (float)positionPaket.X;

                if ((positionPaket.Flags & 0x02) == 0x02) NewPos.Y += (float)positionPaket.Y!;
                else NewPos.Y = (float)positionPaket.Y!;

                if ((positionPaket.Flags & 0x04) == 0x04) NewPos.Z += (float)positionPaket.Z!;
                else NewPos.Z = (float)positionPaket.Z!;

                if ((positionPaket.Flags & 0x08) == 0x08) NewRot.Y += positionPaket.Pitch!;
                else NewRot.Y = positionPaket.Pitch!;

                if ((positionPaket.Flags & 0x10) == 0x10) NewPos.X += positionPaket.Yaw!;
                else NewRot.X = positionPaket.Yaw!;

                MinecraftClient.LocalPlayer.Position = NewPos;
                MinecraftClient.LocalPlayer.Rotation = NewRot;

                Connection.SendPaket(new Pakets.Server.Play.TeleportConfirmPaket()
                {
                    TeleportID = positionPaket.TeleportID
                });

                if (!IsReady)
                {
                    IsReady = true;
                    Connection.SendPaket(new Pakets.Server.Play.PlayerPositionAndRotationPaket()
                    {
                        X = positionPaket.X,
                        Y = positionPaket.Y,
                        Z = positionPaket.Z,
                        OnGround = true,
                        Pitch = positionPaket.Pitch,
                        Yaw = positionPaket.Yaw
                    });
                }
            }

            if (paket is Pakets.Client.Play.DisconnectPaket disconnectPaket)
            {
                Logger.LogError($"{MinecraftClient.APIClient.CurrentUser.name} Disconnected: " + JsonConvert.SerializeObject(disconnectPaket.Message));
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

            await MinecraftClient.APIClient.JoinServer(Hex);

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
