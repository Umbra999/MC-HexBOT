using MCHexBOT.Network;
using MCHexBOT.Protocol;

namespace MCHexBOT.Packets.Server.Play
{
    public class ClientSettingsPacket : IPacket
    {
        public string Locale { get; set; }
        public byte ViewDistance { get; set; }
        public ChatMode ChatMode { get; set; }
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
    }
}
