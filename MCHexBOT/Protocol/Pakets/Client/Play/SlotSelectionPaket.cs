using MCHexBOT.Network;

namespace MCHexBOT.Packets.Client.Play
{
    internal class SlotSelectionPacket : IPacket
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
