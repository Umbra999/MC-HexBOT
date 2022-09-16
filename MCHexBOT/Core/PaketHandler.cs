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
                IsReady = false;
                Logger.LogSuccess($"{MinecraftClient.APIClient.CurrentUser.name} joined the Server");
                Logger.LogDebug("Gamemode: " + joinGamePaket.Gamemode);
                Logger.LogDebug("Sim Dist: " + joinGamePaket.SimulationDistance);
                Logger.LogDebug("EntityID: " + joinGamePaket.EntityId);

                MinecraftClient.Players.Clear();

                MinecraftClient.GetLocalPlayer().EntityID = joinGamePaket.EntityId;

                MinecraftClient.SendPlayerSetings(true, true, ChatMode.Enabled, byte.MaxValue, MainHandType.Left, false, "en_us", 64);
            }

            if (paket is KeepAlivePaket keepAlivePaket)
            {
                Connection.SendPaket(new Pakets.Server.Play.KeepAlivePaket()
                {
                    Payload = keepAlivePaket.Payload
                });
            }

            if (paket is PingPaket pingPaket)
            {
                Connection.SendPaket(new Pakets.Server.Play.PongPaket()
                {
                    ID = pingPaket.ID
                });
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
                            if (player.Name == MinecraftClient.APIClient.CurrentUser.name)
                            {
                                foreach (Player Local in MinecraftClient.Players.Where(x => x.IsLocal))
                                {
                                    Local.PlayerInfo = player;
                                    Local.IsOnGround = true;
                                    Local.Health = 20;
                                    Local.Food = 20;
                                    Local.Saturation = 5;
                                    Local.Position = new Vector3(0, 0, 0);
                                    Local.Rotation = new Vector2(0, 0);
                                }
                            }

                            else if (MinecraftClient.Players.Where(x => x.PlayerInfo?.UUID.ToString() == player.UUID.ToString()).ToArray().Length < 1)
                            {
                                MinecraftClient.Players.Add(new Player()
                                {
                                    PlayerInfo = player,
                                    IsOnGround = true,
                                    Health = 20,
                                    Food = 20,
                                    Saturation = 5,
                                    Position = new Vector3(0, 0, 0),
                                    Rotation = new Vector2(0, 0)
                                });
                                //Logger.Log($"[ + ] {player.Name}");
                            }
                        }
                        break;

                    case 1:
                        foreach (PlayerInfo player in playerInfoPaket.Players)
                        {
                            foreach (Player Cache in MinecraftClient.Players.Where(x => x.PlayerInfo?.UUID.ToString() == player.UUID.ToString()))
                            {
                                Cache.PlayerInfo.GameMode = player.GameMode;
                            }
                        }
                        break;

                    case 2:
                        foreach (PlayerInfo player in playerInfoPaket.Players)
                        {
                            foreach (Player Cache in MinecraftClient.Players.Where(x => x.PlayerInfo?.UUID.ToString() == player.UUID.ToString()))
                            {
                                Cache.PlayerInfo.Ping = player.Ping;
                            }
                        }
                        break;

                    case 3:
                        foreach (PlayerInfo player in playerInfoPaket.Players)
                        {
                            foreach (Player Cache in MinecraftClient.Players.Where(x => x.PlayerInfo?.UUID.ToString() == player.UUID.ToString()))
                            {
                                Cache.PlayerInfo.HasDisplayName = player.HasDisplayName;
                                Cache.PlayerInfo.DisplayName = player.DisplayName;
                            }
                        }
                        break;

                    case 4:
                        foreach (PlayerInfo player in playerInfoPaket.Players)
                        {
                            var List = MinecraftClient.Players.Where(x => x.PlayerInfo?.UUID.ToString() == player.UUID.ToString()).ToList();
                            foreach (Player Cache in List)
                            {
                                //Logger.Log($"[ - ] {Cache.PlayerInfo.Name}");
                                MinecraftClient.Players.Remove(Cache);
                            }
                        }
                        break;
                }
            }

            if (paket is UpdateHealthPaket updateHealthPaket)
            {
                MinecraftClient.GetLocalPlayer().Food = updateHealthPaket.Food;
                MinecraftClient.GetLocalPlayer().Health = updateHealthPaket.Health;
                MinecraftClient.GetLocalPlayer().Saturation = updateHealthPaket.Saturation;
                Logger.LogDebug($"Health update: Health: {updateHealthPaket.Health} Food: {updateHealthPaket.Food} Saturation: {updateHealthPaket.Saturation}");
            }

            if (paket is ChatMessagePaket messagePaket)
            {
                //ChatCommands.HandleChat(MinecraftClient, messagePaket.JsonData);
            }

            if (paket is SlotSelectionPaket slotPaket)
            {
                Logger.LogDebug($"Server changed Item slot to {slotPaket.Slot}");
                MinecraftClient.SendHeldItemSlotSwitch(slotPaket.Slot);
            }

            if (paket is PluginMessagePaket pluginPaket)
            {
                switch (pluginPaket.Channel)
                {
                    case "minecraft:brand":
                        Connection.SendPaket(new Pakets.Server.Play.PluginMessagePaket()
                        {
                            Channel = pluginPaket.Channel,
                            Data = Encoding.UTF8.GetBytes("vanilla")
                        });
                        break;

                    case "minecraft:register":
                        Connection.SendPaket(new Pakets.Server.Play.PluginMessagePaket()
                        {
                            Channel = pluginPaket.Channel,
                            Data = pluginPaket.Data
                        });
                        break;
                }
            }

            if (paket is PlayerPositionAndLookPaket positionPaket)
            {
                Connection.SendPaket(new Pakets.Server.Play.TeleportConfirmPaket()
                {
                    TeleportID = positionPaket.TeleportID
                });

                Vector3 NewPos = MinecraftClient.GetLocalPlayer().Position;
                Vector2 NewRot = MinecraftClient.GetLocalPlayer().Rotation;

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

                MinecraftClient.GetLocalPlayer().Position = NewPos;
                MinecraftClient.GetLocalPlayer().Rotation = NewRot;;

                if (!IsReady)
                {
                    IsReady = true;

                    Connection.SendPaket(new Pakets.Server.Play.PlayerPositionAndRotationPaket()
                    {
                        X = positionPaket.X,
                        Y = positionPaket.Y,
                        Z = positionPaket.Z,
                        OnGround = MinecraftClient.GetLocalPlayer().IsOnGround,
                        Pitch = positionPaket.Pitch,
                        Yaw = positionPaket.Yaw
                    });
                }
            }

            if (paket is Pakets.Client.Play.DisconnectPaket disconnectPaket)
            {
                Logger.LogError($"{MinecraftClient.APIClient.CurrentUser.name} Disconnected: {disconnectPaket.Message}");
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

            // Entity 
            if (paket is SpawnLivingEntity entityAliveSpawnPaket)
            {
                if (entityAliveSpawnPaket.Type == 116)
                {
                    foreach (Player player in MinecraftClient.Players.Where(x => x.EntityID == entityAliveSpawnPaket.EntityId || x.PlayerInfo.UUID.ToString() == entityAliveSpawnPaket.EntityUUID.ToString()))
                    {
                        player.EntityID = entityAliveSpawnPaket.EntityId;
                        player.Position = new Vector3((float)entityAliveSpawnPaket.XPosition, (float)entityAliveSpawnPaket.YPosition, (float)entityAliveSpawnPaket.ZPosition);
                        player.Rotation = new Vector2(entityAliveSpawnPaket.Yaw, entityAliveSpawnPaket.Pitch);
                        player.Velocity = new Vector3(entityAliveSpawnPaket.XVelocity, entityAliveSpawnPaket.YVelocity, entityAliveSpawnPaket.ZVelocity);
                    }
                }
            }

            if (paket is EntityPositionAndRotationPaket entityPosAndRotPaket)
            {
                foreach (Player player in MinecraftClient.Players.Where(x => x.EntityID == entityPosAndRotPaket.EntityId))
                {
                    player.Position += new Vector3(entityPosAndRotPaket.DeltaX / 4096, entityPosAndRotPaket.DeltaY / 4096, entityPosAndRotPaket.DeltaZ / 4096);
                    player.Rotation = new Vector2(entityPosAndRotPaket.Yaw, entityPosAndRotPaket.Pitch);
                    player.IsOnGround = entityPosAndRotPaket.OnGround;
                }
            }

            if (paket is EntityPositionPaket entityPosPaket)
            {
  
                foreach (Player player in MinecraftClient.Players.Where(x => x.EntityID == entityPosPaket.EntityId))
                {
                    player.Position += new Vector3(entityPosPaket.DeltaX / 4096, entityPosPaket.DeltaY / 4096, entityPosPaket.DeltaZ / 4096);
                    player.IsOnGround = entityPosPaket.OnGround;
                }
            }

            if (paket is EntityRotationPaket entityRotPaket)
            {
                foreach (Player player in MinecraftClient.Players.Where(x => x.EntityID == entityRotPaket.EntityId))
                {
                    player.Rotation = new Vector2(entityRotPaket.Yaw, entityRotPaket.Pitch);
                    player.IsOnGround = entityRotPaket.OnGround;
                }
            }

            if (paket is EntityTeleportPaket entityTeleportPaket)
            {
                foreach (Player player in MinecraftClient.Players.Where(x => x.EntityID == entityTeleportPaket.EntityId))
                {
                    player.Position = new Vector3((float)entityTeleportPaket.X, (float)entityTeleportPaket.Y, (float)entityTeleportPaket.Z);
                    player.Rotation = new Vector2(entityTeleportPaket.Yaw, entityTeleportPaket.Pitch);
                    player.IsOnGround = entityTeleportPaket.OnGround;
                }
            }

            if (paket is EntityVelocityPaket entityVelocityPaket)
            {
                foreach (Player player in MinecraftClient.Players.Where(x => x.EntityID == entityVelocityPaket.EntityId))
                {
                    player.Velocity = new Vector3(entityVelocityPaket.XVelocity, entityVelocityPaket.YVelocity, entityVelocityPaket.ZVelocity);
                }
            }

            if (paket is EntityHeadLookPaket entityLookPaket)
            {
                foreach (Player player in MinecraftClient.Players.Where(x => x.EntityID == entityLookPaket.EntityId))
                {
                    player.Rotation = new Vector2(entityLookPaket.HeadYaw, player.Rotation.Y);
                }
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
