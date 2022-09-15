using MCHexBOT.Network;

namespace MCHexBOT.Pakets.Client.Play
{
    public class EntityPositionAndRotationPaket : IPaket
    {
        public int EntityId { get; set; }
        public short DeltaX { get; set; }
        public short DeltaY { get; set; }
        public short DeltaZ { get; set; }
        public int Yaw { get; set; }
        public int Pitch { get; set; }
        public bool OnGround { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
            DeltaX = minecraftStream.ReadShort();
            DeltaY = minecraftStream.ReadShort();
            DeltaZ = minecraftStream.ReadShort();
            Yaw = minecraftStream.ReadByte();
            Pitch = minecraftStream.ReadByte();
            OnGround = minecraftStream.ReadBool();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(EntityId);
            minecraftStream.WriteDouble(DeltaX);
            minecraftStream.WriteDouble(DeltaY);
            minecraftStream.WriteDouble(DeltaZ);
            minecraftStream.WriteByte((byte)Yaw);
            minecraftStream.WriteByte((byte)Pitch);
            minecraftStream.WriteBool(OnGround);
        }
    }
}
