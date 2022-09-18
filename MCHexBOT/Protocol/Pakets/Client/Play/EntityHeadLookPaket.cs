using MCHexBOT.Network;
using MCHexBOT.Packets;

namespace MCHexBOT.Packets.Client.Play
{
    internal class EntityHeadLookPacket : IPacket
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
