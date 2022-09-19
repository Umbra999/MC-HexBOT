using MCHexBOT.Network;
using MCHexBOT.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCHexBOT.Protocol.Packets.LabyServer.Login
{
    public class EncryptionResponsePacket : IPacket
    {
        public int SharedKeyLenght { get; set; }
        public byte[] SharedKey { get; set; }
        public int VerifyTokenLenght { get; set; }
        public byte[] VerifyToken { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            SharedKeyLenght = minecraftStream.ReadInt();
            SharedKey = minecraftStream.Read(SharedKeyLenght);
            VerifyTokenLenght = minecraftStream.ReadInt();
            VerifyToken = minecraftStream.Read(VerifyTokenLenght);
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteInt(SharedKeyLenght);
            minecraftStream.Write(SharedKey);
            minecraftStream.WriteInt(VerifyTokenLenght);
            minecraftStream.Write(VerifyToken);
        }
    }
}
