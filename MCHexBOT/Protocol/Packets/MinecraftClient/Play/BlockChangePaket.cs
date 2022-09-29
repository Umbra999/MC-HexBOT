using MCHexBOT.Network;
using System.Numerics;

namespace MCHexBOT.Packets.Client.Play
{
    public class BlockChangePacket : IPacket
    {
        public Vector3 Location { get; set; }
        public int BlockID { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Location = minecraftStream.ReadPosition();
            BlockID = minecraftStream.ReadVarInt();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
