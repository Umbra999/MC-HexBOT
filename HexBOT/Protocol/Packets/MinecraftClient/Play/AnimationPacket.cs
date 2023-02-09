using HexBOT.Network;

namespace HexBOT.Packets.Client.Play
{
    internal class AnimationPacket : IPacket
    {
        public int EntityId { get; set; }
        public AnimationType AnimationId { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
            AnimationId = (AnimationType)minecraftStream.ReadUnsignedByte();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public enum AnimationType : byte
        {
            SwingArm = 0,
            TakeDamage = 1,
            LeaveBed = 2,
            EatFood = 3,
            CriticalDamage = 4,
            MagicCriticalEffect = 5
        }
    }
}
