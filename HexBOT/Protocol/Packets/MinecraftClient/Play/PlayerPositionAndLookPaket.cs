using HexBOT.Network;

namespace HexBOT.Packets.Client.Play
{
    public class PlayerPositionAndLookPacket : IPacket
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
            throw new NotImplementedException();
        }
    }
}
