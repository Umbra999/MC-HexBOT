using HexBOT.Network;

namespace HexBOT.Packets.Client.Play
{
    public class UpdateHealthPacket : IPacket
    {
        public float Health { get; set; }
        public int Food { get; set; }
        public float Saturation { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Health = minecraftStream.ReadFloat();
            Food = minecraftStream.ReadVarInt();
            Saturation = minecraftStream.ReadFloat();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
