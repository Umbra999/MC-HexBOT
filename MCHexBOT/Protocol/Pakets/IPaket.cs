using MCHexBOT.Network;

namespace MCHexBOT.Packets
{
    public interface IPacket
    {
        public void Encode(MinecraftStream minecraftStream);
        public void Decode(MinecraftStream minecraftStream);
    }
}
