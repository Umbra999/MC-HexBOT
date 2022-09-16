using MCHexBOT.Network;

namespace MCHexBOT.Pakets.Client.Play
{
    internal class PingPaket : IPaket
    {
        public int ID { get; set; }
        public void Decode(MinecraftStream minecraftStream)
        {
            ID = minecraftStream.ReadInt();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteInt(ID);
        }
    }
}
