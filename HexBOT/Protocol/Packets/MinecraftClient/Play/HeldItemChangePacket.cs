using HexBOT.Network;

namespace HexBOT.Packets.Client.Play
{
    internal class HeldItemChangePacket : IPacket
    {
        public byte Slot { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Slot = (byte)minecraftStream.ReadByte();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
