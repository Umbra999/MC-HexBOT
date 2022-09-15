﻿using MCHexBOT.Network;
using MCHexBOT.Utils;

namespace MCHexBOT.Pakets.Client.Login
{
    public class EncryptionRequestPaket : IPaket
    {
        public string ServerId { get; set; }
        public int PublicKeyLenght { get; set; }
        public byte[] PublicKey { get; set; }
        public int VerifyTokenLenght { get; set; }
        public byte[] VerifyToken { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            ServerId = minecraftStream.ReadString();
            PublicKeyLenght = minecraftStream.ReadVarInt();
            PublicKey = new byte[PublicKeyLenght];
            minecraftStream.Read(PublicKey, PublicKeyLenght);
            VerifyTokenLenght = minecraftStream.ReadVarInt();
            VerifyToken = new byte[VerifyTokenLenght];
            minecraftStream.Read(VerifyToken, VerifyTokenLenght);
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteString(ServerId);
            minecraftStream.WriteVarInt(PublicKey.Length);
            minecraftStream.Write(PublicKey);
            minecraftStream.WriteVarInt(VerifyToken.Length);
            minecraftStream.Write(VerifyToken);
        }
    }
}