using HexBOT.Network;

namespace HexBOT.Packets.Server.Status
{
    public class StatusRequestPacket : IPacket
    {
        public void Decode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            // No Data
        }
    }
}
