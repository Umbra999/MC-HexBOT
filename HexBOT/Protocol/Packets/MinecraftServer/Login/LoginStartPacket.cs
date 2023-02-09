using HexBOT.Network;

namespace HexBOT.Packets.Server.Login
{
    public class LoginStartPacket : IPacket
    {
        public string Username { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteString(Username);
        }
    }
}
