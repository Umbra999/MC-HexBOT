using MCHexBOT.Network;

namespace MCHexBOT.Packets.Server.Play
{
    public class PlayerMovementPacket : IPacket
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
