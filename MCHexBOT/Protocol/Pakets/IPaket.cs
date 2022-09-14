using MCHexBOT.Network;

namespace MCHexBOT.Pakets
{
    public interface IPaket
    {
        public void Encode(MinecraftStream minecraftStream);
        public void Decode(MinecraftStream minecraftStream);
    }
}
