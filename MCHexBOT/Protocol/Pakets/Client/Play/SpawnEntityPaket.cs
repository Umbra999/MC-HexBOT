using MCHexBOT.Network;
using MCHexBOT.Utils.Data;

namespace MCHexBOT.Pakets.Client.Play
{
    public class SpawnEntityPaket : IPaket
    {
        public int EntityID { get; set; }
        public UUID ObjectUUID { get; set; }
        public int Type { get; set; }
        public double XPosition { get; set; }
        public double YPosition { get; set; }
        public double ZPosition { get; set; }
        public int Pitch { get; set; }
        public int Yaw { get; set; }
        public int Data { get; set; }
        public short XVelocity { get; set; }
        public short YVelocity { get; set; }
        public short ZVelocity { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityID = minecraftStream.ReadVarInt();
            ObjectUUID = minecraftStream.ReadUuid();
            Type = minecraftStream.ReadVarInt();
            XPosition = minecraftStream.ReadDouble();
            YPosition = minecraftStream.ReadDouble();
            ZPosition = minecraftStream.ReadDouble();
            Yaw = minecraftStream.ReadByte();
            Pitch = minecraftStream.ReadByte();
            Data = minecraftStream.ReadInt();
            XVelocity = minecraftStream.ReadShort();
            YVelocity = minecraftStream.ReadShort();
            ZVelocity = minecraftStream.ReadShort();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(EntityID);
            minecraftStream.WriteUuid(ObjectUUID);
            minecraftStream.WriteVarInt(Type);
            minecraftStream.WriteDouble(XPosition);
            minecraftStream.WriteDouble(YPosition);
            minecraftStream.WriteDouble(ZPosition);
            minecraftStream.WriteByte((byte)Yaw);
            minecraftStream.WriteByte((byte)Pitch);
            minecraftStream.WriteInt(Data);
            minecraftStream.WriteShort(XVelocity);
            minecraftStream.WriteShort(YVelocity);
            minecraftStream.WriteShort(ZVelocity);
        }
    }
}
