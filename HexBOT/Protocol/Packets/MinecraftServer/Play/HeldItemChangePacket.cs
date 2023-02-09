using HexBOT.Network;

namespace HexBOT.Packets.Server.Play
{
    internal class HeldItemChangePacket : IPacket
    {
        public short Slot; // 0 - 8
        public void Decode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteShort(Slot);
        }
    }
}
