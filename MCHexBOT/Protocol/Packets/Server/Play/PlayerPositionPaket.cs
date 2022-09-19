using MCHexBOT.Network;

namespace MCHexBOT.Packets.Server.Play
{
    public class PlayerPositionPacket : IPacket
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public bool OnGround { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteDouble(X);
            minecraftStream.WriteDouble(Y);
            minecraftStream.WriteDouble(Z);
            minecraftStream.WriteBool(OnGround);
        }
    }
}
