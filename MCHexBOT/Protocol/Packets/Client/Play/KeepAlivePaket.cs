using MCHexBOT.Network;

namespace MCHexBOT.Packets.Client.Play
{
    public class KeepAlivePacket : IPacket
    {
        public long Payload { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Payload = minecraftStream.ReadLong();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
