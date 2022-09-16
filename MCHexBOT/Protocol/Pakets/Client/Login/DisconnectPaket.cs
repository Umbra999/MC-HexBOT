using MCHexBOT.Network;
using MCHexBOT.Utils;

namespace MCHexBOT.Pakets.Client.Login
{
    public class DisconnectPaket : IPaket
    {
        public string Message { get; set; } // shpuld be chat object

        public void Decode(MinecraftStream minecraftStream)
        {
            Message = minecraftStream.ReadString();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteString(Message);
        }
    }
}
