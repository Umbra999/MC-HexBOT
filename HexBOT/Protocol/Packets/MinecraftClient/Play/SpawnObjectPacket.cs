using HexBOT.Network;

namespace HexBOT.Packets.Client.Play
{
    public class SpawnObjectPacket : IPacket
    {
        public int EntityId { get; set; }
        public byte Type { get; set; } // make enum?
        public double XPosition { get; set; }
        public double YPosition { get; set; }
        public double ZPosition { get; set; }
        public float Pitch { get; set; }
        public float Yaw { get; set; }
        public int Data { get; set; }
        public short XVelocity { get; set; }
        public short YVelocity { get; set; }
        public short ZVelocity { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
            Type = (byte)minecraftStream.ReadByte();
            XPosition = Protocol.Utils.Math.ConvertFixedPoint(minecraftStream.ReadInt());
            YPosition = Protocol.Utils.Math.ConvertFixedPoint(minecraftStream.ReadInt());
            ZPosition = Protocol.Utils.Math.ConvertFixedPoint(minecraftStream.ReadInt());
            Yaw = minecraftStream.ReadByte() / (256.0f / 360.0f); // this is a Angle?
            Pitch = minecraftStream.ReadByte() / (256.0f / 360.0f); // maybe put this into handler
            Data = minecraftStream.ReadInt();
            if (Data != 0)
            {
                XVelocity = minecraftStream.ReadShort();
                YVelocity = minecraftStream.ReadShort();
                ZVelocity = minecraftStream.ReadShort();
            }
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
