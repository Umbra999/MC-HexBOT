namespace HexBOT.Protocol.Utils
{
    public class UUID
    {
        public static string DecodeUUID(ulong high, ulong low)
        {
            byte[] bytes = new byte[0].Concat(BitConverter.GetBytes(low)).Concat(BitConverter.GetBytes(high)).ToArray();

            string hex = string.Join("", bytes.Select(b => b.ToString("x2")));

            return hex.Substring(0, 8) + "-" + hex.Substring(8, 4) + "-" + hex.Substring(12, 4) + "-" + hex.Substring(16, 4) + "-" + hex.Substring(20, 12);
        }

        public static void EncodeUUID(string uuid, out ulong high, out ulong low)
        {
            uuid = uuid.Replace("-", "");
            byte[] bytes = StringToByteArray(uuid);
            low = BitConverter.ToUInt64(bytes.Skip(0).Take(8).ToArray(), 0);
            high = BitConverter.ToUInt64(bytes.Skip(8).Take(8).ToArray(), 0);
        }

        public static string AddDashesToUUID(string uuidString)
        {
            return uuidString.Substring(0, 8) + "-" + uuidString.Substring(8, 4) + "-" + uuidString.Substring(12, 4) + "-" + uuidString.Substring(16, 4) + "-" + uuidString.Substring(20, 12);
        }

        public static string RemoveDashesFromUUID(string uuidString)
        {
            return uuidString.Replace("-", "");
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
        }
    }
}
