using MCHexBOT.Network;
using MCHexBOT.Protocol;

namespace MCHexBOT.Pakets.Server.Play
{
    public class ClientSettingsPaket : IPaket
    {
        public string Locale { get; set; }
        public int ViewDistance { get; set; }
        public ChatMode ChatMode { get; set; }
        public bool ChatColors { get; set; }
        public uint DisplayedSkinParts { get; set; }
        public MainHandType MainHand { get; set; }
        public bool EnableTextFiltering { get; set; }
        public bool AllowServerListings { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Locale = minecraftStream.ReadString();
            ViewDistance = minecraftStream.ReadByte();
            ChatMode = (ChatMode)minecraftStream.ReadVarInt();
            ChatColors = minecraftStream.ReadBool();
            DisplayedSkinParts = minecraftStream.ReadUnsignedByte();
            MainHand = (MainHandType)minecraftStream.ReadVarInt();
            EnableTextFiltering = minecraftStream.ReadBool();
            AllowServerListings = minecraftStream.ReadBool();

            byte flags = 1 | 2 | 4;
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteString(Locale);
            minecraftStream.WriteByte((byte)ViewDistance);
            minecraftStream.WriteVarInt((int)ChatMode);
            minecraftStream.WriteBool(ChatColors);
            minecraftStream.WriteByte((byte)DisplayedSkinParts);
            minecraftStream.WriteVarInt((int)MainHand);
            minecraftStream.WriteBool(EnableTextFiltering);
            minecraftStream.WriteBool(AllowServerListings);
        }
    }
}
