using MCHexBOT.Features;
using MCHexBOT.Network;
using MCHexBOT.Packets;
using MCHexBOT.Packets.Client.Login;
using MCHexBOT.Packets.Client.Play;
using MCHexBOT.Protocol;
using MCHexBOT.Utils;
using MCHexBOT.Utils.Math;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace MCHexBOT.Core
{
    internal class PacketHandler : IPacketHandler
    {
        public MinecraftConnection Connection { get; set; }
        private MinecraftClient MinecraftClient { get; set; }

        private bool IsReady = false;

        public PacketHandler(MinecraftClient minecraft)
        {
            MinecraftClient = minecraft;
        }

        public void Handshake(IPacket Packet)
        {

        }

        public void Login(IPacket Packet)
        {
            if (Packet is EncryptionRequestPacket encryptionPacket)
            {
                Logger.LogDebug("Received Encryption");
                HandleEncryptionAuth(encryptionPacket);
            }

            if (Packet is SetCompressionPacket compressionPacket)
            {
                Logger.LogDebug("Enabled compression");
            }

            if (Packet is LoginSuccessPacket loginSuccessPacket)
            {
                Connection.State = ConnectionState.Play;
                Logger.LogSuccess($"Authenticated as {loginSuccessPacket.Username} [{loginSuccessPacket.Uuid}]");
            }

            if (Packet is Packets.Client.Login.DisconnectPacket disconnectPacket)
            {
                Logger.LogError($"{MinecraftClient.APIClient.CurrentUser.name} Disconnected: {disconnectPacket.Message}");
            }
        }

        public void Play(IPacket Packet)
        {
            if (Packet is JoinGamePacket joinGamePacket)
            {
                IsReady = false;
                Logger.LogSuccess($"{MinecraftClient.APIClient.CurrentUser.name} joined the Server");
                Logger.LogDebug("Gamemode: " + joinGamePacket.Gamemode);
                Logger.LogDebug("Sim Dist: " + joinGamePacket.SimulationDistance);
                Logger.LogDebug("EntityID: " + joinGamePacket.EntityId);

                MinecraftClient.Players.Clear();

                MinecraftClient.GetLocalPlayer().EntityID = joinGamePacket.EntityId;

                MinecraftClient.SendPlayerSetings(true, true, ChatMode.Enabled, byte.MaxValue, MainHandType.Left, false, "en_us", 64);
            }

            if (Packet is KeepAlivePacket keepAlivePacket)
            {
                Connection.SendPacket(new Packets.Server.Play.KeepAlivePacket()
                {
                    Payload = keepAlivePacket.Payload
                });
            }

            if (Packet is PingPacket pingPacket)
            {
                Connection.SendPacket(new Packets.Server.Play.PongPacket()
                {
                    ID = pingPacket.ID
                });
            }

            if (Packet is DeathCombatPacket deathPacket)
            {
                Logger.LogDebug($"{deathPacket.KillerEntityID} killed {deathPacket.EntityID}: {deathPacket.Message}");
                MinecraftClient.SendRespawn();
            }

            if (Packet is PlayerInfoPacket playerInfoPacket)
            {
                switch (playerInfoPacket.Action)
                {
                    case 0:
                        foreach (PlayerInfo player in playerInfoPacket.Players)
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
                        foreach (PlayerInfo player in playerInfoPacket.Players)
                        {
                            foreach (Player Cache in MinecraftClient.Players.Where(x => x.PlayerInfo?.UUID.ToString() == player.UUID.ToString()))
                            {
                                Cache.PlayerInfo.GameMode = player.GameMode;
                            }
                        }
                        break;

                    case 2:
                        foreach (PlayerInfo player in playerInfoPacket.Players)
                        {
                            foreach (Player Cache in MinecraftClient.Players.Where(x => x.PlayerInfo?.UUID.ToString() == player.UUID.ToString()))
                            {
                                Cache.PlayerInfo.Ping = player.Ping;
                            }
                        }
                        break;

                    case 3:
                        foreach (PlayerInfo player in playerInfoPacket.Players)
                        {
                            foreach (Player Cache in MinecraftClient.Players.Where(x => x.PlayerInfo?.UUID.ToString() == player.UUID.ToString()))
                            {
                                Cache.PlayerInfo.HasDisplayName = player.HasDisplayName;
                                Cache.PlayerInfo.DisplayName = player.DisplayName;
                            }
                        }
                        break;

                    case 4:
                        foreach (PlayerInfo player in playerInfoPacket.Players)
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

            if (Packet is UpdateHealthPacket updateHealthPacket)
            {
                MinecraftClient.GetLocalPlayer().Food = updateHealthPacket.Food;
                MinecraftClient.GetLocalPlayer().Health = updateHealthPacket.Health;
                MinecraftClient.GetLocalPlayer().Saturation = updateHealthPacket.Saturation;
                Logger.LogDebug($"Health update: Health: {updateHealthPacket.Health} Food: {updateHealthPacket.Food} Saturation: {updateHealthPacket.Saturation}");
            }

            if (Packet is ChatMessagePacket messagePacket)
            {
                //ChatCommands.HandleChat(MinecraftClient, messagePacket.JsonData);
            }

            if (Packet is SlotSelectionPacket slotPacket)
            {
                Logger.LogDebug($"Server changed Item slot to {slotPacket.Slot}");
                //MinecraftClient.SendHeldItemSlotSwitch(slotPacket.Slot);
            }

            if (Packet is PluginMessagePacket pluginPacket)
            {
                switch (pluginPacket.Channel)
                {
                    case "minecraft:brand":
                        Connection.SendPacket(new Packets.Server.Play.PluginMessagePacket()
                        {
                            Channel = pluginPacket.Channel,
                            Data = Encoding.UTF8.GetBytes("vanilla")
                        });
                        break;

                    case "minecraft:register":
                        Connection.SendPacket(new Packets.Server.Play.PluginMessagePacket()
                        {
                            Channel = pluginPacket.Channel,
                            Data = pluginPacket.Data
                        });
                        break;
                }
            }

            if (Packet is PlayerPositionAndLookPacket positionPacket)
            {
                Connection.SendPacket(new Packets.Server.Play.TeleportConfirmPacket()
                {
                    TeleportID = positionPacket.TeleportID
                });

                Vector3 NewPos = MinecraftClient.GetLocalPlayer().Position;
                Vector2 NewRot = MinecraftClient.GetLocalPlayer().Rotation;

                if ((positionPacket.Flags & 0x01) == 0x01) NewPos.X += (float)positionPacket.X;
                else NewPos.X = (float)positionPacket.X;

                if ((positionPacket.Flags & 0x02) == 0x02) NewPos.Y += (float)positionPacket.Y!;
                else NewPos.Y = (float)positionPacket.Y!;

                if ((positionPacket.Flags & 0x04) == 0x04) NewPos.Z += (float)positionPacket.Z!;
                else NewPos.Z = (float)positionPacket.Z!;

                if ((positionPacket.Flags & 0x08) == 0x08) NewRot.Y += positionPacket.Pitch!;
                else NewRot.Y = positionPacket.Pitch!;

                if ((positionPacket.Flags & 0x10) == 0x10) NewPos.X += positionPacket.Yaw!;
                else NewRot.X = positionPacket.Yaw!;

                MinecraftClient.GetLocalPlayer().Position = NewPos;
                MinecraftClient.GetLocalPlayer().Rotation = NewRot;

                if (!IsReady)
                {
                    IsReady = true;
                    Connection.SendPacket(new Packets.Server.Play.PlayerPositionAndRotationPacket()
                    {
                        X = positionPacket.X,
                        Y = positionPacket.Y,
                        Z = positionPacket.Z,
                        OnGround = MinecraftClient.GetLocalPlayer().IsOnGround,
                        Pitch = positionPacket.Pitch,
                        Yaw = positionPacket.Yaw
                    });
                }
            }

            if (Packet is Packets.Client.Play.DisconnectPacket disconnectPacket)
            {
                Logger.LogError($"{MinecraftClient.APIClient.CurrentUser.name} Disconnected: {disconnectPacket.Message}");
            }

            if (Packet is SpawnPlayerPacket spawnPlayerPacket)
            {
                foreach (Player player in MinecraftClient.Players)
                {
                    if (player.PlayerInfo.UUID.ToString() == spawnPlayerPacket.UUID.ToString())
                    {
                        player.Position = new Vector3((float)spawnPlayerPacket.X, (float)spawnPlayerPacket.Y, (float)spawnPlayerPacket.Z);
                        player.Rotation = new Vector2(spawnPlayerPacket.Yaw, spawnPlayerPacket.Pitch);
                        player.EntityID = spawnPlayerPacket.EntityId;
                    }
                }
                //Logger.LogDebug($"Spawning player at {spawnPlayerPacket.X} {spawnPlayerPacket.Y} {spawnPlayerPacket.Z} [{spawnPlayerPacket.Yaw} / {spawnPlayerPacket.Pitch}]");
            }

            // Entity 
            if (Packet is SpawnLivingEntity entityAliveSpawnPacket)
            {
                if (entityAliveSpawnPacket.Type == 116)
                {
                    foreach (Player player in MinecraftClient.Players.Where(x => x.EntityID == entityAliveSpawnPacket.EntityId || x.PlayerInfo.UUID.ToString() == entityAliveSpawnPacket.EntityUUID.ToString()))
                    {
                        player.EntityID = entityAliveSpawnPacket.EntityId;
                        player.Position = new Vector3((float)entityAliveSpawnPacket.XPosition, (float)entityAliveSpawnPacket.YPosition, (float)entityAliveSpawnPacket.ZPosition);
                        player.Rotation = new Vector2(entityAliveSpawnPacket.Yaw, entityAliveSpawnPacket.Pitch);
                        player.Velocity = new Vector3(entityAliveSpawnPacket.XVelocity, entityAliveSpawnPacket.YVelocity, entityAliveSpawnPacket.ZVelocity);
                    }
                }
            }

            if (Packet is EntityPositionAndRotationPacket entityPosAndRotPacket)
            {
                foreach (Player player in MinecraftClient.Players.Where(x => x.EntityID == entityPosAndRotPacket.EntityId))
                {
                    player.Position += new Vector3(entityPosAndRotPacket.DeltaX / 4096, entityPosAndRotPacket.DeltaY / 4096, entityPosAndRotPacket.DeltaZ / 4096);
                    player.Rotation = new Vector2(entityPosAndRotPacket.Yaw, entityPosAndRotPacket.Pitch);
                    player.IsOnGround = entityPosAndRotPacket.OnGround;
                }
            }

            if (Packet is EntityPositionPacket entityPosPacket)
            {
  
                foreach (Player player in MinecraftClient.Players.Where(x => x.EntityID == entityPosPacket.EntityId))
                {
                    player.Position += new Vector3(entityPosPacket.DeltaX / 4096, entityPosPacket.DeltaY / 4096, entityPosPacket.DeltaZ / 4096);
                    player.IsOnGround = entityPosPacket.OnGround;
                }
            }

            if (Packet is EntityRotationPacket entityRotPacket)
            {
                foreach (Player player in MinecraftClient.Players.Where(x => x.EntityID == entityRotPacket.EntityId))
                {
                    player.Rotation = new Vector2(entityRotPacket.Yaw, entityRotPacket.Pitch);
                    player.IsOnGround = entityRotPacket.OnGround;
                }
            }

            if (Packet is EntityTeleportPacket entityTeleportPacket)
            {
                foreach (Player player in MinecraftClient.Players.Where(x => x.EntityID == entityTeleportPacket.EntityId))
                {
                    player.Position = new Vector3((float)entityTeleportPacket.X, (float)entityTeleportPacket.Y, (float)entityTeleportPacket.Z);
                    player.Rotation = new Vector2(entityTeleportPacket.Yaw, entityTeleportPacket.Pitch);
                    player.IsOnGround = entityTeleportPacket.OnGround;
                }
            }

            if (Packet is EntityVelocityPacket entityVelocityPacket)
            {
                foreach (Player player in MinecraftClient.Players.Where(x => x.EntityID == entityVelocityPacket.EntityId))
                {
                    player.Velocity = new Vector3(entityVelocityPacket.XVelocity, entityVelocityPacket.YVelocity, entityVelocityPacket.ZVelocity);
                }
            }

            if (Packet is EntityHeadLookPacket entityLookPacket)
            {
                foreach (Player player in MinecraftClient.Players.Where(x => x.EntityID == entityLookPacket.EntityId))
                {
                    player.Rotation = new Vector2(entityLookPacket.HeadYaw, player.Rotation.Y);
                }
            }
        }

        public void Status(IPacket Packet)
        {

        }

        private async void HandleEncryptionAuth(EncryptionRequestPacket Packet)
        {
            Aes aes = Aes.Create();
            aes.KeySize = 128;
            aes.GenerateKey();

            byte[] hash = SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(Packet.ServerId).Concat(aes.Key).Concat(Packet.PublicKey!).ToArray());
            Array.Reverse(hash);
            BigInteger b = new(hash);
            string Hex;
            if (b < 0) Hex = "-" + (-b).ToString("x").TrimStart('0');
            else Hex = b.ToString("x").TrimStart('0');

            await MinecraftClient.APIClient.JoinServer(Hex);

            RSA rsa = RsaHelper.DecodePublicKey(Packet.PublicKey!);
            if (rsa == null)
            {
                throw new Exception("Could not decode public key");
            }
            byte[] encrypted = rsa.Encrypt(aes.Key, RSAEncryptionPadding.Pkcs1);
            byte[] encVerTok = rsa.Encrypt(Packet.VerifyToken!, RSAEncryptionPadding.Pkcs1);

            Connection.SendPacket(new Packets.Server.Login.EncryptionResponsePacket()
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
