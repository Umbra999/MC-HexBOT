using HexBOT.Network;
using HexBOT.Packets;

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
