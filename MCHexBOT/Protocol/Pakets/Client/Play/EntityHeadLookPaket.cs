using MCHexBOT.Network;
using MCHexBOT.Pakets;

namespace MCHexBOT.Pakets.Client.Play
{
    internal class EntityHeadLookPaket : IPaket
    {
        public int EntityId { get; set; }
        public byte HeadYaw { get; set; }
        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
            HeadYaw = (byte)minecraftStream.ReadByte();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(EntityId);
            minecraftStream.WriteByte(HeadYaw);
        }
    }
}
