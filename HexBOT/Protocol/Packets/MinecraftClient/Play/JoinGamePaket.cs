﻿using fNbt;

using HexBOT.Network;

namespace HexBOT.Packets.Client.Play
{
    public class JoinGamePacket : IPacket
    {
        public int EntityId { get; set; }
        public bool IsHardcore { get; set; }
        public GamemodeType Gamemode { get; set; }
        public GamemodeType PreviousGamemode { get; set; }
        public int DimensionCount { get; set; }
        public string[] DimensionNames { get; set; }
        public NbtCompound DimesionCodec { get; set; }
        public NbtCompound Dimesion { get; set; }
        public string WorldName { get; set; }
        public long HashedSeed { get; set; }
        public int MaxPlayers { get; set; }
        public int ViewDistance { get; set; }
        public int SimulationDistance { get; set; }
        public bool ReducedDebugInfo { get; set; }
        public bool EnableRespawnScreen { get; set; }
        public bool IsDebug { get; set; }
        public bool IsFlat { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadInt();
            IsHardcore = minecraftStream.ReadBool();
            Gamemode = (GamemodeType)minecraftStream.ReadUnsignedByte();
            PreviousGamemode = (GamemodeType)minecraftStream.ReadByte();
            DimensionCount = minecraftStream.ReadVarInt();

            List<string> names = new();
            for (int i = 0; i <= DimensionCount; i++)
            {
                names.Add(minecraftStream.ReadString());
            }
            DimensionNames = names.ToArray();

            Dimesion = minecraftStream.ReadNbtCompound();
            DimesionCodec = minecraftStream.ReadNbtCompound();

            WorldName = minecraftStream.ReadString();
            HashedSeed = minecraftStream.ReadLong();
            MaxPlayers = minecraftStream.ReadVarInt();
            ViewDistance = minecraftStream.ReadVarInt();
            SimulationDistance = minecraftStream.ReadVarInt();
            ReducedDebugInfo = minecraftStream.ReadBool();
            EnableRespawnScreen = minecraftStream.ReadBool();
            IsDebug = minecraftStream.ReadBool();
            IsFlat = minecraftStream.ReadBool();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public enum GamemodeType : byte
        {
            Survival = 0,
            Creative = 1,
            Adventure = 2,
            Spectator = 3
        }
    }
}