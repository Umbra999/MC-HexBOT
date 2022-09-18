﻿using MCHexBOT.Network;

namespace MCHexBOT.Packets.Server.Login
{
    public class LoginStartPacket : IPacket
    {
        public string Username { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Username = minecraftStream.ReadString();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteString(Username);
        }
    }
}
