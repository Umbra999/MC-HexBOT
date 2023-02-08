using HexBOT.Network;
using HexBOT.Packets;

namespace HexBOT.Protocol.Packets.LabyServer.Play
{
    internal class FriendRequestPacket : IPacket
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
