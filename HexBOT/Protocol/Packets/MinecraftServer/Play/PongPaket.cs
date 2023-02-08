using HexBOT.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexBOT.Packets.Server.Play
{
    internal class PongPacket : IPacket
    {
        public int ID { get; set; }
        public void Decode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteInt(ID);
        }
    }
}
