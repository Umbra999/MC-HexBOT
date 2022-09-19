using MCHexBOT.Network;
using MCHexBOT.Packets;


namespace MCHexBOT.Protocol.Packets.LabyServer.Login
{
    internal class LoginVersionPacket : IPacket
    {
        public int Version { get; set; }
        public string Name { get; set; }
        public string UpdateUrl { get; set; }
        public void Decode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteInt(Version);
            minecraftStream.WriteString(Name);
            minecraftStream.WriteString(UpdateUrl);
        }
    }
}
