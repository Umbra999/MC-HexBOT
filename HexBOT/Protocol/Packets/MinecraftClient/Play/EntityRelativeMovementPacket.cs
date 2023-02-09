using HexBOT.Network;

namespace HexBOT.Packets.Client.Play
{
    public class EntityRelativeMovementPacket : IPacket
    {
        public int EntityId { get; set; }
        public double DeltaX { get; set; }
        public double DeltaY { get; set; }
        public double DeltaZ { get; set; }
        public bool OnGround { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
            DeltaX = Protocol.Utils.Math.ConvertFixedPoint(minecraftStream.ReadByte());
            DeltaY = Protocol.Utils.Math.ConvertFixedPoint(minecraftStream.ReadByte());
            DeltaZ = Protocol.Utils.Math.ConvertFixedPoint(minecraftStream.ReadByte());
            OnGround = minecraftStream.ReadBool();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
