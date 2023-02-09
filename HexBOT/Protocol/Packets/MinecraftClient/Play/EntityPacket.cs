using HexBOT.Network;

namespace HexBOT.Packets.Client.Play
{
    internal class EntityPacket : IPacket
    {
        public int EntityID { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityID = minecraftStream.ReadVarInt();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
