using MCHexBOT.Network;
using MCHexBOT.Protocol;

namespace MCHexBOT.Packets.Server.Handshake
{
    public class HandshakePacket : IPacket
    {
        public int ProtocolVersion { get; set; }
        public string ServerAddress { get; set; }
        public ushort ServerPort { get; set; }
        public HandshakeType NextState { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
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
