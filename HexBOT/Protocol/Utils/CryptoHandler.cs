﻿using System.Text;
using System.Security.Cryptography;

namespace HexBOT.Protocol.Utils
{
    public static class CryptoHandler
    {
        public static RSACryptoServiceProvider DecodeRSAPublicKey(byte[] x509key)
        {
            byte[] SeqOID = { 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01 };

            MemoryStream ms = new(x509key);
            BinaryReader reader = new(ms);

            if (reader.ReadByte() == 0x30) ReadASNLength(reader); 
            else return null;

            int identifierSize;
            if (reader.ReadByte() == 0x30) identifierSize = ReadASNLength(reader);
            else return null;

            if (reader.ReadByte() == 0x06)
            {
                int oidLength = ReadASNLength(reader);
                byte[] oidBytes = new byte[oidLength];
                reader.Read(oidBytes, 0, oidBytes.Length);
                if (oidBytes.SequenceEqual(SeqOID) == false) return null;

                int remainingBytes = identifierSize - 2 - oidBytes.Length;
                reader.ReadBytes(remainingBytes);
            }

            if (reader.ReadByte() == 0x03) 
            {
                ReadASNLength(reader);
                reader.ReadByte(); 
                if (reader.ReadByte() == 0x30)
                {
                    ReadASNLength(reader); 
                    if (reader.ReadByte() == 0x02) 
                    {
                        int modulusSize = ReadASNLength(reader);
                        byte[] modulus = new byte[modulusSize];
                        reader.Read(modulus, 0, modulus.Length);
                        if (modulus[0] == 0x00)
                        {
                            byte[] tempModulus = new byte[modulus.Length - 1];
                            Array.Copy(modulus, 1, tempModulus, 0, modulus.Length - 1);
                            modulus = tempModulus;
                        }

                        if (reader.ReadByte() == 0x02) 
                        {
                            int exponentSize = ReadASNLength(reader);
                            byte[] exponent = new byte[exponentSize];
                            reader.Read(exponent, 0, exponent.Length);

                            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                            RSAParameters RSAKeyInfo = new RSAParameters();
                            RSAKeyInfo.Modulus = modulus;
                            RSAKeyInfo.Exponent = exponent;
                            RSA.ImportParameters(RSAKeyInfo);
                            return RSA;
                        }
                    }
                }
            }
            return null;
        }

        private static int ReadASNLength(BinaryReader reader)
        {
            int length = reader.ReadByte();
            if ((length & 0x00000080) == 0x00000080)
            {
                int count = length & 0x0000000f;
                byte[] lengthBytes = new byte[4];
                reader.Read(lengthBytes, 4 - count, count);
                Array.Reverse(lengthBytes); 
                length = BitConverter.ToInt32(lengthBytes, 0);
            }
            return length;
        }

        public static byte[] GenerateAESPrivateKey()
        {
            AesManaged AES = new()
            {
                KeySize = 128
            };
            AES.GenerateKey();
            return AES.Key;
        }

        public static string getServerHash(string serverID, byte[] PublicKey, byte[] SecretKey)
        {
            byte[] hash = digest(new byte[][] { Encoding.GetEncoding("iso-8859-1").GetBytes(serverID), SecretKey, PublicKey });
            bool negative = (hash[0] & 0x80) == 0x80;
            if (negative) { hash = TwosComplementLittleEndian(hash); }
            string result = GetHexString(hash).TrimStart('0');
            if (negative) { result = "-" + result; }
            return result;
        }

        private static byte[] digest(byte[][] tohash)
        {
            SHA1CryptoServiceProvider sha1 = new();
            for (int i = 0; i < tohash.Length; i++)
                sha1.TransformBlock(tohash[i], 0, tohash[i].Length, tohash[i], 0);
            sha1.TransformFinalBlock(new byte[] { }, 0, 0);
            return sha1.Hash;
        }

        private static string GetHexString(byte[] p)
        {
            string result = string.Empty;
            for (int i = 0; i < p.Length; i++)
                result += p[i].ToString("x2");
            return result;
        }

        private static byte[] TwosComplementLittleEndian(byte[] p)
        {
            int i;
            bool carry = true;
            for (i = p.Length - 1; i >= 0; i--)
            {
                p[i] = (byte)~p[i];
                if (carry)
                {
                    carry = p[i] == 0xFF;
                    p[i]++;
                }
            }
            return p;
        }
    }
}
