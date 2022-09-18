using MCHexBOT.Packets.Client.Play;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections;
using System.Numerics;
using System.Text;

namespace MCHexBOT.Utils
{
    public static class Misc
    {
        public static string RandomString(int length)
        {
            char[] array = "abcdefghlijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToArray();
            string text = string.Empty;
            for (int i = 0; i < length; i++)
            {
                text += array[new Random(Environment.TickCount).Next(array.Length)].ToString();
            }
            return text;
        }

        public static string RandomNumberString(int length)
        {
            char[] array = "0123456789".ToArray();
            string text = string.Empty;
            for (int i = 0; i < length; i++)
            {
                text += array[new Random(Environment.TickCount).Next(array.Length)].ToString();
            }
            return text;
        }

        public static int RandomNumber(int Lowest, int Highest)
        {
            return new Random(Environment.TickCount).Next(Lowest, Highest);
        }

        public static byte RandomByte()
        {
            return (byte)new Random(Environment.TickCount).Next(0, 255);
        }

        public static string FromBase64(string Data)
        {
            var base64EncodedBytes = Convert.FromBase64String(Data);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string ToBase64(string Data)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(Data));
        }

        public static double DistanceSquared(Vector3 From, Vector3 To)
        {
            var diff = new Vector3(From.X - To.X, From.Y - To.Y, From.Z - To.Z);
            return diff.X * diff.X + diff.Y * diff.Y + diff.Z * diff.Z;
        }

        public static Dictionary<string, int> ProtocolVersions = new()
        {
            { "1.7", 4 },
            { "1.8", 47 },
            { "1.9", 110 },
            { "1.10", 210 },
            { "1.11", 316 },
            { "1.12", 340 },
            { "1.13", 404 },
            { "1.14", 498 },
            { "1.15", 578 },
            { "1.16", 754 },
            { "1.17", 756 },
            { "1.18", 757 },
            { "1.19", 760 },
        };
    }
}
