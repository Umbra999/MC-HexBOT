using MCHexBOT.Network;
using MCHexBOT.Utils.Math;

namespace MCHexBOT.Pakets.Client.Play
{
    public class SpawnPositionPaket : IPaket
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
            minecraftStream.WritePosition(Location);
            minecraftStream.WriteFloat(Angle);
        }
    }
}
