﻿using MCHexBOT.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCHexBOT.Pakets.Server.Play
{
    internal class PongPaket : IPaket
    {
        public int ID { get; set; }
        public void Decode(MinecraftStream minecraftStream)
        {
            ID = minecraftStream.ReadInt();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteInt(ID);
        }
    }
}