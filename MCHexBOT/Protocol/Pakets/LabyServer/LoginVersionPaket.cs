using MCHexBOT.Network;
using MCHexBOT.Packets;


namespace MCHexBOT.Protocol.Packets.LabyServer
{
    internal class LoginVersionPacket : IPacket
    {
        public int Version { get; set; }
        public string Name { get; set; }
        public string UpdateUrl { get; set; }
        public void Decode(MinecraftStream minecraftStream)
        {
            Version = minecraftStream.ReadInt();
            Name = minecraftStream.ReadString();
            UpdateUrl = minecraftStream.ReadString();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteInt(Version);
            minecraftStream.WriteString(Name);
            minecraftStream.WriteString(UpdateUrl);
        }
    }
}
