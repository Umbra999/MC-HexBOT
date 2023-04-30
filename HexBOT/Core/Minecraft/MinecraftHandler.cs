using HexBOT.Core.Minecraft;
using HexBOT.Features;
using HexBOT.HexedServer;
using HexBOT.Network;
using HexBOT.Packets;
using HexBOT.Packets.Client.Login;
using HexBOT.Packets.Client.Play;
using HexBOT.Protocol;
using HexBOT.Protocol.Utils;
using HexBOT.Utils;
using System.Net;
using System.Numerics;
using System.Security.Cryptography;

namespace HexBOT.Core
{
    internal class MinecraftHandler : IPacketHandler
    {
        public ConnectionHandler Connection { get; set; }
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
                Task.Run(() => HandleEncryptionAuth(encryptionPacket));
            }

            if (Packet is LoginSuccessPacket loginSuccessPacket)
            {
                Connection.EnableWriteEncryption(PrivateKey);

                Connection.State = ConnectionState.Play;
                Logger.LogSuccess($"Authenticated as {loginSuccessPacket.Username} [{loginSuccessPacket.Uuid}]");
            }

            if (Packet is Packets.Client.Login.DisconnectPacket disconnectPacket)
            {
                MinecraftClient.NetworkManager.OnDisconnectedFromServer(disconnectPacket.Reason);
            }
        }

        public void Play(IPacket Packet)
        {
            if (Packet is JoinGamePacket joinGamePacket)
            {
                MinecraftClient.NetworkManager.OnConnectedToServer(joinGamePacket);
            }

            if (Packet is KeepAlivePacket keepAlivePacket)
            {
               MinecraftClient.NetworkManager.OnPingReceived(keepAlivePacket);
            }

            if (Packet is PlayerListItemPacket playerInfoPacket)
            {
                switch (playerInfoPacket.Action)
                {
                    case 0:
                        foreach (PlayerListItemPacket.PlayerInfo player in playerInfoPacket.Players)
                        {
                            MinecraftClient.EntityManager.AddPlayer(new Player()
                            {
                                EntityID = 0,
                                IsLocal = player.Name == MinecraftClient.APIClient.CurrentUser.name,
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
                                IsBurning = false,
                                IsInvisible = false,
                                GameMode = player.GameMode,
                                UUID = player.UUID,
                                Name = player.Name,
                                NumberOfProperties = player.NumberOfProperties,
                                Properties = player.Properties,
                                Ping = player.Ping,
                                HasDisplayName = player.HasDisplayName,
                                DisplayName = player.DisplayName,
                            });

                            IPEndPoint CurrentServer = (IPEndPoint)MinecraftClient.MCConnection.Tcp.Client.RemoteEndPoint;
                            ServerHandler.CheckOverseePlayer(player.UUID, player.Name, CurrentServer.Address.ToString() + ":" + CurrentServer.Port);
                        }
                        break;

                    case 1:
                        foreach (PlayerListItemPacket.PlayerInfo player in playerInfoPacket.Players)
                        {
                            var MatchingPlayers = MinecraftClient.EntityManager.AllPlayers.Where(x => x.UUID == player.UUID);
                            if (MatchingPlayers != null)
                            {
                                Player[] players = MatchingPlayers.ToArray();
                                foreach (Player p in players)
                                {
                                    p.GameMode = player.GameMode;
                                }
                            }
                        }
                        break;

                    case 2:
                        foreach (PlayerListItemPacket.PlayerInfo player in playerInfoPacket.Players)
                        {
                            var MatchingPlayers = MinecraftClient.EntityManager.AllPlayers.Where(x => x.UUID == player.UUID);
                            if (MatchingPlayers != null)
                            {
                                Player[] players = MatchingPlayers.ToArray();
                                foreach (Player p in players)
                                {
                                    p.Ping = player.Ping;
                                }
                            }
                        }
                        break;

                    case 3:
                        foreach (PlayerListItemPacket.PlayerInfo player in playerInfoPacket.Players)
                        {
                            var MatchingPlayers = MinecraftClient.EntityManager.AllPlayers.Where(x => x.UUID == player.UUID);
                            if (MatchingPlayers != null)
                            {
                                Player[] players = MatchingPlayers.ToArray();
                                foreach (Player p in players)
                                {
                                    p.HasDisplayName = player.HasDisplayName;
                                    p.DisplayName = player.DisplayName;
                                }
                            }
                        }
                        break;

                    case 4:
                        foreach (PlayerListItemPacket.PlayerInfo player in playerInfoPacket.Players)
                        {
                            var MatchingPlayers = MinecraftClient.EntityManager.AllPlayers.Where(x => x.UUID == player.UUID);
                            if (MatchingPlayers != null)
                            {
                                Player[] players = MatchingPlayers.ToArray();
                                foreach (Player p in players)
                                {
                                    MinecraftClient.EntityManager.ClearPlayer(p);
                                }
                            }
                        }
                        break;
                }
            }

            if (Packet is UpdateHealthPacket updateHealthPacket)
            {
                MinecraftClient.EntityManager.LocalPlayer.Food = updateHealthPacket.Food;
                MinecraftClient.EntityManager.LocalPlayer.Health = updateHealthPacket.Health;
                MinecraftClient.EntityManager.LocalPlayer.Saturation = updateHealthPacket.Saturation;
                Logger.LogDebug($"Health update: Health: {updateHealthPacket.Health} Food: {updateHealthPacket.Food} Saturation: {updateHealthPacket.Saturation}");
            }

            if (Packet is ChatMessagePacket messagePacket)
            {
                MinecraftClient.ChatManager.OnChatMessageReceived(messagePacket);
            }

            if (Packet is PluginMessagePacket pluginPacket)
            {
                MinecraftClient.NetworkManager.OnPluginMessageReceived(pluginPacket);
            }

            if (Packet is PlayerPositionAndLookPacket positionPacket)
            {
                Logger.LogDebug("Server forced Position and Rotation");

                if (MinecraftClient.EntityManager.LocalPlayer == null)
                {
                    MinecraftClient.EntityManager.AddPlayer(new Player()
                    {
                        IsLocal = true,
                        Position = Vector3.Zero,
                        Rotation = Vector2.Zero,
                        IsOnGround = true,
                    });
                }

                Vector3 NewPos = MinecraftClient.EntityManager.LocalPlayer.Position;
                Vector2 NewRot = MinecraftClient.EntityManager.LocalPlayer.Rotation;

                if ((positionPacket.Flags & 0x01) == 0x01) NewPos.X += (float)positionPacket.X;
                else NewPos.X = (float)positionPacket.X;

                if ((positionPacket.Flags & 0x02) == 0x02) NewPos.Y += (float)positionPacket.Y;
                else NewPos.Y = (float)positionPacket.Y;

                if ((positionPacket.Flags & 0x04) == 0x04) NewPos.Z += (float)positionPacket.Z;
                else NewPos.Z = (float)positionPacket.Z;

                if ((positionPacket.Flags & 0x08) == 0x08) NewRot.Y += positionPacket.Pitch;
                else NewRot.Y = positionPacket.Pitch;

                if ((positionPacket.Flags & 0x10) == 0x10) NewPos.X += positionPacket.Yaw;
                else NewRot.X = positionPacket.Yaw;

                MinecraftClient.EntityManager.LocalPlayer.Position = NewPos;
                MinecraftClient.EntityManager.LocalPlayer.Rotation = NewRot;

                Movement.SendPositionAndRotation(MinecraftClient, NewPos, NewRot, MinecraftClient.EntityManager.LocalPlayer.IsOnGround);
            }

            if (Packet is Packets.Client.Play.DisconnectPacket disconnectPacket)
            {
                MinecraftClient.NetworkManager.OnDisconnectedFromServer(disconnectPacket.Reason);
            }

            // Entity 
            if (Packet is SpawnPlayerPacket spawnPlayerPacket)
            {
                var MatchingPlayers = MinecraftClient.EntityManager.AllPlayers.Where(x => x.UUID == spawnPlayerPacket.UUID);
                if (MatchingPlayers != null)
                {
                    Player[] players = MatchingPlayers.ToArray();
                    foreach (Player p in players)
                    {
                        p.EntityID = spawnPlayerPacket.EntityId;
                        p.Position = new Vector3((float)spawnPlayerPacket.X, (float)spawnPlayerPacket.Y, (float)spawnPlayerPacket.Z);
                        p.Rotation = new Vector2(spawnPlayerPacket.Yaw, spawnPlayerPacket.Pitch);
                    }
                }
            }

            if (Packet is SpawnMobPacket entityAliveSpawnPacket)
            {
                if (entityAliveSpawnPacket.Type == 116)
                {
                    var MatchingPlayers = MinecraftClient.EntityManager.AllPlayers.Where(x => x.EntityID == entityAliveSpawnPacket.EntityId);
                    if (MatchingPlayers != null)
                    {
                        Player[] players = MatchingPlayers.ToArray();
                        foreach (Player p in players)
                        {
                            p.Position = new Vector3((float)entityAliveSpawnPacket.XPosition, (float)entityAliveSpawnPacket.YPosition, (float)entityAliveSpawnPacket.ZPosition);
                            p.Rotation = new Vector2(entityAliveSpawnPacket.Yaw, entityAliveSpawnPacket.Pitch);
                            p.Velocity = new Vector3(entityAliveSpawnPacket.XVelocity, entityAliveSpawnPacket.YVelocity, entityAliveSpawnPacket.ZVelocity);
                        }
                    }
                }
            }

            if (Packet is EntityRelativeMovementAndRotationPacket entityPosAndRotPacket)
            {
                var MatchingPlayers = MinecraftClient.EntityManager.AllPlayers.Where(x => x.EntityID == entityPosAndRotPacket.EntityId);
                if (MatchingPlayers != null)
                {
                    Player[] players = MatchingPlayers.ToArray();
                    foreach (Player p in players)
                    {
                        p.Position += new Vector3((float)entityPosAndRotPacket.DeltaX / 4096, (float)entityPosAndRotPacket.DeltaY / 4096, (float)entityPosAndRotPacket.DeltaZ / 4096); // idk if thats still needed with da math
                        p.Rotation = new Vector2(entityPosAndRotPacket.Yaw, entityPosAndRotPacket.Pitch);
                        p.IsOnGround = entityPosAndRotPacket.OnGround;
                    }
                }
            }

            if (Packet is EntityRelativeMovementPacket entityPosPacket)
            {
                var MatchingPlayers = MinecraftClient.EntityManager.AllPlayers.Where(x => x.EntityID == entityPosPacket.EntityId);
                if (MatchingPlayers != null)
                {
                    Player[] players = MatchingPlayers.ToArray();
                    foreach (Player p in players)
                    {
                        p.Position += new Vector3((float)entityPosPacket.DeltaX / 4096, (float)entityPosPacket.DeltaY / 4096, (float)entityPosPacket.DeltaZ / 4096); // idk if thats still needed with da math
                        p.IsOnGround = entityPosPacket.OnGround;
                    }
                }
            }

            if (Packet is EntityRotationPacket entityRotPacket)
            {
                var MatchingPlayers = MinecraftClient.EntityManager.AllPlayers.Where(x => x.EntityID == entityRotPacket.EntityId);
                if (MatchingPlayers != null)
                {
                    Player[] players = MatchingPlayers.ToArray();
                    foreach (Player p in players)
                    {
                        p.Rotation = new Vector2(entityRotPacket.Yaw, entityRotPacket.Pitch);
                        p.IsOnGround = entityRotPacket.OnGround;
                    }
                }
            }

            if (Packet is EntityTeleportPacket entityTeleportPacket)
            {
                var MatchingPlayers = MinecraftClient.EntityManager.AllPlayers.Where(x => x.EntityID == entityTeleportPacket.EntityId);
                if (MatchingPlayers != null)
                {
                    Player[] players = MatchingPlayers.ToArray();
                    foreach (Player p in players)
                    {
                        p.Position = new Vector3((float)entityTeleportPacket.X, (float)entityTeleportPacket.Y, (float)entityTeleportPacket.Z);
                        p.Rotation = new Vector2(entityTeleportPacket.Yaw, entityTeleportPacket.Pitch);
                        p.IsOnGround = entityTeleportPacket.OnGround;
                    }
                }
            }

            if (Packet is EntityVelocityPacket entityVelocityPacket)
            {
                var MatchingPlayers = MinecraftClient.EntityManager.AllPlayers.Where(x => x.EntityID == entityVelocityPacket.EntityId);
                if (MatchingPlayers != null)
                {
                    Player[] players = MatchingPlayers.ToArray();
                    foreach (Player p in players)
                    {
                        p.Velocity = new Vector3(entityVelocityPacket.XVelocity, entityVelocityPacket.YVelocity, entityVelocityPacket.ZVelocity);
                    }
                }
            }

            if (Packet is EntityHeadLookPacket entityLookPacket)
            {
                var MatchingPlayers = MinecraftClient.EntityManager.AllPlayers.Where(x => x.EntityID == entityLookPacket.EntityId);
                if (MatchingPlayers != null)
                {
                    Player[] players = MatchingPlayers.ToArray();
                    foreach (Player p in players)
                    {
                        p.Rotation = new Vector2(entityLookPacket.HeadYaw, p.Rotation.Y);
                    }
                }
            }
        }

        public void Status(IPacket Packet)
        {
           
        }

        private byte[] PrivateKey;
        private async Task<bool> HandleEncryptionAuth(EncryptionRequestPacket Packet)
        {
            if (Packet.ServerId == "-") return false;

            PrivateKey = CryptoHandler.GenerateAESPrivateKey();

            string serverHash = CryptoHandler.getServerHash(Packet.ServerId, Packet.PublicKey, PrivateKey);

            RSA rsa = CryptoHandler.DecodeRSAPublicKey(Packet.PublicKey);

            byte[] encrypted = rsa.Encrypt(PrivateKey, RSAEncryptionPadding.Pkcs1);
            byte[] encVerTok = rsa.Encrypt(Packet.VerifyToken!, RSAEncryptionPadding.Pkcs1);

            await MinecraftClient.APIClient.JoinServer(serverHash);

            Connection.EnableReadEncryption(PrivateKey);

            Connection.SendPacket(new Packets.Server.Login.EncryptionResponsePacket()
            {
                SharedKey = encrypted,
                VerifyToken = encVerTok,
                SharedKeyLenght = encrypted.Length,
                VerifyTokenLenght = encVerTok.Length
            });

            Logger.LogDebug($"{MinecraftClient.APIClient.CurrentUser.name} Authenticated to Minecraft");
            return true;
        }
    }
}
