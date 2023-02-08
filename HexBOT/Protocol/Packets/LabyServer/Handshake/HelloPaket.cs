using HexBOT.Network;
using HexBOT.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexBOT.Protocol.Packets.LabyServer.Handshake
{
    internal class HelloPacket : IPacket
    {
        public long TickTime { get; set; }
        public int Type { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            TickTime = minecraftStream.ReadLong();
            Type = minecraftStream.ReadInt();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteLong(TickTime);
            minecraftStream.WriteInt(Type);
        }
    }
}
