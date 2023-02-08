using HexBOT.Network;

namespace HexBOT.Packets.Server.Play
{
    public class TeleportConfirmPacket : IPacket
    {
        public int TeleportID { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(TeleportID);
        }
    }
}
