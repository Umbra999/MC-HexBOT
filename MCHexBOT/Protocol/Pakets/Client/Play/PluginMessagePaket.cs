using MCHexBOT.Network;
using MCHexBOT.Pakets;

namespace MCHexBOT.Protocol.Pakets.Client.Play
{
    internal class PluginMessagePaket : IPaket
    {
        public string Channel { get; set; }
        public byte[] Data { get; set; }
        public void Decode(MinecraftStream minecraftStream)
        {
            Channel = minecraftStream.ReadString();
            Data = minecraftStream.ar
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            
        }
    }
}
