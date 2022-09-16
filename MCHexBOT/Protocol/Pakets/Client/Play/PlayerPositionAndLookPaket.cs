using MCHexBOT.Network;

namespace MCHexBOT.Pakets.Client.Play
{
    public class PlayerPositionAndLookPaket : IPaket
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public byte Flags { get; set; }
        public int TeleportID { get; set; }
        public bool DismountVehicle { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            X = minecraftStream.ReadDouble();
            Y = minecraftStream.ReadDouble();
            Z = minecraftStream.ReadDouble();
            Yaw = minecraftStream.ReadFloat();
            Pitch = minecraftStream.ReadFloat();
            Flags = (byte)minecraftStream.ReadByte();
            TeleportID = minecraftStream.ReadVarInt();
            DismountVehicle = minecraftStream.ReadBool();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteDouble(X);
            minecraftStream.WriteDouble(Y);
            minecraftStream.WriteDouble(Z);
            minecraftStream.WriteFloat(Yaw);
            minecraftStream.WriteFloat(Pitch);
            minecraftStream.WriteByte(Flags);
            minecraftStream.WriteVarInt(TeleportID);
            minecraftStream.WriteBool(DismountVehicle);
        }
    }
}
