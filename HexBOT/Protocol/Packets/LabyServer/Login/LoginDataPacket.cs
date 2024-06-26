﻿using HexBOT.Network;
using HexBOT.Packets;

namespace HexBOT.Protocol.Packets.LabyServer.Login
{
    internal class LoginDataPacket : IPacket
    {
        public string id { get; set; }

        public string name { get; set; }

        public string motd { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            id = minecraftStream.ReadUuid();
            name = minecraftStream.ReadString();
            motd = minecraftStream.ReadString();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteString(id.ToString());
            minecraftStream.WriteString(name);
            minecraftStream.WriteString(motd);
        }
    }
}
