using MCHexBOT.Pakets;
using MCHexBOT.Protocol;

namespace MCHexBOT.Network
{
    public class PacketQueueItem
    {
        public IPaket Paket { get; set; }
        public ConnectionState State { get; set; }
    }
}
