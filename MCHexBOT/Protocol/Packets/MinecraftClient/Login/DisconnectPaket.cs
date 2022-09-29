using MCHexBOT.Network;

namespace MCHexBOT.Packets.Client.Login
{
    public class DisconnectPacket : IPacket
    {
        public string Message { get; set; } // Chat Object

        public void Decode(MinecraftStream minecraftStream)
        {
            Message = minecraftStream.ReadString();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
