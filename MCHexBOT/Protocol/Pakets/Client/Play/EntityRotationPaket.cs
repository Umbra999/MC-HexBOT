using MCHexBOT.Network;
using MCHexBOT.Pakets;

namespace MCHexBOT.Protocol.Pakets.Client.Play
{
    internal class EntityRotationPaket : IPaket
    {
        public int EntityId { get; set; }
        public byte[] Yaw { get; set; }
        public byte[] Pitch { get; set; }
        public bool OnGround { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
            Yaw = minecraftStream.Read(1);
            Pitch = minecraftStream.Read(1);
            OnGround = minecraftStream.ReadBool();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(EntityId);
            minecraftStream.Write(Yaw, 0, 1);
            minecraftStream.Write(Pitch, 0, 1);
            minecraftStream.WriteBool(OnGround);
        }
    }
}
