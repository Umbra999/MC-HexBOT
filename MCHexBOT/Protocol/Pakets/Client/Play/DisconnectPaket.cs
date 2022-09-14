using MCHexBOT.Network;

namespace MCHexBOT.Pakets.Client.Play
{
    internal class DisconnectPaket : IPaket
    {
        public string Message { get; set; }
        public void Decode(MinecraftStream minecraftStream)
        {
            Message = minecraftStream.ReadChatObject();   
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            
        }
    }
}
