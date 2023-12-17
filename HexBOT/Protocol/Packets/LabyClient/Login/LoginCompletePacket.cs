using HexBOT.Network;
using HexBOT.Packets;
using HexBOT.Utils;
using System.Text.Json;

namespace HexBOT.Protocol.Packets.LabyClient.Login
{
    internal class LoginCompletePacket : IPacket
    {
        public LabyPin DashboardPin { get; set; }
        public void Decode(MinecraftStream minecraftStream)
        {
            DashboardPin = JsonSerializer.Deserialize<LabyPin>(minecraftStream.ReadString());
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
