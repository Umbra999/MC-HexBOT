using HexBOT.Network;
using HexBOT.Protocol.Utils;

namespace HexBOT.Packets.Client.Play
{
    public class SpawnEntityPacket : IPacket
    {
        public int EntityId { get; set; }
        public UUID ObjectUUID { get; set; }
        public int Type { get; set; }
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
            ObjectUUID = minecraftStream.ReadUuid();
            Type = minecraftStream.ReadVarInt();
            XPosition = minecraftStream.ReadDouble();
            YPosition = minecraftStream.ReadDouble();
            ZPosition = minecraftStream.ReadDouble();
            Yaw = minecraftStream.ReadByte() / (256.0f / 360.0f);
            Pitch = minecraftStream.ReadByte() / (256.0f / 360.0f); // maybe put this into handler
            Data = minecraftStream.ReadInt();
            XVelocity = minecraftStream.ReadShort();
            YVelocity = minecraftStream.ReadShort();
            ZVelocity = minecraftStream.ReadShort();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
