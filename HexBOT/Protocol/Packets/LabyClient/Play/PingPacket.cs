using HexBOT.Network;
using HexBOT.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexBOT.Protocol.Packets.LabyClient.Play
{
    internal class PingPacket : IPacket
    {
        public void Decode(MinecraftStream minecraftStream)
        {

        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
