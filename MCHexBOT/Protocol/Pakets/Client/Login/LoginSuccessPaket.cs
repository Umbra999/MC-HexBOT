using System;

using MCHexBOT.Network;
using MCHexBOT.Utils.Data;

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
            minecraftStream.WriteUuid(Uuid);
            minecraftStream.WriteString(Username);
        }
    }
}
