using MCHexBOT.Network;

namespace MCHexBOT.Packets.Client.Play
{
    public class UpdateViewPositionPacket : IPacket
    {
        public int ChunkX { get; set; }
        public int ChunkZ { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            ChunkX = minecraftStream.ReadVarInt();
            ChunkZ = minecraftStream.ReadVarInt();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
