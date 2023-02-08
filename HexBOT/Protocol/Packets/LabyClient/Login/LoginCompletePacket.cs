using HexBOT.Network;
using HexBOT.Packets;
using HexBOT.Utils;
using Newtonsoft.Json;

namespace HexBOT.Protocol.Packets.LabyClient.Login
{
    internal class LoginCompletePacket : IPacket
    {
        public LabyPin DashboardPin { get; set; }
        public void Decode(MinecraftStream minecraftStream)
        {
            DashboardPin = JsonConvert.DeserializeObject<LabyPin>(minecraftStream.ReadString());
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
