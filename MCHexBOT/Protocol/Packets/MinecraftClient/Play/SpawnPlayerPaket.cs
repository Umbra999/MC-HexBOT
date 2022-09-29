using MCHexBOT.Network;
using MCHexBOT.Protocol.Utils;

namespace MCHexBOT.Packets.Client.Play
{
    public class SpawnPlayerPacket : IPacket
    {
        public int EntityId { get; set; }
        public UUID UUID { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public byte Yaw { get; set; }
        public byte Pitch { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
            UUID = minecraftStream.ReadUuid();
            X = minecraftStream.ReadDouble();
            Y = minecraftStream.ReadDouble();
            Z = minecraftStream.ReadDouble();
            Yaw = (byte)minecraftStream.ReadByte();
            Pitch = (byte)minecraftStream.ReadByte();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
