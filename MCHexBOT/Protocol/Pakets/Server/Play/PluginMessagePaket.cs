using MCHexBOT.Network;

namespace MCHexBOT.Pakets.Server.Play
{
    internal class PluginMessagePaket : IPaket
    {
        public string Channel { get; set; }
        public byte[] Data { get; set; }
        public void Decode(MinecraftStream minecraftStream)
        {
            Channel = minecraftStream.ReadString();
            Data = minecraftStream.Read((int)(minecraftStream.Length - minecraftStream.Position));
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteString(Channel);
            minecraftStream.Write(Data);
        }
    }
}
