﻿using HexBOT.Network;
using HexBOT.Protocol;

namespace HexBOT.Packets.Server.Play
{
    public class ClientSettingsPacket : IPacket
    {
        public string Locale { get; set; }
        public byte ViewDistance { get; set; }
        public ChatType ChatMode { get; set; }
        public bool ChatColors { get; set; }
        public uint DisplayedSkinParts { get; set; }
        public MainHandType MainHand { get; set; }
        public bool EnableTextFiltering { get; set; }
        public bool AllowServerListings { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteString(Locale);
            minecraftStream.WriteByte(ViewDistance);
            minecraftStream.WriteVarInt((int)ChatMode);
            minecraftStream.WriteBool(ChatColors);
            minecraftStream.WriteByte((byte)DisplayedSkinParts);
            minecraftStream.WriteVarInt((int)MainHand);
            minecraftStream.WriteBool(EnableTextFiltering);
            minecraftStream.WriteBool(AllowServerListings);
        }

        public enum MainHandType
        {
            Left = 0,
            Right = 1
        }

        public enum ChatType
        {
            Enabled = 0,
            Commands = 1,
            Hidden = 2
        }
    }
}