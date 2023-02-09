using HexBOT.Network;

namespace HexBOT.Packets.Client.Status
{
    public class StatusResponsePacket : IPacket
    {
        public string Status { get; set; } // Json Object

        public void Decode(MinecraftStream minecraftStream)
        {
            Status = minecraftStream.ReadString();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
