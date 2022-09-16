using MCHexBOT.Network;

namespace MCHexBOT.Pakets.Client.Play
{
    internal class SlotSelectionPaket : IPaket
    {
        public byte Slot { get; set; }
        public void Decode(MinecraftStream minecraftStream)
        {
            Slot = (byte)minecraftStream.ReadByte();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteByte(Slot);   
        }
    }
}
