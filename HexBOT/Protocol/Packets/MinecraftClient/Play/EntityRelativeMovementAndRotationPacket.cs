using HexBOT.Network;

namespace HexBOT.Packets.Client.Play
{
    public class EntityRelativeMovementAndRotationPacket : IPacket
    {
        public int EntityId { get; set; }
        public double DeltaX { get; set; }
        public double DeltaY { get; set; }
        public double DeltaZ { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public bool OnGround { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
            DeltaX = Protocol.Utils.Math.ConvertFixedPoint(minecraftStream.ReadByte());
            DeltaY = Protocol.Utils.Math.ConvertFixedPoint(minecraftStream.ReadByte());
            DeltaZ = Protocol.Utils.Math.ConvertFixedPoint(minecraftStream.ReadByte());
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
