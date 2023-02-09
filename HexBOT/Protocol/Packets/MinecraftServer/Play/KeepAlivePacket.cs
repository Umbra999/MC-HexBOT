using HexBOT.Network;

namespace HexBOT.Packets.Server.Play
{
    public class KeepAlivePacket : IPacket
    {
        public int KeepAliveID { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(KeepAliveID);
        }
    }
}
