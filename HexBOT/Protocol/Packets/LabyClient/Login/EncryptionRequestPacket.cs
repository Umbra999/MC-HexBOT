using HexBOT.Network;
using HexBOT.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexBOT.Protocol.Packets.LabyClient.Login
{
    internal class EncryptionRequestPacket : IPacket
    {
        public string ServerId { get; set; }
        public int PublicKeyLenght { get; set; }
        public byte[] PublicKey { get; set; }
        public int VerifyTokenLenght { get; set; }
        public byte[] VerifyToken { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            ServerId = minecraftStream.ReadString();
            PublicKeyLenght = minecraftStream.ReadInt();
            PublicKey = new byte[PublicKeyLenght];
            minecraftStream.Read(PublicKey, PublicKeyLenght);
            VerifyTokenLenght = minecraftStream.ReadInt();
            VerifyToken = new byte[VerifyTokenLenght];
            minecraftStream.Read(VerifyToken, VerifyTokenLenght);
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
