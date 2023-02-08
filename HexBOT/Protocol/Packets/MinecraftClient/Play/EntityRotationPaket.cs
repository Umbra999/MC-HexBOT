using HexBOT.Network;

namespace HexBOT.Packets.Client.Play
{
    internal class EntityRotationPacket : IPacket
    {
        public int EntityId { get; set; }
        public byte Yaw { get; set; }
        public byte Pitch { get; set; }
        public bool OnGround { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
            Yaw = (byte)minecraftStream.ReadByte();
            Pitch = (byte)minecraftStream.ReadByte();
            OnGround = minecraftStream.ReadBool();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
