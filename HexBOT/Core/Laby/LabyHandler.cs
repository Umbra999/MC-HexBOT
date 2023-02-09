using HexBOT.Network;
using HexBOT.Utils;
using HexBOT.Packets;
using HexBOT.Protocol;
using System.Security.Cryptography;
using HexBOT.Protocol.Packets.LabyClient.Handshake;
using HexBOT.Protocol.Packets.LabyClient.Login;
using HexBOT.Protocol.Packets.LabyServer.Login;
using HexBOT.Protocol.Utils;
using HexBOT.Core.Laby;

namespace HexBOT.Core
{
    internal class LabyHandler : IPacketHandler
    {
        private LabyClient LabyClient { get; set; }
        public ConnectionHandler Connection { get; set; }

        public LabyHandler(LabyClient minecraft)
        {
            LabyClient = minecraft;
        }

        public void Handshake(IPacket Packet)
        {
            if (Packet is HelloPacket pongPacket)
            {
                Connection.State = ConnectionState.Login;

                Connection.SendPacket(new LoginDataPacket()
                {
                    name = LabyClient.MinecraftClient.APIClient.CurrentUser.name,
                    id = UUID.AddDashesToUUID(LabyClient.MinecraftClient.APIClient.CurrentUser.id),
                    motd = "www.logout.space"
                });

                Connection.SendPacket(new LoginOptionPacket()
                {
                    ShowServer = true,
                    Status = 0,
                    TimeZone = "Europe/Moscow"
                });

                Connection.SendPacket(new LoginVersionPacket()
                {
                    Version = LabyClient.ProtocolVersion,
                    Name = "1.8.9_3.9.54",
                    UpdateUrl = ""
                });
            }
        }

        public void Login(IPacket Packet)
        {
            if (Packet is EncryptionRequestPacket encryptionRequestPacket)
            {
                Task.Run(() => HandleEncryptionAuth(encryptionRequestPacket));
            }

            if (Packet is LoginCompletePacket loginSuccessPacket)
            {
                Connection.EnableWriteEncryption(PrivateKey);
                Connection.State = ConnectionState.Play;

                Connection.SendPacket(new Protocol.Packets.LabyServer.Play.PlayServerPacket()
                {
                    IP = "➥ www.logout.space ⚠",
                    //IP = Encryption.RandomString(99900),
                    Port = 25565,
                    //Port = -999,
                    viaServerList = true,
                    //Gamemode = Encryption.RandomString(99900),
                    Gamemode = "❤ i hate everything about u ❤"
                });

                Logger.Log($"{LabyClient.MinecraftClient.APIClient.CurrentUser.name} connected to Labymod");
                LabyClient.DashboadPin = loginSuccessPacket.DashboardPin.pin;

                LabyClient.OnReceivedPin();
            }
        }

        public void Play(IPacket Packet)
        {
            if (Packet is Protocol.Packets.LabyClient.Play.PingPacket pingPacket)
            {
                Connection.SendPacket(new Protocol.Packets.LabyServer.Play.PongPacket());
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

            await LabyClient.MinecraftClient.APIClient.JoinServer(serverHash);

            Connection.EnableReadEncryption(PrivateKey);

            Connection.SendPacket(new EncryptionResponsePacket()
            {
                SharedKey = encrypted,
                VerifyToken = encVerTok,
                SharedKeyLenght = encrypted.Length,
                VerifyTokenLenght = encVerTok.Length
            });

            Logger.LogDebug($"{LabyClient.MinecraftClient.APIClient.CurrentUser.name} Authenticated to Labymod");
            return true;
        }
    }
}
