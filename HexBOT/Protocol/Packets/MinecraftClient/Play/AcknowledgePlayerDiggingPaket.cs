using HexBOT.Network;
using System.Numerics;

namespace HexBOT.Packets.Client.Play
{
    public class AcknowledgePlayerDiggingPacket : IPacket
    {
        public Vector3 Location { get; set; }
        public int BlockID { get; set; }
        public PlayerDiggingType Status { get; set; }
        public bool Successful { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Location = minecraftStream.ReadPosition();
            BlockID = minecraftStream.ReadVarInt();
            Status = (PlayerDiggingType)minecraftStream.ReadVarInt();
            Successful = minecraftStream.ReadBool();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public enum PlayerDiggingType
        {
            Start = 0,
            Cancel = 1,
            Finish = 2
        }
    }
}
