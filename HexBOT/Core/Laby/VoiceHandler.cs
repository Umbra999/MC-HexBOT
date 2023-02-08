using HexBOT.Network;
using HexBOT.Packets;
using HexBOT.Protocol.Packets.LabyClient.Login;
using HexBOT.Protocol.Packets.LabyServer.Login;
using HexBOT.Protocol.Utils;
using System.Security.Cryptography;

namespace HexBOT.Core
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
