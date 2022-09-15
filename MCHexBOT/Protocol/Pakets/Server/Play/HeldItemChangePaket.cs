using MCHexBOT.Network;

namespace MCHexBOT.Pakets.Server.Play
{
    internal class HeldItemChangePaket : IPaket
    {
        public short Slot;
        public void Decode(MinecraftStream minecraftStream)
        {
            Slot = minecraftStream.ReadShort();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteShort(Slot);
        }
    }
}
