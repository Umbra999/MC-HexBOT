using HexBOT.Network;

namespace HexBOT.Packets.Server.Play
{
    public class ClientStatusPacket : IPacket
    {
        public Action ActionID { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt((int)ActionID);
        }

        public enum Action
        {
            Respawn = 0,
            Statistics = 1,
            TakeInventoryAchivement = 2
        }
    }
}
