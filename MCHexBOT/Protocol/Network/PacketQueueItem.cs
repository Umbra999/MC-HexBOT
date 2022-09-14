using MCHexBOT.Enums;
using MCHexBOT.Pakets;

namespace MCHexBOT.Network
{
    public class PacketQueueItem
    {
        public IPaket Paket { get; set; }
        public MinecraftState State { get; set; }
    }
}
