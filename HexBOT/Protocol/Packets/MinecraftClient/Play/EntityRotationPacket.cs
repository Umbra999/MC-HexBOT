using HexBOT.Network;

namespace HexBOT.Packets.Client.Play
{
    internal class EntityRotationPacket : IPacket
    {
        public int EntityId { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public bool OnGround { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
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
