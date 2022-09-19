using MCHexBOT.Network;
using MCHexBOT.Packets;
using MCHexBOT.Utils;
using Newtonsoft.Json;

namespace MCHexBOT.Protocol.Packets.LabyClient.Login
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
