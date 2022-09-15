using MCHexBOT.Network;
using MCHexBOT.Protocol;

namespace MCHexBOT.Pakets.Server.Handshake
{
    public class HandshakePaket : IPaket
    {
        public int ProtocolVersion { get; set; }
        public string ServerAddress { get; set; }
        public ushort ServerPort { get; set; }
        public HandshakeType NextState { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            ProtocolVersion = minecraftStream.ReadVarInt();
            ServerAddress = minecraftStream.ReadString();
            ServerPort = minecraftStream.ReadUShort();
            NextState = (HandshakeType)minecraftStream.ReadVarInt();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(ProtocolVersion);
            minecraftStream.WriteString(ServerAddress);
            minecraftStream.WriteUShort(ServerPort);
            minecraftStream.WriteVarInt((int)NextState);
        }
    }
}
