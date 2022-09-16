using MCHexBOT.Network;
using MCHexBOT.Utils;

namespace MCHexBOT.Pakets.Client.Play
{
    internal class DisconnectPaket : IPaket
    {
        public string Message { get; set; }
        public void Decode(MinecraftStream minecraftStream)
        {
            Message = minecraftStream.ReadString();   
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteString(Message);
        }
    }
}
