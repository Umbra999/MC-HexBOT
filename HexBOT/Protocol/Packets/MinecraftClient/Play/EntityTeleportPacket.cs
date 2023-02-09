using HexBOT.Network;

namespace HexBOT.Packets.Client.Play
{
    internal class EntityTeleportPacket : IPacket
    {
        public int EntityId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public bool OnGround { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
            X = Protocol.Utils.Math.ConvertFixedPoint(minecraftStream.ReadInt());
            Y = Protocol.Utils.Math.ConvertFixedPoint(minecraftStream.ReadInt());
            Z = Protocol.Utils.Math.ConvertFixedPoint(minecraftStream.ReadInt());
            Yaw = minecraftStream.ReadByte() / (256.0f / 360.0f); // this is a Angle?
            Pitch = minecraftStream.ReadByte() / (256.0f / 360.0f); // maybe put this into handler
            OnGround = minecraftStream.ReadBool();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
