using HexBOT.Network;

namespace HexBOT.Packets.Server.Play
{
    public class PlayerPacket : IPacket
    {
        public bool OnGround { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteBool(OnGround);
        }
    }
}
