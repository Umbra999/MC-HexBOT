﻿using HexBOT.Network;
using HexBOT.Packets;
using HexBOT.Protocol.Utils;

namespace HexBOT.Protocol.Packets.LabyVoiceServer
{
    internal class HandshakePacket : IPacket
    {
        public int ProtocolVersion { get; set; }
        public int PublicKeyLenght { get; set; }
        public byte[] PublicKey { get; set; }
        public string UUID { get; set; }
        public int Auth { get; set; }
        public string Pin { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteInt(ProtocolVersion);
            //minecraftStream.WriteInt(PublicKeyLenght);
            minecraftStream.Write(PublicKey);
            minecraftStream.WriteUuid(UUID);
            minecraftStream.WriteInt(Auth);
            minecraftStream.WriteString(Pin);
        }
    }
}
