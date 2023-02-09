using HexBOT.Network;

namespace HexBOT.Packets.Client.Play
{
    public class KeepAlivePacket : IPacket
    {
        public int KeepAliveID { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            KeepAliveID = minecraftStream.ReadVarInt();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
