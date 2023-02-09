using HexBOT.Core.Minecraft;
using HexBOT.HexServer;
using HexBOT.Network;
using HexBOT.Packets;
using HexBOT.Packets.Client.Login;
using HexBOT.Packets.Client.Play;
using HexBOT.Protocol;
using HexBOT.Protocol.Utils;
using HexBOT.Utils;
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
                Logger.LogError($"{MinecraftClient.APIClient.CurrentUser.name} Disconnected: {disconnectPacket.Reason}");
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
                //switch (playerInfoPacket.Action)
                //{
                //    case 0:
                //        foreach (PlayerInfo player in playerInfoPacket.Players)
                //        {
                //            if (player.Name == MinecraftClient.APIClient.CurrentUser.name)
                //            {
                //                Player[] Players = MinecraftClient.EntityManager.AllPlayers.Where(x => x.IsLocal).ToArray();
                //                if (Players.Length > 0)
                //                {
                //                    Players.First().PlayerInfo = player;
                //                    Players.First().IsOnGround = true;
                //                    Players.First().Health = 20;
                //                    Players.First().Food = 20;
                //                    Players.First().Saturation = 5;
                //                    Players.First().Position = new Vector3(0, 0, 0);
                //                    Players.First().Rotation = new Vector2(0, 0);
                //                    Players.First().Velocity = new Vector3(0, 0, 0);
                //                    Players.First().HeldItemSlot = 0;
                //                    Players.First().IsSneaking = false;
                //                    Players.First().IsSprinting = false;
                //                    Players.First().IsBurning = false;
                //                    Players.First().IsInvisible = false;
                //                }
                //            }

                //            else if (MinecraftClient.EntityManager.AllPlayers.Where(x => x.PlayerInfo?.UUID.ToString() == player.UUID.ToString()).ToArray().Length == 0)
                //            {
                //                MinecraftClient.EntityManager.AddPlayer(new Player()
                //                {
                //                    PlayerInfo = player,
                //                    EntityID = 0,
                //                    IsLocal = false,
                //                    IsOnGround = true,
                //                    Health = 20,
                //                    Food = 20,
                //                    Saturation = 5,
                //                    Position = new Vector3(0, 0, 0),
                //                    Rotation = new Vector2(0, 0),
                //                    Velocity = new Vector3(0, 0, 0),
                //                    HeldItemSlot = 0,
                //                    IsSneaking = false,
                //                    IsSprinting = false,
                //                    IsBurning = false,
                //                    IsInvisible = false
                //                });
                //                //Logger.Log($"[ + ] {player.Name}");

                //                IPEndPoint CurrentServer = (IPEndPoint)MinecraftClient.MCConnection.Tcp.Client.RemoteEndPoint;
                //                ServerHandler.CheckOverseePlayer(player.UUID.ToString(), player.Name, CurrentServer.Address.ToString() + ":" + CurrentServer.Port);
                //            }
                //        }
                //        break;

                //    case 1:
                //        foreach (PlayerInfo player in playerInfoPacket.Players)
                //        {
                //            Player[] Players = MinecraftClient.EntityManager.AllPlayers.Where(x => x.PlayerInfo?.UUID.ToString() == player.UUID.ToString()).ToArray();
                //            if (Players.Length > 0)
                //            {
                //                Players.First().PlayerInfo.GameMode = player.GameMode;
                //            }
                //        }
                //        break;

                //    case 2:
                //        foreach (PlayerInfo player in playerInfoPacket.Players)
                //        {
                //            Player[] Players = MinecraftClient.EntityManager.AllPlayers.Where(x => x.PlayerInfo?.UUID.ToString() == player.UUID.ToString()).ToArray();
                //            if (Players.Length > 0)
                //            {
                //                Players.First().PlayerInfo.Ping = player.Ping;
                //            }
                //        }
                //        break;

                //    case 3:
                //        foreach (PlayerInfo player in playerInfoPacket.Players)
                //        {
                //            Player[] Players = MinecraftClient.EntityManager.AllPlayers.Where(x => x.PlayerInfo?.UUID.ToString() == player.UUID.ToString()).ToArray();
                //            if (Players.Length > 0)
                //            {
                //                Players.First().PlayerInfo.HasDisplayName = player.HasDisplayName;
                //                Players.First().PlayerInfo.DisplayName = player.DisplayName;
                //            }
                //        }
                //        break;

                //    case 4:
                //        foreach (PlayerInfo player in playerInfoPacket.Players)
                //        {
                //            Player[] Players = MinecraftClient.EntityManager.AllPlayers.Where(x => x.PlayerInfo?.UUID.ToString() == player.UUID.ToString()).ToArray();
                //            if (Players.Length > 0)
                //            {
                //                MinecraftClient.EntityManager.ClearPlayer(Players.First());
                //            }
                //        }
                //        break;
                //}
            }

            if (Packet is UpdateHealthPacket updateHealthPacket)
            {
                //MinecraftClient.EntityManager.LocalPlayer.Food = updateHealthPacket.Food;
                //MinecraftClient.EntityManager.LocalPlayer.Health = updateHealthPacket.Health;
                //MinecraftClient.EntityManager.LocalPlayer.Saturation = updateHealthPacket.Saturation;
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

            //if (Packet is PlayerPositionAndLookPacket positionPacket)
            //{
            //    Vector3 NewPos = MinecraftClient.EntityManager.LocalPlayer.Position;
            //    Vector2 NewRot = MinecraftClient.EntityManager.LocalPlayer.Rotation;

            //    if ((positionPacket.Flags & 0x01) == 0x01) NewPos.X += (float)positionPacket.X;
            //    else NewPos.X = (float)positionPacket.X;

            //    if ((positionPacket.Flags & 0x02) == 0x02) NewPos.Y += (float)positionPacket.Y!;
            //    else NewPos.Y = (float)positionPacket.Y!;

            //    if ((positionPacket.Flags & 0x04) == 0x04) NewPos.Z += (float)positionPacket.Z!;
            //    else NewPos.Z = (float)positionPacket.Z!;

            //    if ((positionPacket.Flags & 0x08) == 0x08) NewRot.Y += positionPacket.Pitch!;
            //    else NewRot.Y = positionPacket.Pitch!;

            //    if ((positionPacket.Flags & 0x10) == 0x10) NewPos.X += positionPacket.Yaw!;
            //    else NewRot.X = positionPacket.Yaw!;

            //    SendMovement(MinecraftClient, NewPos, NewRot, MinecraftClient.EntityManager.LocalPlayer.IsOnGround);
            //}

            if (Packet is Packets.Client.Play.DisconnectPacket disconnectPacket)
            {
                Logger.LogError($"{MinecraftClient.APIClient.CurrentUser.name} Disconnected: {disconnectPacket.Reason}");
            }

            // Entity 
            if (Packet is SpawnPlayerPacket spawnPlayerPacket)
            {
                //MinecraftClient.EntityManager.AddPlayer(new Player()
                //{
                //    EntityID = spawnPlayerPacket.EntityId,
                //    UUID = spawnPlayerPacket.UUID,
                //    Position = new Vector3((float)spawnPlayerPacket.X, (float)spawnPlayerPacket.Y, (float)spawnPlayerPacket.Z),
                //    Rotation = new Vector2(spawnPlayerPacket.Yaw, spawnPlayerPacket.Pitch)
                //});

                //// this might not be needed anymore?
                //Player[] Players = MinecraftClient.Players.Where(x => x.PlayerInfo.UUID.ToString() == spawnPlayerPacket.UUID.ToString()).ToArray();
                //if (Players.Length > 0)
                //{
                //    Players.First().Position = new Vector3((float)spawnPlayerPacket.X, (float)spawnPlayerPacket.Y, (float)spawnPlayerPacket.Z);
                //    Players.First().Rotation = new Vector2(spawnPlayerPacket.Yaw, spawnPlayerPacket.Pitch);
                //    Players.First().EntityID = spawnPlayerPacket.EntityId;
                //}
            }

            //if (Packet is SpawnMobPacket entityAliveSpawnPacket)
            //{
            //    if (entityAliveSpawnPacket.Type == 116)
            //    {
            //        Player[] Players = MinecraftClient.EntityManager.AllPlayers.Where(x => x.EntityID == entityAliveSpawnPacket.EntityId).ToArray();
            //        if (Players.Length > 0)
            //        {
            //            Players.First().EntityID = entityAliveSpawnPacket.EntityId;
            //            Players.First().Position = new Vector3((float)entityAliveSpawnPacket.XPosition, (float)entityAliveSpawnPacket.YPosition, (float)entityAliveSpawnPacket.ZPosition);
            //            Players.First().Rotation = new Vector2(entityAliveSpawnPacket.Yaw, entityAliveSpawnPacket.Pitch);
            //            Players.First().Velocity = new Vector3(entityAliveSpawnPacket.XVelocity, entityAliveSpawnPacket.YVelocity, entityAliveSpawnPacket.ZVelocity);
            //        }
            //    }
            //}

            //if (Packet is EntityRelativeMovementAndRotationPacket entityPosAndRotPacket)
            //{
            //    Player[] Players = MinecraftClient.EntityManager.AllPlayers.Where(x => x.EntityID == entityPosAndRotPacket.EntityId).ToArray();
            //    if (Players.Length > 0)
            //    {
            //        Players.First().Position += new Vector3((float)entityPosAndRotPacket.DeltaX / 4096, (float)entityPosAndRotPacket.DeltaY / 4096, (float)entityPosAndRotPacket.DeltaZ / 4096); // idk if thats still needed with da math
            //        Players.First().Rotation = new Vector2(entityPosAndRotPacket.Yaw, entityPosAndRotPacket.Pitch);
            //        Players.First().IsOnGround = entityPosAndRotPacket.OnGround;
            //    }
            //}

            //if (Packet is EntityRelativeMovementPacket entityPosPacket)
            //{

            //    Player[] Players = MinecraftClient.EntityManager.AllPlayers.Where(x => x.EntityID == entityPosPacket.EntityId).ToArray();
            //    if (Players.Length > 0)
            //    {
            //        Players.First().Position += new Vector3((float)entityPosPacket.DeltaX / 4096, (float)entityPosPacket.DeltaY / 4096, (float)entityPosPacket.DeltaZ / 4096); // idk if thats still needed with da math
            //        Players.First().IsOnGround = entityPosPacket.OnGround;
            //    }
            //}

            //if (Packet is EntityRotationPacket entityRotPacket)
            //{
            //    Player[] Players = MinecraftClient.EntityManager.AllPlayers.Where(x => x.EntityID == entityRotPacket.EntityId).ToArray();
            //    if (Players.Length > 0)
            //    {
            //        Players.First().Rotation = new Vector2(entityRotPacket.Yaw, entityRotPacket.Pitch);
            //        Players.First().IsOnGround = entityRotPacket.OnGround;
            //    }
            //}

            //if (Packet is EntityTeleportPacket entityTeleportPacket)
            //{
            //    Player[] Players = MinecraftClient.EntityManager.AllPlayers.Where(x => x.EntityID == entityTeleportPacket.EntityId).ToArray();
            //    if (Players.Length > 0)
            //    {
            //        Players.First().Position = new Vector3((float)entityTeleportPacket.X, (float)entityTeleportPacket.Y, (float)entityTeleportPacket.Z);
            //        Players.First().Rotation = new Vector2(entityTeleportPacket.Yaw, entityTeleportPacket.Pitch);
            //        Players.First().IsOnGround = entityTeleportPacket.OnGround;
            //    }
            //}

            //if (Packet is EntityVelocityPacket entityVelocityPacket)
            //{
            //    Player[] Players = MinecraftClient.EntityManager.AllPlayers.Where(x => x.EntityID == entityVelocityPacket.EntityId).ToArray();
            //    if (Players.Length > 0)
            //    {
            //        Players.First().Velocity = new Vector3(entityVelocityPacket.XVelocity, entityVelocityPacket.YVelocity, entityVelocityPacket.ZVelocity);
            //    }
            //}

            //if (Packet is EntityHeadLookPacket entityLookPacket)
            //{
            //    Player[] Players = MinecraftClient.EntityManager.AllPlayers.Where(x => x.EntityID == entityLookPacket.EntityId).ToArray();
            //    if (Players.Length > 0)
            //    {
            //        Players.First().Rotation = new Vector2(entityLookPacket.HeadYaw, Players.First().Rotation.Y);
            //    }
            //}
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
