using MCHexBOT.Network;

namespace MCHexBOT.Pakets.Client.Play
{
    internal class EntityTeleportPaket : IPaket
    {
        public int EntityId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public int Yaw { get; set; }
        public int Pitch { get; set; }
        public bool OnGround { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
            X = minecraftStream.ReadDouble();
            Y = minecraftStream.ReadDouble();
            Z = minecraftStream.ReadDouble();
            Yaw = minecraftStream.ReadByte();
            Pitch = minecraftStream.ReadByte();
            OnGround = minecraftStream.ReadBool();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(EntityId);
            minecraftStream.WriteDouble(X);
            minecraftStream.WriteDouble(Y);
            minecraftStream.WriteDouble(Z);
            minecraftStream.WriteByte((byte)Yaw);
            minecraftStream.WriteByte((byte)Pitch);
            minecraftStream.WriteBool(OnGround);
        }
    }
}
