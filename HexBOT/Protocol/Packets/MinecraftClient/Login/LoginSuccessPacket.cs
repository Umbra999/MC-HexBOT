using HexBOT.Network;

namespace HexBOT.Packets.Client.Login
{
    public class LoginSuccessPacket : IPacket
    {
        public string Uuid { get; set; }
        public string Username { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Uuid = minecraftStream.ReadString();
            Username = minecraftStream.ReadString();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
