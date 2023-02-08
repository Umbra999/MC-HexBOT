using HexBOT.Network;
using HexBOT.Protocol;
using HexBOT.Protocol.Utils;
using HexBOT.Utils;

namespace HexBOT.Packets.Client.Play
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

        public enum ChatMessagePosition : byte
        {
            Chat = 0,
            System = 1,
            Hotbar = 2
        }
    }
}
