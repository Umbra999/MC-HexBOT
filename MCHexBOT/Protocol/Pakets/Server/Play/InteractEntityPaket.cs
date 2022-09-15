using MCHexBOT.Network;
using MCHexBOT.Protocol;

namespace MCHexBOT.Pakets.Server.Play
{
    internal class InteractEntityPaket : IPaket
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
            EntityID = minecraftStream.ReadVarInt();
            InteractType = (EntityInteractType)minecraftStream.ReadVarInt();
            TargetX = minecraftStream.ReadFloat();
            TargetY = minecraftStream.ReadFloat();
            TargetZ = minecraftStream.ReadFloat();
            HandType = (EntityInteractHandType)minecraftStream.ReadVarInt();
            Sneaking = minecraftStream.ReadBool();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(EntityID);
            minecraftStream.WriteVarInt((int)InteractType);
            minecraftStream.WriteFloat(TargetX);
            minecraftStream.WriteFloat(TargetY);
            minecraftStream.WriteFloat(TargetZ);
            minecraftStream.WriteVarInt((int)HandType);
            minecraftStream.WriteBool(Sneaking);
        }
    }
}
