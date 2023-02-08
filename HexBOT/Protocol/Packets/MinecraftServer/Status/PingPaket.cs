using HexBOT.Network;

namespace HexBOT.Packets.Server.Status
{
    public class PingPacket : IPacket
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
