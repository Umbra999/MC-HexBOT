using HexBOT.Network;

namespace HexBOT.Packets.Server.Play
{
    internal class HeldItemChangePacket : IPacket
    {
        public short Slot;
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
