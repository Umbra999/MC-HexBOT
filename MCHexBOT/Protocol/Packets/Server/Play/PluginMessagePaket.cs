using MCHexBOT.Network;

namespace MCHexBOT.Packets.Server.Play
{
    internal class PluginMessagePacket : IPacket
    {
        public string Channel { get; set; }
        public byte[] Data { get; set; }
        public void Decode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteString(Channel);
            minecraftStream.Write(Data);
        }
    }
}
