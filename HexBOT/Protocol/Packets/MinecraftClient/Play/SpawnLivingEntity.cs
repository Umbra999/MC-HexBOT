using HexBOT.Network;
using HexBOT.Protocol.Utils;

namespace HexBOT.Packets.Client.Play
{
    public class SpawnLivingEntity : IPacket
    {
        public int EntityId { get; set; }
        public UUID EntityUUID { get; set; }
        public int Type { get; set; }
        public double XPosition { get; set; }
        public double YPosition { get; set; }
        public double ZPosition { get; set; }
        public byte Pitch { get; set; }
        public byte Yaw { get; set; }
        public byte HeadPitch { get; set; }
        public short XVelocity { get; set; }
        public short YVelocity { get; set; }
        public short ZVelocity { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
            EntityUUID = minecraftStream.ReadUuid();
            Type = minecraftStream.ReadVarInt();
            XPosition = minecraftStream.ReadDouble();
            YPosition = minecraftStream.ReadDouble();
            ZPosition = minecraftStream.ReadDouble();
            Yaw = (byte)minecraftStream.ReadByte();
            Pitch = (byte)minecraftStream.ReadByte();
            HeadPitch = (byte)minecraftStream.ReadByte();
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
