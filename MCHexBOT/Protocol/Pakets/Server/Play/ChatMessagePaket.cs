﻿using MCHexBOT.Network;

namespace MCHexBOT.Packets.Server.Play
{
    public class ChatMessagePacket : IPacket
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
