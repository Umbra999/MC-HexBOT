using MCHexBOT.Network;

namespace MCHexBOT.Packets.Client.Play
{
    internal class PingPacket : IPacket
    {
        public int ID { get; set; }
        public void Decode(MinecraftStream minecraftStream)
        {
            ID = minecraftStream.ReadInt();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
