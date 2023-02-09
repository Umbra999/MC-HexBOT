using HexBOT.Network;

namespace HexBOT.Packets.Client.Play
{
    public class JoinGamePacket : IPacket
    {
        public int EntityId { get; set; }
        public GamemodeType Gamemode { get; set; }
        public DimensionType Dimension { get; set; }
        public DifficultyType Difficulty { get; set; }
        public byte MaxPlayers { get; set; }
        public string LevelType { get; set; }
        public bool ReducedDebugInfo { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadInt();
            Gamemode = (GamemodeType)minecraftStream.ReadUnsignedByte();
            Dimension = (DimensionType)minecraftStream.ReadByte();
            Difficulty = (DifficultyType)minecraftStream.ReadUnsignedByte();
            MaxPlayers = minecraftStream.ReadUnsignedByte();
            LevelType = minecraftStream.ReadString();
            ReducedDebugInfo = minecraftStream.ReadBool();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public enum GamemodeType : byte // Bit 3 (0x8) is the hardcore flag.
        {
            Survival = 0,
            Creative = 1,
            Adventure = 2,
            Spectator = 3
        }

        public enum DimensionType : sbyte
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
    }
}
