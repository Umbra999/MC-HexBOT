using HexBOT.Network;

namespace HexBOT.Packets.Client.Play
{
    public class ChatMessagePacket : IPacket
    {
        public string JsonData { get; set; } // ChatObject
        public ChatMessagePosition Position { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            JsonData = minecraftStream.ReadString();
            Position = (ChatMessagePosition)minecraftStream.ReadByte();
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
