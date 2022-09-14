﻿using MCHexBOT.Network;

using System;
using System.Collections.Generic;
using System.Text;

namespace MCHexBOT.Pakets.Server.Play
{
    public class KeepAlivePaket : IPaket
    {
        public long Payload { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Payload = minecraftStream.ReadLong();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteLong(Payload);
        }
    }
}
