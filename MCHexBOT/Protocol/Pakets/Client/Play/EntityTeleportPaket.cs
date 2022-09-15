using MCHexBOT.Network;
using MCHexBOT.Pakets;

namespace MCHexBOT.Protocol.Pakets.Client.Play
{
    internal class EntityTeleportPaket : IPaket
    {
        public int EntityId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public byte[] Yaw { get; set; }
        public byte[] Pitch { get; set; }
        public bool OnGround { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
            X = minecraftStream.ReadDouble();
            Y = minecraftStream.ReadDouble();
            Z = minecraftStream.ReadDouble();
            Yaw = minecraftStream.Read(1);
            Pitch = minecraftStream.Read(1);
            OnGround = minecraftStream.ReadBool();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(EntityId);
            minecraftStream.WriteDouble(X);
            minecraftStream.WriteDouble(Y);
            minecraftStream.WriteDouble(Z);
            minecraftStream.Write(Yaw, 0, 1);
            minecraftStream.Write(Pitch, 0, 1);
            minecraftStream.WriteBool(OnGround);
        }
    }
}
