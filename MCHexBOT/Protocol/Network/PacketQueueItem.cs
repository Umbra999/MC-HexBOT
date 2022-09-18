using MCHexBOT.Packets;
using MCHexBOT.Protocol;

namespace MCHexBOT.Network
{
    public class PacketQueueItem
    {
        public IPacket Packet { get; set; }
        public ConnectionState State { get; set; }
    }
}
