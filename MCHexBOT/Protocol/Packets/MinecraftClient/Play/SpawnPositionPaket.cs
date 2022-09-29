using MCHexBOT.Network;
using System.Numerics;

namespace MCHexBOT.Packets.Client.Play
{
    public class SpawnPositionPacket : IPacket
    {
        public Vector3 Location { get; set; }
        public float Angle { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Location = minecraftStream.ReadPosition();
            Angle = minecraftStream.ReadFloat();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
