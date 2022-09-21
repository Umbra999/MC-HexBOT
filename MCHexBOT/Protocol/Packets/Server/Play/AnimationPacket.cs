﻿using MCHexBOT.Network;
using MCHexBOT.Packets;

namespace MCHexBOT.Packets.Server.Play
{
    public class AnimationPacket : IPacket
    {
        public HandType Hand { get; set; }
        public void Decode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt((int)Hand);
        }

        public enum HandType
        {
            Main = 0,
            Second = 1
        }
    }
}
