using MCHexBOT.Network;
using MCHexBOT.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCHexBOT.Protocol.Packets.LabyClient.Play
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
