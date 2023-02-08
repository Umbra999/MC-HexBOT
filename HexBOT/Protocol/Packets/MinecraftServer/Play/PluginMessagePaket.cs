using HexBOT.Network;
using HexBOT.Utils;

namespace HexBOT.Packets.Server.Play
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
            minecraftStream.Write(Misc.ConcatBytes(Misc.GetVarInt(Data.Length), Data));
        }
    }
}
