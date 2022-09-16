using MCHexBOT.Network;
using MCHexBOT.Protocol;
using MCHexBOT.Utils;
using MCHexBOT.Utils.Data;

namespace MCHexBOT.Pakets.Client.Play
{
    public class ChatMessagePaket : IPaket
    {
        public string JsonData { get; set; }
        public ChatMessagePosition Position { get; set; }
        public UUID Sender { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            JsonData = minecraftStream.ReadString();
            Position = (ChatMessagePosition)minecraftStream.ReadByte();
            Sender = minecraftStream.ReadUuid();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteString(JsonData);
            minecraftStream.WriteByte((byte)Position);
            minecraftStream.WriteUuid(Sender);
        }
    }
}
