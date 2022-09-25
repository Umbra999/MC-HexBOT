using MCHexBOT.Network;

namespace MCHexBOT.Packets.Client.Play
{
    internal class EntityMetadataPacket : IPacket
    {
        public int EntityId { get; set; }
        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
            // Metadata here
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
