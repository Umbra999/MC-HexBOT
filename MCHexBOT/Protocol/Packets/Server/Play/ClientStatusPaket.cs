using MCHexBOT.Network;

namespace MCHexBOT.Packets.Server.Play
{
    public class ClientStatusPacket : IPacket
    {
        public int ActionID { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(ActionID);
        }
    }
}
