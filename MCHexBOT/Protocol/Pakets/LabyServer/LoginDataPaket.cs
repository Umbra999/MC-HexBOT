using MCHexBOT.Network;
using MCHexBOT.Packets;
using MCHexBOT.Utils.Data;

namespace MCHexBOT.Protocol.Packets.LabyServer
{
    internal class LoginDataPacket : IPacket
    {
        public UUID id { get; set; }

        public string name { get; set; }

        public string motd { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            id = minecraftStream.ReadUuid();
            name = minecraftStream.ReadString();
            motd = minecraftStream.ReadString();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteUuid(id);
            minecraftStream.WriteString(name);
            minecraftStream.WriteString(motd);
        }
    }
}
