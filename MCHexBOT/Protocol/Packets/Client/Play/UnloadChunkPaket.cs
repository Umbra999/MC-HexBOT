using MCHexBOT.Network;

namespace MCHexBOT.Packets.Client.Play
{
    public class UnloadChunkPacket : IPacket
    {
        public int ChunkX { get; set; }
        public int ChunkZ { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            ChunkX = minecraftStream.ReadInt();
            ChunkZ = minecraftStream.ReadInt();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
