using HexBOT.Network;
using HexBOT.Packets;

namespace HexBOT.Protocol.Packets.LabyServer.Login
{
    internal class LoginOptionPacket : IPacket
    {
        public bool ShowServer { get; set; }
        public byte Status { get; set; }
        public string TimeZone { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            ShowServer = minecraftStream.ReadBool();
            Status = (byte)minecraftStream.ReadByte();
            TimeZone = minecraftStream.ReadString();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteBool(ShowServer);
            minecraftStream.WriteByte(Status);
            minecraftStream.WriteString(TimeZone);
        }
    }
}
