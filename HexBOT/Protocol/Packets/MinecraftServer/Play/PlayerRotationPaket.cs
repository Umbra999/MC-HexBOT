using HexBOT.Network;

namespace HexBOT.Packets.Server.Play
{
    public class PlayerRotationPacket : IPacket
    {
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public bool OnGround { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteFloat(Yaw);
            minecraftStream.WriteFloat(Pitch);
            minecraftStream.WriteBool(OnGround);
        }
    }
}
