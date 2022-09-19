using MCHexBOT.Network;
using System.Numerics;

namespace MCHexBOT.Packets.Client.Play
{
    public class BlockChangePacket : IPacket
    {
        public Vector3 Location { get; set; }
        public int BlockId { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            ulong ll = minecraftStream.ReadULong();
            var x = ll >> 38;
            var y = ll & 0xFFF;
            var z = (ll >> 12) & 0x3FFFFFF;
            Location = new Vector3((int)x, (int)y, (int)z);

            BlockId = minecraftStream.ReadVarInt();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
