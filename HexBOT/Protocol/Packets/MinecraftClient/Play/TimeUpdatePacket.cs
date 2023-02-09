using HexBOT.Network;

namespace HexBOT.Packets.Client.Play
{
    public class TimeUpdatePacket : IPacket
    {
        public long WorldAge { get; set; }
        public long TimeOfDay { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            WorldAge = minecraftStream.ReadLong();
            TimeOfDay = minecraftStream.ReadLong();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
