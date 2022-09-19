using MCHexBOT.Network;

namespace MCHexBOT.Packets.Client.Play
{
    public class EntityPositionPacket : IPacket
    {
        public int EntityId { get; set; }
        public short DeltaX { get; set; }
        public short DeltaY { get; set; }
        public short DeltaZ { get; set; }
        public bool OnGround { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
            DeltaX = minecraftStream.ReadShort();
            DeltaY = minecraftStream.ReadShort();
            DeltaZ = minecraftStream.ReadShort();
            OnGround = minecraftStream.ReadBool();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
