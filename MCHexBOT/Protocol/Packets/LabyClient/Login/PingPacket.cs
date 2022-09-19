using MCHexBOT.Network;
using MCHexBOT.Packets;

namespace MCHexBOT.Protocol.Packets.LabyClient.Login
{
    internal class PingPacket : IPacket
    {
        public void Decode(MinecraftStream minecraftStream)
        {

        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
