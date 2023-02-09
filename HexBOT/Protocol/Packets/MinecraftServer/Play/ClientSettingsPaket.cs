using HexBOT.Network;

namespace HexBOT.Packets.Server.Play
{
    public class ClientSettingsPacket : IPacket
    {
        public string Locale { get; set; }
        public byte ViewDistance { get; set; }
        public ChatType ChatMode { get; set; }
        public bool ChatColors { get; set; }
        public uint DisplayedSkinParts { get; set; }

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
            minecraftStream.WriteByte((byte)DisplayedSkinParts); // rework to have a easier bit toggler

            //          Bit 0(0x01): Cape enabled
            //          Bit 1(0x02): Jacket enabled
            //          Bit 2(0x04): Left Sleeve enabled
            //          Bit 3(0x08): Right Sleeve enabled
            //          Bit 4(0x10): Left Pants Leg enabled
            //          Bit 5(0x20): Right Pants Leg enabled
            //          Bit 6(0x40): Hat enabled
            //          The most significant bit (bit 7, 0x80) appears to be unused.
        }

        public enum ChatType
        {
            Enabled = 0,
            CommandsOnly = 1,
            Hidden = 2
        }
    }
}
