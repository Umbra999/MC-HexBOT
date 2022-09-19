using MCHexBOT.HexServer;
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
using static MCHexBOT.Features.Movement;

namespace MCHexBOT.Core
{
    internal class MinecraftHandler : IPacketHandler
    {
        public MinecraftConnection Connection { get; set; }
        private MinecraftClient MinecraftClient { get; set; }

        public MinecraftHandler(MinecraftClient minecraft)
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
                Logger.LogDebug("Handling Encryption Auth");
                HandleEncryptionAuth(encryptionPacket);
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
                Logger.LogSuccess($"{MinecraftClient.APIClient.CurrentUser.name} joined the Server");
                Logger.LogDebug("Gamemode: " + joinGamePacket.Gamemode);
                Logger.LogDebug("Sim Dist: " + joinGamePacket.SimulationDistance);
                Logger.LogDebug("EntityID: " + joinGamePacket.EntityId);

                MinecraftClient.Players.Clear();

                MinecraftClient.GetLocalPlayer().EntityID = joinGamePacket.EntityId;

                MinecraftClient.SendPlayerSetings(false, true, ChatMode.Enabled, byte.MaxValue, MainHandType.Left, false, "en_us", 64);
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
                                Player[] Players = MinecraftClient.Players.Where(x => x.IsLocal).ToArray();
                                if (Players.Length > 0)
                                {
                                    Players.First().PlayerInfo = player;
                                    Players.First().IsOnGround = true;
                                    Players.First().Health = 20;
                                    Players.First().Food = 20;
                                    Players.First().Saturation = 5;
                                    Players.First().Position = new Vector3(0, 0, 0);
                                    Players.First().Rotation = new Vector2(0, 0);
                                    Players.First().Velocity = new Vector3(0, 0, 0);
                                    Players.First().HeldItemSlot = 0;
                                    Players.First().IsSneaking = false;
                                    Players.First().IsSprinting = false;
                                }
                            }

                            else if (MinecraftClient.Players.Where(x => x.PlayerInfo?.UUID.ToString() == player.UUID.ToString()).ToArray().Length == 0)
                            {
                                MinecraftClient.Players.Add(new Player()
                                {
                                    PlayerInfo = player,
                                    EntityID = 0,
                                    IsLocal = false,
                                    IsOnGround = true,
                                    Health = 20,
                                    Food = 20,
                                    Saturation = 5,
                                    Position = new Vector3(0, 0, 0),
                                    Rotation = new Vector2(0, 0),
                                    Velocity = new Vector3(0, 0, 0),
                                    HeldItemSlot = 0,
                                    IsSneaking = false,
                                    IsSprinting = false,
                                });
                                //Logger.Log($"[ + ] {player.Name}");

                                ServerHandler.CheckOverseePlayer(player.UUID.ToString(), player.Name, MinecraftClient.ServerAddress);
                            }
                        }
                        break;

                    case 1:
                        foreach (PlayerInfo player in playerInfoPacket.Players)
                        {
                            Player[] Players = MinecraftClient.Players.Where(x => x.PlayerInfo?.UUID.ToString() == player.UUID.ToString()).ToArray();
                            if (Players.Length > 0)
                            {
                                Players.First().PlayerInfo.GameMode = player.GameMode;
                            }
                        }
                        break;

                    case 2:
                        foreach (PlayerInfo player in playerInfoPacket.Players)
                        {
                            Player[] Players = MinecraftClient.Players.Where(x => x.PlayerInfo?.UUID.ToString() == player.UUID.ToString()).ToArray();
                            if (Players.Length > 0)
                            {
                                Players.First().PlayerInfo.Ping = player.Ping;
                            }
                        }
                        break;

                    case 3:
                        foreach (PlayerInfo player in playerInfoPacket.Players)
                        {
                            Player[] Players = MinecraftClient.Players.Where(x => x.PlayerInfo?.UUID.ToString() == player.UUID.ToString()).ToArray();
                            if (Players.Length > 0)
                            {
                                Players.First().PlayerInfo.HasDisplayName = player.HasDisplayName;
                                Players.First().PlayerInfo.DisplayName = player.DisplayName;
                            }
                        }
                        break;

                    case 4:
                        foreach (PlayerInfo player in playerInfoPacket.Players)
                        {
                            Player[] Players = MinecraftClient.Players.Where(x => x.PlayerInfo?.UUID.ToString() == player.UUID.ToString()).ToArray();
                            if (Players.Length > 0)
                            {
                                MinecraftClient.Players.Remove(Players.First());
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

                SendMovement(MinecraftClient, NewPos, NewRot, MinecraftClient.GetLocalPlayer().IsOnGround);
            }

            if (Packet is Packets.Client.Play.DisconnectPacket disconnectPacket)
            {
                Logger.LogError($"{MinecraftClient.APIClient.CurrentUser.name} Disconnected: {disconnectPacket.Message}");
            }

            if (Packet is SpawnPlayerPacket spawnPlayerPacket)
            {
                Player[] Players = MinecraftClient.Players.Where(x => x.PlayerInfo.UUID.ToString() == spawnPlayerPacket.UUID.ToString()).ToArray();
                if (Players.Length > 0)
                {
                    Players.First().Position = new Vector3((float)spawnPlayerPacket.X, (float)spawnPlayerPacket.Y, (float)spawnPlayerPacket.Z);
                    Players.First().Rotation = new Vector2(spawnPlayerPacket.Yaw, spawnPlayerPacket.Pitch);
                    Players.First().EntityID = spawnPlayerPacket.EntityId;
                }
            }

            // Entity 
            if (Packet is SpawnLivingEntity entityAliveSpawnPacket)
            {
                if (entityAliveSpawnPacket.Type == 116)
                {
                    Player[] Players = MinecraftClient.Players.Where(x => x.EntityID == entityAliveSpawnPacket.EntityId || x.PlayerInfo.UUID.ToString() == entityAliveSpawnPacket.EntityUUID.ToString()).ToArray();
                    if (Players.Length > 0)
                    {
                        Players.First().EntityID = entityAliveSpawnPacket.EntityId;
                        Players.First().Position = new Vector3((float)entityAliveSpawnPacket.XPosition, (float)entityAliveSpawnPacket.YPosition, (float)entityAliveSpawnPacket.ZPosition);
                        Players.First().Rotation = new Vector2(entityAliveSpawnPacket.Yaw, entityAliveSpawnPacket.Pitch);
                        Players.First().Velocity = new Vector3(entityAliveSpawnPacket.XVelocity, entityAliveSpawnPacket.YVelocity, entityAliveSpawnPacket.ZVelocity);
                    }
                }
            }

            if (Packet is EntityPositionAndRotationPacket entityPosAndRotPacket)
            {
                Player[] Players = MinecraftClient.Players.Where(x => x.EntityID == entityPosAndRotPacket.EntityId).ToArray();
                if (Players.Length > 0)
                {
                    Players.First().Position += new Vector3(entityPosAndRotPacket.DeltaX / 4096, entityPosAndRotPacket.DeltaY / 4096, entityPosAndRotPacket.DeltaZ / 4096);
                    Players.First().Rotation = new Vector2(entityPosAndRotPacket.Yaw, entityPosAndRotPacket.Pitch);
                    Players.First().IsOnGround = entityPosAndRotPacket.OnGround;
                }
            }

            if (Packet is EntityPositionPacket entityPosPacket)
            {

                Player[] Players = MinecraftClient.Players.Where(x => x.EntityID == entityPosPacket.EntityId).ToArray();
                if (Players.Length > 0)
                {
                    Players.First().Position += new Vector3(entityPosPacket.DeltaX / 4096, entityPosPacket.DeltaY / 4096, entityPosPacket.DeltaZ / 4096);
                    Players.First().IsOnGround = entityPosPacket.OnGround;
                }
            }

            if (Packet is EntityRotationPacket entityRotPacket)
            {
                Player[] Players = MinecraftClient.Players.Where(x => x.EntityID == entityRotPacket.EntityId).ToArray();
                if (Players.Length > 0)
                {
                    Players.First().Rotation = new Vector2(entityRotPacket.Yaw, entityRotPacket.Pitch);
                    Players.First().IsOnGround = entityRotPacket.OnGround;
                }
            }

            if (Packet is EntityTeleportPacket entityTeleportPacket)
            {
                Player[] Players = MinecraftClient.Players.Where(x => x.EntityID == entityTeleportPacket.EntityId).ToArray();
                if (Players.Length > 0)
                {
                    Players.First().Position = new Vector3((float)entityTeleportPacket.X, (float)entityTeleportPacket.Y, (float)entityTeleportPacket.Z);
                    Players.First().Rotation = new Vector2(entityTeleportPacket.Yaw, entityTeleportPacket.Pitch);
                    Players.First().IsOnGround = entityTeleportPacket.OnGround;
                }
            }

            if (Packet is EntityVelocityPacket entityVelocityPacket)
            {
                Player[] Players = MinecraftClient.Players.Where(x => x.EntityID == entityVelocityPacket.EntityId).ToArray();
                if (Players.Length > 0)
                {
                    Players.First().Velocity = new Vector3(entityVelocityPacket.XVelocity, entityVelocityPacket.YVelocity, entityVelocityPacket.ZVelocity);
                }
            }

            if (Packet is EntityHeadLookPacket entityLookPacket)
            {
                Player[] Players = MinecraftClient.Players.Where(x => x.EntityID == entityLookPacket.EntityId).ToArray();
                if (Players.Length > 0)
                {
                    Players.First().Rotation = new Vector2(entityLookPacket.HeadYaw, Players.First().Rotation.Y);
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
