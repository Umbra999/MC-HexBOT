using HexBOT.Network;

namespace HexBOT.Packets.Client.Play
{
    internal class EntityHeadLookPacket : IPacket
    {
        public int EntityId { get; set; }
        public float HeadYaw { get; set; }
        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
            HeadYaw = minecraftStream.ReadByte() / (256.0f / 360.0f); // this is a Angle?
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
