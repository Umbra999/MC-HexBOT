using HexBOT.Network;

namespace HexBOT.Packets.Client.Play
{
    internal class EntityEquipmentPacket : IPacket
    {
        public int EntityId { get; set; }
        public short Slot { get; set; }
        //public Slot Item { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
            Slot = minecraftStream.ReadShort();
            //Item = minecraftStream.ReadSlot();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
