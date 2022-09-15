using MCHexBOT.Network;
using System.Numerics;

namespace MCHexBOT.Pakets.Client.Play
{
    public class BlockChangePaket : IPaket
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
            ulong ll = (((ulong)Location.X & 0x3FFFFFF) << 38) | (((ulong)Location.Y & 0x3FFFFFF) << 12) | ((ulong)Location.Z & 0xFFF);

            minecraftStream.WriteULong(ll);
            minecraftStream.WriteVarInt(BlockId);
        }
    }
}
