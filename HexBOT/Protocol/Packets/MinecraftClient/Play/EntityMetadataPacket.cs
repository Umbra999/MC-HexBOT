using HexBOT.Network;
using HexBOT.Utils;

namespace HexBOT.Packets.Client.Play
{
    internal class EntityMetadataPacket : IPacket
    {
        public int EntityId { get; set; }
        public List<EntityMetadata> Metadata { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
            Metadata = minecraftStream.ReadEntityData();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
