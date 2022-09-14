using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.IO;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;

namespace MCHexBOT.Protocol.Utils
{
    public class AesStream : Stream
    {

        private BufferedBlockCipher EncryptCipher;
        private BufferedBlockCipher DecryptCipher;
        private Stream BaseStream;

        public AesStream(Stream stream, byte[] key)
        {
            EncryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesEngine(), 8));
            EncryptCipher.Init(true, new ParametersWithIV(new KeyParameter(key), key, 0, 16));

            DecryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesEngine(), 8));
            DecryptCipher.Init(false, new ParametersWithIV(new KeyParameter(key), key, 0, 16));

            BaseStream = new CipherStream(stream, DecryptCipher, EncryptCipher);
        }

        public override bool CanRead => BaseStream.CanRead;

        public override bool CanSeek => BaseStream.CanSeek;

        public override bool CanWrite => BaseStream.CanWrite;

        public override long Length => BaseStream.Length;

        public override long Position { get => BaseStream.Position; set => throw new NotImplementedException(); }

        public override void Flush()
        {
            BaseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return BaseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return BaseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            BaseStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            BaseStream.Write(buffer, offset, count);
        }
    }
}
