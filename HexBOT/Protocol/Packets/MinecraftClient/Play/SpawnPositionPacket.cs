﻿using HexBOT.Network;
using System.Numerics;

namespace HexBOT.Packets.Client.Play
{
    public class SpawnPositionPacket : IPacket
    {
        public Vector3 Location { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Location = minecraftStream.ReadPosition();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
