using MCHexBOT.Network;

namespace MCHexBOT.Packets.Client.Play
{
    internal class PluginMessagePacket : IPacket
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
            throw new NotImplementedException();
        }
    }
}
