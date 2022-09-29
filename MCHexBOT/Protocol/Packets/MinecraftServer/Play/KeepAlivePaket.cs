using MCHexBOT.Network;

namespace MCHexBOT.Packets.Server.Play
{
    public class KeepAlivePacket : IPacket
    {
        public long Payload { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteLong(Payload);
        }
    }
}
