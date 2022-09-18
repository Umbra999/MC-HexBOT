using MCHexBOT.Network;

namespace MCHexBOT.Packets.Server.Play
{
    public class EntityActionPacket : IPacket
    {
        public int EntityId { get; set; }
        public int ActionId { get; set; }
        public int JumpBoost { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
            ActionId = minecraftStream.ReadVarInt();
            JumpBoost = minecraftStream.ReadVarInt();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(EntityId);
            minecraftStream.WriteVarInt(ActionId);
            minecraftStream.WriteVarInt(JumpBoost);
        }
    }
}
