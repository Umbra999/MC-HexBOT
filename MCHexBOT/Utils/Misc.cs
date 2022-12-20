namespace MCHexBOT.Utils
{
    public static class Misc
    {
        public static byte[] GetVarInt(int paramInt)
        {
            List<byte> bytes = new();
            while ((paramInt & -128) != 0)
            {
                bytes.Add((byte)(paramInt & 127 | 128));
                paramInt = (int)(((uint)paramInt) >> 7);
            }
            bytes.Add((byte)paramInt);
            return bytes.ToArray();
        }

        public static byte[] ConcatBytes(params byte[][] bytes)
        {
            List<byte> result = new();
            foreach (byte[] array in bytes)
            {
                result.AddRange(array);
            }
            return result.ToArray();
        }

        public class DiscordPayload
        {
            public string EncryptedURL { get; set; }
            public bool Public { get; set; }
            public string Title { get; set; }
            public int Color { get; set; }
            public DiscordEmbedField[] Fields { get; set; }
        }

        public class DiscordEmbedField
        {
            public string name { get; set; }
            public string value { get; set; }
        }
    }
}
