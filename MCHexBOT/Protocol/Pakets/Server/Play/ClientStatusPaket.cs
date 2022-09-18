using MCHexBOT.Network;

namespace MCHexBOT.Packets.Server.Play
{
    public class ClientStatusPacket : IPacket
    {
        public int ActionID { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            ActionID = minecraftStream.ReadVarInt();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(ActionID);
        }
    }
}
