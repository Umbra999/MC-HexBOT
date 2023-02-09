using HexBOT.Network;

namespace HexBOT.Packets.Client.Play
{
    public class SpawnPlayerPacket : IPacket
    {
        public int EntityId { get; set; }
        public string UUID { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public short CurrentItem { get; set; }
        //public Metadata MetaData { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
            UUID = minecraftStream.ReadUuid();
            X = Protocol.Utils.Math.ConvertFixedPoint(minecraftStream.ReadInt());
            Y = Protocol.Utils.Math.ConvertFixedPoint(minecraftStream.ReadInt());
            Z = Protocol.Utils.Math.ConvertFixedPoint(minecraftStream.ReadInt());
            Yaw = minecraftStream.ReadByte() / (256.0f / 360.0f); // this is a Angle?
            Pitch = minecraftStream.ReadByte() / (256.0f / 360.0f); // maybe put this into handler
            CurrentItem = minecraftStream.ReadShort();
            //MetaData = minecraftStream.ReadEntityData();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
