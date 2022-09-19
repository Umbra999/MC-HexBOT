using MCHexBOT.Network;
using MCHexBOT.Utils;

namespace MCHexBOT.Packets.Client.Login
{
    public class DisconnectPacket : IPacket
    {
        public string Message { get; set; } // should be chat object

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
