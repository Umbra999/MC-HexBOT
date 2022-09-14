using MCHexBOT.Network;

namespace MCHexBOT.Pakets.Server.Play
{
    public class PlayerMovementPaket : IPaket
    {
        public bool OnGround { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            OnGround = minecraftStream.ReadBool();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteBool(OnGround);
        }
    }
}
