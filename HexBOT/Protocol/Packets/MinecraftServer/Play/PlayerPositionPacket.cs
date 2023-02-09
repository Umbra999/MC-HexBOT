using HexBOT.Network;

namespace HexBOT.Packets.Server.Play
{
    public class PlayerPositionPacket : IPacket
    {
        public double X { get; set; }
        public double Y { get; set; } // feet pos, normally Head Y - 1.62
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
