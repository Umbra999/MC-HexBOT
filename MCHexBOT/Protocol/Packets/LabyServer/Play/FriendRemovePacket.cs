using MCHexBOT.Network;
using MCHexBOT.Packets;

namespace MCHexBOT.Protocol.Packets.LabyServer.Play
{
    internal class FriendRemovePacket : IPacket
    {
        public string Name { get; set; }
        public void Decode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteString(Name);
        }
    }
}
