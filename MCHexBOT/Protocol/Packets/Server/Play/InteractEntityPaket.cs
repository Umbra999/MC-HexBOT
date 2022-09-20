using MCHexBOT.Network;

namespace MCHexBOT.Packets.Server.Play
{
    public class InteractEntityPacket : IPacket
    {
        public int EntityID { get; set; }
        public EntityInteractType InteractType { get; set; }
        public float TargetX { get; set; }
        public float TargetY { get; set; }
        public float TargetZ { get; set; }
        public EntityInteractHandType HandType { get; set; }
        public bool Sneaking { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(EntityID);
            minecraftStream.WriteVarInt((int)InteractType);
            if (InteractType == EntityInteractType.InteractAt)
            {
                minecraftStream.WriteFloat(TargetX);
                minecraftStream.WriteFloat(TargetY);
                minecraftStream.WriteFloat(TargetZ);
                minecraftStream.WriteVarInt((int)HandType);
            }
            minecraftStream.WriteBool(Sneaking);
        }

        public enum EntityInteractType
        {
            Interact = 0,
            Attack = 1,
            InteractAt = 2,
        }

        public enum EntityInteractHandType
        {
            Main = 0,
            Second = 1
        }
    }
}
