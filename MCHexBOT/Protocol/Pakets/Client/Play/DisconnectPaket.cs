using MCHexBOT.Network;
using MCHexBOT.Utils;

namespace MCHexBOT.Pakets.Client.Play
{
    internal class DisconnectPaket : IPaket
    {
        public ChatMessage Message { get; set; }
        public void Decode(MinecraftStream minecraftStream)
        {
            Message = minecraftStream.ReadChatObject();   
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteChatObject(Message);
        }
    }
}
