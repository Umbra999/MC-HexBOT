using MCHexBOT.Network;
using MCHexBOT.Protocol.Utils;

namespace MCHexBOT.Packets.Client.Login
{
    public class LoginSuccessPacket : IPacket
    {
        public UUID Uuid { get; set; }
        public string Username { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Uuid = minecraftStream.ReadUuid();
            Username = minecraftStream.ReadString();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
