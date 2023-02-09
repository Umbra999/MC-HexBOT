using HexBOT.Network;

namespace HexBOT.Packets.Client.Play
{
    public class RespawnPacket : IPacket
    {
        public DimensionType Dimension { get; set; }
        public DifficultyType Difficulty { get; set; }
        public GamemodeType Gamemode { get; set; }
        public string LevelType { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Dimension = (DimensionType)minecraftStream.ReadInt();
            Difficulty = (DifficultyType)minecraftStream.ReadUnsignedByte();
            Gamemode = (GamemodeType)minecraftStream.ReadUnsignedByte();
            LevelType = minecraftStream.ReadString();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public enum DimensionType : int
        {
            Nether = -1,
            Overworld = 0,
            End = 1,
        }

        public enum DifficultyType : byte
        {
            Peaceful = 0,
            Easy = 1,
            Normal = 2,
            Hard = 3
        }

        public enum GamemodeType : byte
        {
            Survival = 0,
            Creative = 1,
            Adventure = 2,
        }
    }
}
