using MCHexBOT.Network;
using MCHexBOT.Utils;
using MCHexBOT.Packets;
using MCHexBOT.Protocol;
using MCHexBOT.Utils.Data;
using MCHexBOT.Utils.Math;
using System.Security.Cryptography;
using System.Text;
using System.Numerics;
using MCHexBOT.Protocol.Packets.LabyClient.Handshake;
using MCHexBOT.Protocol.Packets.LabyClient.Login;
using MCHexBOT.Protocol.Packets.LabyServer.Login;
using MCHexBOT.Features;

namespace MCHexBOT.Core
{
    internal class LabyHandler : IPacketHandler
    {
        private LabyClient LabyClient { get; set; }
        public MinecraftConnection Connection { get; set; }

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
                    name = LabyClient.APIClient.CurrentUser.name,
                    id = new UUID(LabyClient.APIClient.CurrentUser.id),
                    motd = "www.logout.space"
                });

                Connection.SendPacket(new LoginOptionPacket()
                {
                    ShowServer = true,
                    Status = 0,
                    TimeZone = "Europe/Berlin"
                });

                Connection.SendPacket(new LoginVersionPacket()
                {
                    Version = LabyClient.ProtocolVersion,
                    Name = "1.8.9_3.9.46",
                    UpdateUrl = ""
                });
            }
        }

        public void Login(IPacket Packet)
        {
            if (Packet is EncryptionRequestPacket encryptionRequestPacket)
            {
                HandleEncryptionAuth(encryptionRequestPacket);
            }

            if (Packet is PingPacket pingPacket)
            {
                Connection.SendPacket(new PongPacket());
            }

            if (Packet is LoginCompletePacket loginSuccessPacket)
            {
                Connection.State = ConnectionState.Play;

                Connection.SendPacket(new Protocol.Packets.LabyServer.Play.PlayServerPacket()
                {
                    IP = "www.logout.space <3",
                    Port = 25565,
                    viaServerList = true,
                    Gamemode = "i simp for pokimane <3"
                });

                Logger.Log($"{LabyClient.APIClient.CurrentUser.name} connected to Labymod");
                Task.Run(() => LabyFeatures.CollectCoinLoop(LabyClient, loginSuccessPacket.DashboardPin.pin));
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

            await LabyClient.APIClient.JoinServer(Hex);

            RSA rsa = RsaHelper.DecodePublicKey(Packet.PublicKey!);
            if (rsa == null)
            {
                throw new Exception("Could not decode public key");
            }
            byte[] encrypted = rsa.Encrypt(aes.Key, RSAEncryptionPadding.Pkcs1);
            byte[] encVerTok = rsa.Encrypt(Packet.VerifyToken!, RSAEncryptionPadding.Pkcs1);

            Connection.SendPacket(new EncryptionResponsePacket()
            {
                SharedKey = encrypted,
                VerifyToken = encVerTok,
                SharedKeyLenght = encrypted.Length,
                VerifyTokenLenght = encVerTok.Length,
            });

            Connection.EnableEncryption(aes.Key);
        }
    }
}
