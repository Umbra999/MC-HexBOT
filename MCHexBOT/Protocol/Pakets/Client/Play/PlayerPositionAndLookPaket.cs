using MCHexBOT.Network;

namespace MCHexBOT.Pakets.Client.Play
{
    public class PlayerPositionAndLookPaket : IPaket
    {
        public double X { get; set; }
        public bool XAbsolute { get; set; }
        public double Y { get; set; }
        public bool YAbsolute { get; set; }
        public double Z { get; set; }
        public bool ZAbsolute { get; set; }
        public float Yaw { get; set; }
        public bool YawAbsolute { get; set; }
        public float Pitch { get; set; }
        public bool PitchAbsolute { get; set; }
        public int Flags { get; set; }
        public int TeleportID { get; set; }
        public bool DismountVehicle { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            X = minecraftStream.ReadDouble();
            Y = minecraftStream.ReadDouble();
            Z = minecraftStream.ReadDouble();
            Yaw = minecraftStream.ReadFloat();
            Pitch = minecraftStream.ReadFloat();
            Flags = minecraftStream.ReadByte();
            TeleportID = minecraftStream.ReadVarInt();
            DismountVehicle = minecraftStream.ReadBool();
            XAbsolute = (Flags & 1) == 0;
            YAbsolute = (Flags & 2) == 0;
            ZAbsolute = (Flags & 4) == 0;
            PitchAbsolute = (Flags & 8) == 0;
            YawAbsolute = (Flags & 16) == 0;
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteDouble(X);
            minecraftStream.WriteDouble(Y);
            minecraftStream.WriteDouble(Z);
            minecraftStream.WriteFloat(Yaw);
            minecraftStream.WriteFloat(Pitch);
            minecraftStream.WriteByte((byte)Flags);
            minecraftStream.WriteVarInt(TeleportID);
            minecraftStream.WriteBool(DismountVehicle);
        }
    }
}
