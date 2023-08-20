﻿using Microsoft.Win32;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace HexBOT.HexedServer
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

        public static bool ValidateServerCertificate(HttpRequestMessage request, X509Certificate2 certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return certificate.Thumbprint == "F0E7EEA966225C228595BC56883EAD7C3E387EF8";
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

        public static string EncryptAuthKey(string Key, string Timestamp, string HWID)
        {
            string EncryptedKey = "";
            EncryptedKey += ToBase64(Timestamp);
            EncryptedKey += ":";
            EncryptedKey += ToBase64(HWID);
            EncryptedKey += ":";
            EncryptedKey += ToBase64(Key);

            return ToBase64(ToBase64(EncryptedKey) + RandomString(5));
        }

        // HWID
        public static string GetHWID()
        {
            string HWID = Environment.MachineName;
            HWID += GetProcessorID();
            HWID += GetProcessorName();
            HWID += GetProcessorVendor();
            HWID += GetBIOSManufacturer();
            HWID += GetBIOSProduct();
            HWID += GetDriveName();
            HWID += GetDriveID();
            HWID += GetDriveID();
            HWID += GetHWIDProfile();
            HWID = GenerateHash(ToBase64(HWID));
            return "H" + HWID + "EX";
        }

        public static string GenerateHash(string Text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Text);
            byte[] hash = SHA256.HashData(bytes);
            string ComputeHash = string.Join("", from it in hash select it.ToString("x2"));
            return ComputeHash;
        }

        private static string GetProcessorID()
        {
            using RegistryKey key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0");
            if (key != null) return key.GetValue("Identifier")?.ToString();

            return "";
        }

        private static string GetProcessorVendor()
        {
            using RegistryKey key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0");
            if (key != null) return key.GetValue("VendorIdentifier")?.ToString();

            return "";
        }

        private static string GetProcessorName()
        {
            using RegistryKey key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0");
            if (key != null) return key.GetValue("ProcessorNameString")?.ToString();

            return "";
        }

        private static string GetBIOSManufacturer()
        {
            using RegistryKey key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\BIOS");
            if (key != null) return key.GetValue("BaseBoardManufacturer")?.ToString();

            return "";
        }

        private static string GetBIOSProduct()
        {
            using RegistryKey key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\BIOS");
            if (key != null) return key.GetValue("BaseBoardProduct")?.ToString();

            return "";
        }

        private static string GetDriveName()
        {
            using RegistryKey key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DEVICEMAP\Scsi\Scsi Port 0\Scsi Bus 0\Target Id 0\Logical Unit Id 0");
            if (key != null) return key.GetValue("Identifier")?.ToString();

            return "";
        }

        private static string GetDriveID()
        {
            using RegistryKey key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DEVICEMAP\Scsi\Scsi Port 0\Scsi Bus 0\Target Id 0\Logical Unit Id 0");
            if (key != null) return key.GetValue("SerialNumber")?.ToString();

            return "";
        }

        private static string GetHWIDProfile()
        {
            using RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\IDConfigDB\Hardware Profiles\0001");
            if (key != null) return key.GetValue("HwProfileGuid")?.ToString();

            return "";
        }
    }
}
