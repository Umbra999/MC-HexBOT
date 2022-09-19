using MCHexBOT.Network;

namespace MCHexBOT.Packets.Client.Login
{
    public class SetCompressionPacket : IPacket
    {
        public int Threshold { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Threshold = minecraftStream.ReadVarInt();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
