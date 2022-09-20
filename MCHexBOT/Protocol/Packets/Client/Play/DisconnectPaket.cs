using MCHexBOT.Network;
using MCHexBOT.Utils;

namespace MCHexBOT.Packets.Client.Play
{
    internal class DisconnectPacket : IPacket
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
