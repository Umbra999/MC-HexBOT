using MCHexBOT.Network;
using MCHexBOT.Protocol;
using MCHexBOT.Utils;
using MCHexBOT.Utils.Data;

namespace MCHexBOT.Packets.Client.Play
{
    public class ChatMessagePacket : IPacket
    {
        public string JsonData { get; set; } // ChatObject
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
            throw new NotImplementedException();
        }
    }
}
