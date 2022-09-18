using MCHexBOT.Network;

namespace MCHexBOT.Packets.Client.Status
{
    public class StatusResponsePacket : IPacket
    {
        public string Status { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Status = minecraftStream.ReadString();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteString(Status);
        }
    }
}
