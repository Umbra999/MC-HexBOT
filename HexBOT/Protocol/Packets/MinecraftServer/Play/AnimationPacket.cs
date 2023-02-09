using HexBOT.Network;

namespace HexBOT.Packets.Server.Play
{
    public class AnimationPacket : IPacket
    {
        public void Decode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            // No Data
        }
    }
}
