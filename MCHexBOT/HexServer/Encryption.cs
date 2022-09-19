using Microsoft.Win32;
using System.Security.Cryptography;
using System.Text;

namespace MCHexBOT.HexServer
{
    internal class Encryption
    {
        public static string FromBase64(string Data)
        {
            var base64EncodedBytes = Convert.FromBase64String(Data);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string ToBase64(string Data)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(Data);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static Random Random = new(Environment.TickCount);
        public static string RandomString(int length)
        {
            char[] array = "abcdefghlijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToArray();
            string text = string.Empty;
            for (int i = 0; i < length; i++)
            {
                text += array[Random.Next(array.Length)].ToString();
            }
            return text;
        }

        public static int RandomNumber(int Lowest, int Highest)
        {
            return Random.Next(Lowest, Highest);
        }

        public static byte RandomByte()
        {
            return (byte)Random.Next(0, 255);
        }

        public static string EncryptAuthKey(string Key, string Timestamp, string ValidationType, string HWID)
        {
            string EncryptedKey = ValidationType;
            EncryptedKey += ":";
            EncryptedKey += ToBase64(Timestamp);
            EncryptedKey += ":";
            EncryptedKey += ToBase64(HWID);
            EncryptedKey += ":";
            EncryptedKey += ToBase64(Key);
            EncryptedKey += "98NXvV3d";
            EncryptedKey += ToBase64("10792dC");

            return ToBase64(EncryptedKey);
        }


        // HWID
        public static string GetHWID()
        {
            string HWID = "";
            HWID += GetProductID();
            HWID += "0";
            HWID += Environment.ProcessorCount;
            HWID += ToBase64(Environment.UserName);
            HWID += "o";
            HWID += ToBase64(Environment.MachineName);
            HWID += "XI";
            HWID += GetMachineID();
            HWID += "v3";
            HWID += GetProfileGUID();
            HWID = ToBase64(HWID);
            HWID += "94p";
            HWID = ToBase64(HWID);
            HWID += "xL";
            return GenerateHash(HWID);
        }

        public static string GenerateHash(string Text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Text);
            byte[] hash = SHA256.Create().ComputeHash(bytes);
            string ComputeHash = string.Join("", from it in hash select it.ToString("x2"));
            return ComputeHash;
        }


        private static string GetProductID()
        {
            RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", false);
            string ProductID = registryKey.GetValue("ProductID").ToString();
            registryKey.Close();
            return ProductID;
        }

        private static string GetMachineID()
        {
            RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SOFTWARE\\Microsoft\\SQMClient", false);
            string MachineID = registryKey.GetValue("MachineId").ToString();
            registryKey.Close();
            return MachineID;
        }

        private static string GetProfileGUID()
        {
            RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\IDConfigDB\\Hardware Profiles\\0001", false);
            string ProfileGUID = registryKey.GetValue("HwProfileGUID").ToString();
            registryKey.Close();
            return ProfileGUID;
        }
    }
}
