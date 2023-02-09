using HexBOT.Network;

namespace HexBOT.Packets.Client.Play
{
    internal class DestroyEntitiesPacket : IPacket
    {
        public int Count { get; set; }
        public int[] EntityIDs { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Count = minecraftStream.ReadVarInt();
            EntityIDs = new int[Count];
            for (int i = 0; i < Count; i++) 
            {
                EntityIDs[i] = minecraftStream.ReadVarInt();
            }
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
