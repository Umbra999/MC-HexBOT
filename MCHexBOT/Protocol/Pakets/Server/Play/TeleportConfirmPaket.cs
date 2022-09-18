using MCHexBOT.Network;

namespace MCHexBOT.Packets.Server.Play
{
    public class TeleportConfirmPacket : IPacket
    {
        public int TeleportID { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            TeleportID = minecraftStream.ReadVarInt();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(TeleportID);
        }
    }
}
