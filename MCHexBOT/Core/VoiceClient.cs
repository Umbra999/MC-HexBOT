using MCHexBOT.Network;
using MCHexBOT.Protocol;
using MCHexBOT.Protocol.Packets.LabyVoiceServer;
using MCHexBOT.Protocol.Utils;
using MCHexBOT.Utils;
using MCHexBOT.Utils.Data;
using Org.BouncyCastle.Bcpg;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace MCHexBOT.Core
{
    public class VoiceClient
    {
        public MinecraftConnection MCConnection;
        public LabyClient LabyClient;
        public int ProtocolVersion = 1;

        public VoiceClient(LabyClient Client)
        {
            LabyClient = Client;
            Task.Run(() => Connect("voice.labymod.net", 8066)); // 8077 for debug
        }

        public async Task<bool> Connect(string Host, int Port)
        {
            return false; // WIP

            TcpClient Client = new(Host, Port);

            MCConnection = new MinecraftConnection(Client, false);

            PacketRegistry writer = new();
            PacketRegistry.RegisterVoiceServerPackets(writer);

            PacketRegistry reader = new();
            PacketRegistry.RegisterVoiceClientPackets(reader);

            MCConnection.WriterRegistry = writer;
            MCConnection.ReaderRegistry = reader;

            MCConnection.Handler = new VoiceHandler(this)
            {
                Connection = MCConnection
            };

            MCConnection.Start();

            return true;
        }

        private byte[] PrivateKey;
        private async Task<bool> HandleEncryptionAuth()
        {
            PrivateKey = CryptoHandler.GenerateAESPrivateKey();


            MCConnection.SendPacket(new HandshakePacket()
            {
                Auth = 0,
                UUID = new UUID(LabyClient.MinecraftClient.APIClient.CurrentUser.id),
                Pin = LabyClient.DashboadPin,
                ProtocolVersion = ProtocolVersion,
            });

            Logger.LogDebug($"{LabyClient.MinecraftClient.APIClient.CurrentUser.name} Authenticated to LabyVoice");
            return true;
        }
    }
}
