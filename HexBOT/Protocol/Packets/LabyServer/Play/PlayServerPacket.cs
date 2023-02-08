using HexBOT.Network;
using HexBOT.Packets;

namespace HexBOT.Protocol.Packets.LabyServer.Play
{
    internal class PlayServerPacket : IPacket
    {
        public string IP { get; set; }
        public int Port { get; set; }
        public string Gamemode { get; set; }
        public bool viaServerList { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteString(IP);
            minecraftStream.WriteInt(Port);
            minecraftStream.WriteBool(viaServerList);
            if (Gamemode != null)
            {
                minecraftStream.WriteBool(true);
                minecraftStream.WriteString(Gamemode);
            }
            else minecraftStream.WriteBool(false);
        }
    }
}
