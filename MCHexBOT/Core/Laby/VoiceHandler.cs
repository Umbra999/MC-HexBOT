using MCHexBOT.Network;
using MCHexBOT.Packets;
using MCHexBOT.Protocol.Packets.LabyClient.Login;
using MCHexBOT.Protocol.Packets.LabyServer.Login;
using MCHexBOT.Protocol.Utils;
using System.Security.Cryptography;

namespace MCHexBOT.Core
{
    internal class VoiceHandler : IPacketHandler
    {
        private VoiceClient VoiceClient { get; set; }
        public ConnectionHandler Connection { get; set; }

        public VoiceHandler(VoiceClient minecraft)
        {
            VoiceClient = minecraft;
        }

        public void Handshake(IPacket Packet)
        {
            
        }

        public void Login(IPacket Packet)
        {
           
        }

        public void Play(IPacket Packet)
        {
           
        }

        public void Status(IPacket Packet)
        {

        }
    }
}
