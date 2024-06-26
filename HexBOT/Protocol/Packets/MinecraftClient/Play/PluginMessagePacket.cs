﻿using HexBOT.Network;

namespace HexBOT.Packets.Client.Play
{
    public class PluginMessagePacket : IPacket
    {
        public string Channel { get; set; }
        public byte[] Data { get; set; }
        public void Decode(MinecraftStream minecraftStream)
        {
            Channel = minecraftStream.ReadString();
            Data = minecraftStream.Read((int)(minecraftStream.Length - minecraftStream.Position));
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
