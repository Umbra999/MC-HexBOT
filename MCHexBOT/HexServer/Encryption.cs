using Microsoft.Win32;
using System.Management;
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
            HWID += Environment.MachineName;
            HWID += GetMacID();
            HWID += GetBiosID();
            HWID += GetDriveID();
            HWID += GetBoardID();
            HWID += GetProcessorID();
            HWID = GenerateHash(ToBase64(HWID));
            return "H" + HWID + "EX";
        }

        public static string GenerateHash(string Text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Text);
            byte[] hash = SHA256.Create().ComputeHash(bytes);
            string ComputeHash = string.Join("", from it in hash select it.ToString("x2"));
            return ComputeHash;
        }


        private static string GetIdentifier(string wmiClass, string wmiProperty, string wmiMustBeTrue = "")
        {
            string result = "";
            ManagementClass mc = new(wmiClass);
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (wmiMustBeTrue != "")
                {
                    if (mo[wmiMustBeTrue].ToString() != "True") continue;
                }

                if (result == "")
                {
                    try
                    {
                        result = mo[wmiProperty].ToString();
                        break;
                    }
                    catch { }
                }
            }
            return result;
        }

        private static string GetMacID()
        {
            return GetIdentifier("Win32_NetworkAdapterConfiguration", "MACAddress", "IPEnabled");
        }

        private static string GetBiosID()
        {
            return GetIdentifier("Win32_BIOS", "Manufacturer") + GetIdentifier("Win32_BIOS", "SerialNumber") + GetIdentifier("Win32_BIOS", "ReleaseDate");
        }

        private static string GetDriveID()
        {
            return GetIdentifier("Win32_DiskDrive", "Model");
        }

        private static string GetBoardID()
        {
            return GetIdentifier("Win32_BaseBoard", "Manufacturer") + GetIdentifier("Win32_BaseBoard", "SerialNumber");
        }

        private static string GetProcessorID()
        {
            return GetIdentifier("Win32_Processor", "ProcessorId") + GetIdentifier("Win32_Processor", "Name") + GetIdentifier("Win32_Processor", "Manufacturer");
        }
    }
}
