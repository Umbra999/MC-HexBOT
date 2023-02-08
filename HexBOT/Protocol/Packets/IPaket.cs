using HexBOT.Network;

namespace HexBOT.Packets
{
    public interface IPacket
    {
        public void Encode(MinecraftStream minecraftStream);
        public void Decode(MinecraftStream minecraftStream);
    }
}
