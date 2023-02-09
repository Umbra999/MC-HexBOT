namespace HexBOT.Protocol.Utils
{
    public class UUID
    {
        private ulong low;
        private ulong high;

        public static string getUUIDFromCompactUUID(string compactUUID)
        {
            return compactUUID.Substring(0, 8) + "-" + compactUUID.Substring(8, 12) + "-" + compactUUID.Substring(12, 16) + "-" + compactUUID.Substring(16, 20) + "-" + compactUUID.Substring(20, 32);
        }

        public UUID(byte[] rfc4122Bytes)
        {
            low = BitConverter.ToUInt64(rfc4122Bytes.Skip(0).Take(8).ToArray(), 0);
            high = BitConverter.ToUInt64(rfc4122Bytes.Skip(8).Take(8).ToArray(), 0);
        }

        public UUID(string uuidString)
        {
            uuidString = uuidString.Replace("-", "");
            byte[] bytes = StringToByteArray(uuidString);
            low = BitConverter.ToUInt64(bytes.Skip(0).Take(8).ToArray(), 0);
            high = BitConverter.ToUInt64(bytes.Skip(8).Take(8).ToArray(), 0);
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
        }

        public byte[] GetBytes()
        {
            return new byte[0].Concat(BitConverter.GetBytes(low)).Concat(BitConverter.GetBytes(high)).ToArray();
        }

        public override string ToString()
        {
            byte[] bytes = new byte[0].Concat(BitConverter.GetBytes(low)).Concat(BitConverter.GetBytes(high)).ToArray();

            string hex = string.Join("", bytes.Select(b => b.ToString("x2")));

            return hex.Substring(0, 8) + "-" + hex.Substring(8, 4) + "-" + hex.Substring(12, 4) + "-" + hex.Substring(16, 4) + "-" + hex.Substring(20, 12);
        }
    }
}
