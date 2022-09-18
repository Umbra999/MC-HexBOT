using MCHexBOT.Network;
using MCHexBOT.Packets;

namespace MCHexBOT.Protocol.Packets.LabyClient
{
    internal class HelloPacket : IPacket
    {
        public long TickTime { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            TickTime = minecraftStream.ReadLong();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteLong(TickTime);
        }
    }
}
