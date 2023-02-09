using HexBOT.Network;

namespace HexBOT.Packets.Client.Play
{
    public class CollectItemPacket : IPacket
    {
        public int CollectedEntityID { get; set; }
        public int CollectorEntityID { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            CollectedEntityID = minecraftStream.ReadInt();
            CollectorEntityID = minecraftStream.ReadInt();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
