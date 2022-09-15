using MCHexBOT.Network;
using MCHexBOT.Pakets;

namespace MCHexBOT.Protocol.Pakets.Client.Play
{
    internal class EntityRotationPaket : IPaket
    {
        public int EntityId { get; set; }
        public int Yaw { get; set; }
        public int Pitch { get; set; }
        public bool OnGround { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
            Yaw = minecraftStream.ReadByte();
            Pitch = minecraftStream.ReadByte();
            OnGround = minecraftStream.ReadBool();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(EntityId);
            minecraftStream.WriteByte((byte)Yaw);
            minecraftStream.WriteByte((byte)Pitch);
            minecraftStream.WriteBool(OnGround);
        }
    }
}
