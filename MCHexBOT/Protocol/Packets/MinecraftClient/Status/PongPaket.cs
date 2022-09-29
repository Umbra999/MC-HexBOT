using MCHexBOT.Network;

namespace MCHexBOT.Packets.Client.Status
{
    public class PongPacket : IPacket
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
