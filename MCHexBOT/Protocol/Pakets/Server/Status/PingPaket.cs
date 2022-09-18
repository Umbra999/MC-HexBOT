using MCHexBOT.Network;

namespace MCHexBOT.Packets.Server.Status
{
    public class PingPacket : IPacket
    {
        public long Payload{ get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Payload = minecraftStream.ReadLong();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteLong(Payload);
        }
    }
}
