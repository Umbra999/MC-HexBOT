using HexBOT.Network;

namespace HexBOT.Packets.Server.Play
{
    public class EntityActionPacket : IPacket
    {
        public int EntityId { get; set; }
        public Action ActionId { get; set; }
        public int JumpBoost { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(EntityId);
            minecraftStream.WriteVarInt((int)ActionId);
            minecraftStream.WriteVarInt(JumpBoost);
        }

        public enum Action
        {
            StartSneaking = 0,
            StopSneaking = 1,
            LeaveBed = 2,
            StartSprinting = 3,
            StopSprinting = 4,
            StartHorseJump = 5,
            StopHorseJump = 6,
            OpenHorseInventory = 7,
            StartElytraFlying = 8
        }
    }
}
