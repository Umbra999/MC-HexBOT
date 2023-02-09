using HexBOT.Network;

namespace HexBOT.Packets.Server.Play
{
    public class EntityActionPacket : IPacket
    {
        public int EntityId { get; set; }
        public Action ActionId { get; set; }
        public int HorseJumpBoost { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(EntityId);
            minecraftStream.WriteVarInt((int)ActionId);
            minecraftStream.WriteVarInt(ActionId == Action.JumpWithHorse ? HorseJumpBoost : 0);
        }

        public enum Action
        {
            StartSneaking = 0,
            StopSneaking = 1,
            LeaveBed = 2,
            StartSprinting = 3,
            StopSprinting = 4,
            JumpWithHorse = 5,
            OpenHorseInventory = 6,
        }
    }
}
