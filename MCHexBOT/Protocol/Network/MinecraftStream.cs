using fNbt;
using MCHexBOT.Protocol.Utils;
using MCHexBOT.Utils;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.IO;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;

namespace MCHexBOT.Network
{
	public class MinecraftStream : Stream
	{
		private BufferedBlockCipher EncryptCipher { get; set; }
		private BufferedBlockCipher DecryptCipher { get; set; }

		private Stream BaseStream { get; set; }
        private Protocol.ProtocolType ProtocolType { get; set; }

        public bool DataAvailable
		{
			get
			{
				if (_originalBaseStream is NetworkStream ns) return ns.DataAvailable;

                return _originalBaseStream.Position < _originalBaseStream.Length;
			}
		}

		private readonly Stream _originalBaseStream;
		public MinecraftStream(Stream baseStream, Protocol.ProtocolType Type)
		{
			_originalBaseStream = baseStream;
			BaseStream = baseStream;
            ProtocolType = Type;
		}

		public MinecraftStream(Protocol.ProtocolType Type) : this(new MemoryStream(), Type)
		{

		}

		public void InitEncryption(byte[] key)
		{
			EncryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesEngine(), 8));
			EncryptCipher.Init(true, new ParametersWithIV(new KeyParameter(key), key, 0, 16));

			DecryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesEngine(), 8));
			DecryptCipher.Init(false, new ParametersWithIV(new KeyParameter(key), key, 0, 16));

			BaseStream = new CipherStream(BaseStream, DecryptCipher, EncryptCipher);
		}

		public override bool CanRead => BaseStream.CanRead;
		public override bool CanSeek => BaseStream.CanRead;
		public override bool CanWrite => BaseStream.CanRead;
		public override long Length => BaseStream.Length;

		public override long Position
		{
			get { return BaseStream.Position; }
			set { BaseStream.Position = value; }
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return BaseStream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			BaseStream.SetLength(value);
		}

		public void Read(Span<byte> memory, int count)
		{
			byte[] data = new byte[count];
			BaseStream.Read(data, 0, count);
			data.CopyTo(memory);
		}

		public void ReadMemory(Memory<byte> memory, int count)
		{
			byte[] data = new byte[count];
			BaseStream.Read(data, 0, count);
			data.CopyTo(memory);
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return BaseStream.Read(buffer, offset, count);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			BaseStream.Write(buffer, offset, count);
		}

		public void Write(in Memory<byte> buffer, int offset, in int bufferLength)
		{
			BaseStream.Write(buffer.Slice(offset, bufferLength).Span);
		}

		public override void Flush()
		{
			BaseStream.Flush();
		}

		#region Reader

		public override int ReadByte()
		{
			return BaseStream.ReadByte();
		}

		public byte[] Read(int length)
		{
			if (BaseStream is MemoryStream)
			{
				var dat = new byte[length];
				Read(dat, 0, length);
				return dat;
			}

			int read = 0;

			var buffer = new byte[length];
			while (read < buffer.Length)
			{
				int r = this.Read(buffer, read, length - read);
				if (r < 0) break;

                read += r;
			}

			return buffer;
		}


		public int ReadInt()
		{
			var dat = new byte[4];
			Read(dat, 0, 4);
			var value = BitConverter.ToInt32(dat, 0);
			return IPAddress.NetworkToHostOrder(value);
		}

		public float ReadFloat()
		{
			var almost = Read(4);
			var f = BitConverter.ToSingle(almost, 0);
			return NetworkToHostOrder(f);
		}

		public bool ReadBool()
		{
			var answer = ReadByte();
			if (answer == 1) return true;
			return false;
		}

		public double ReadDouble()
		{
			var almostValue = Read(8);
			return NetworkToHostOrder(almostValue);
		}

		public int ReadVarInt()
		{
			return ReadVarInt(out int read);
		}

		public byte ReadUnsignedByte()
		{
			var buffer = new byte[1];
			Read(buffer);

			return buffer[0];
		}

		public int ReadVarInt(out int bytesRead)
		{
			int numRead = 0;
			int result = 0;
			byte read;
			do
			{
				read = (byte)ReadByte();
				int value = (read & 0x7f);
				result |= (value << (7 * numRead));

				numRead++;
				if (numRead > 5) throw new Exception("VarInt is too big");
            } while ((read & 0x80) != 0);
			bytesRead = numRead;
			return result;
		}

		public long ReadVarLong()
		{
			int numRead = 0;
			long result = 0;
			byte read;
			do
			{
				read = (byte)ReadByte();
				int value = read & 0x7f;
				result |= value << (7 * numRead);

				numRead++;
				if (numRead > 10)
				{
					throw new Exception("VarLong is too big");
				}
			} while ((read & 0x80) != 0);

			return result;
		}

		public short ReadShort()
		{
			var da = Read(2);
			var d = BitConverter.ToInt16(da, 0);
			return IPAddress.NetworkToHostOrder(d);
		}

		public ushort ReadUShort()
		{
			var da = Read(2);
			return NetworkToHostOrder(BitConverter.ToUInt16(da, 0));
		}

		public ushort[] ReadUShort(int count)
		{
			var us = new ushort[count];
			for (var i = 0; i < us.Length; i++)
			{
				var da = Read(2);
				var d = BitConverter.ToUInt16(da, 0);
				us[i] = d;
			}
			return NetworkToHostOrder(us);
		}

		public ushort[] ReadUShortLocal(int count)
		{
			var us = new ushort[count];
			for (var i = 0; i < us.Length; i++)
			{
				var da = Read(2);
				var d = BitConverter.ToUInt16(da, 0);
				us[i] = d;
			}
			return us;
		}

		public short[] ReadShortLocal(int count)
		{
			var us = new short[count];
			for (var i = 0; i < us.Length; i++)
			{
				var da = Read(2);
				var d = BitConverter.ToInt16(da, 0);
				us[i] = d;
			}
			return us;
		}

		public string ReadString()
		{
			var length = ProtocolType == Protocol.ProtocolType.Minecraft ? ReadVarInt() : ReadInt();
			var stringValue = Read(length);

			return Encoding.UTF8.GetString(stringValue);
		}

		public long ReadLong()
		{
			var l = Read(8);
			return IPAddress.NetworkToHostOrder(BitConverter.ToInt64(l, 0));
		}

		public ulong ReadULong()
		{
			var l = Read(8);
			return NetworkToHostOrder(BitConverter.ToUInt64(l, 0));
		}

		public Vector3 ReadPosition()
		{
            long val = ReadLong();
            long x = val >> 38;
            long y = val << 52 >> 52;
            long z = val << 26 >> 38;
            return new Vector3(x, y, z);
		}

		private static double NetworkToHostOrder(byte[] data)
		{
			if (BitConverter.IsLittleEndian) Array.Reverse(data);
            return BitConverter.ToDouble(data, 0);
		}

		private static float NetworkToHostOrder(float network)
		{
			var bytes = BitConverter.GetBytes(network);

			if (BitConverter.IsLittleEndian) Array.Reverse(bytes);

			return BitConverter.ToSingle(bytes, 0);
		}

		private static ushort[] NetworkToHostOrder(ushort[] network)
		{
			if (BitConverter.IsLittleEndian) Array.Reverse(network);
			return network;
		}

		private static ushort NetworkToHostOrder(ushort network)
		{
			var net = BitConverter.GetBytes(network);
			if (BitConverter.IsLittleEndian) Array.Reverse(net);
			return BitConverter.ToUInt16(net, 0);
		}
		private static ulong NetworkToHostOrder(ulong network)
		{
			var net = BitConverter.GetBytes(network);
			if (BitConverter.IsLittleEndian) Array.Reverse(net);
			return BitConverter.ToUInt64(net, 0);
		}

		#endregion

		#region Writer

		public void Write(byte[] data)
		{
			Write(data, 0, data.Length);
		}

		public void WritePosition(Vector3 position)
		{
			var x = Convert.ToInt64(position.X);
			var y = Convert.ToInt64(position.Y);
			var z = Convert.ToInt64(position.Z);
			long toSend = ((x & 0x3FFFFFF) << 38) | ((z & 0x3FFFFFF) << 12) | (y & 0xFFF);
			WriteLong(toSend);
		}

		private int WriteRawVarInt32(uint value)
		{
			int written = 0;
			while ((value & -128) != 0)
			{
				WriteByte((byte)((value & 0x7F) | 0x80));
				value >>= 7;
			}

			WriteByte((byte)value);
			written++;

			return written;
		}

		public int WriteVarInt(int value)
		{
			return WriteRawVarInt32((uint)value);
		}

		public int WriteVarLong(long value)
		{
			int write = 0;
			do
			{
				byte temp = (byte)(value & 127);
				value >>= 7;
				if (value != 0) temp |= 128;
                WriteByte(temp);
				write++;
			} while (value != 0);
			return write;
		}

		public void WriteInt(int data)
		{
			var buffer = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data));
			Write(buffer);
		}

		public void WriteString(string data)
		{
			var stringData = Encoding.UTF8.GetBytes(data);
			if (ProtocolType == Protocol.ProtocolType.Minecraft) WriteVarInt(stringData.Length);
			else WriteInt(stringData.Length);
            Write(stringData);
		}

		public void WriteShort(short data)
		{
			var shortData = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data));
			Write(shortData);
		}

		public void WriteUShort(ushort data)
		{
			var uShortData = BitConverter.GetBytes(data);
			Write(uShortData);
		}

		public void WriteBool(bool data)
		{
			Write(BitConverter.GetBytes(data));
		}

		public void WriteDouble(double data)
		{
			Write(HostToNetworkOrder(data));
		}

		public void WriteFloat(float data)
		{
			Write(HostToNetworkOrder(data));
		}

		public void WriteLong(long data)
		{
			Write(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(data)));
		}

		public void WriteULong(ulong data)
		{
			Write(HostToNetworkOrderLong(data));
		}

		public void WriteUuid(UUID uuid)
		{
			var guid = uuid.GetBytes();
			var long1 = new byte[8];
			var long2 = new byte[8];
			Array.Copy(guid, 0, long1, 0, 8);
			Array.Copy(guid, 8, long2, 0, 8);
			Write(long1);
			Write(long2);
		}

		public void WriteChatObject(ChatMessage data)
		{
			WriteString(JsonConvert.SerializeObject(data));
		}

		public UUID ReadUuid()
		{
			var long1 = Read(8);
			var long2 = Read(8);

			return new UUID(long1.Concat(long2).ToArray());
		}


		private static byte[] HostToNetworkOrder(double d)
		{
			var data = BitConverter.GetBytes(d);
			if (BitConverter.IsLittleEndian) Array.Reverse(data);

			return data;
		}

		private static byte[] HostToNetworkOrder(float host)
		{
			var bytes = BitConverter.GetBytes(host);
			if (BitConverter.IsLittleEndian) Array.Reverse(bytes);

			return bytes;
		}

		private static byte[] HostToNetworkOrderLong(ulong host)
		{
			var bytes = BitConverter.GetBytes(host);
			if (BitConverter.IsLittleEndian) Array.Reverse(bytes);

			return bytes;
		}

		#endregion

		public NbtCompound ReadNbtCompound()
		{
			try
            {
				NbtTagType t = (NbtTagType)(ReadUnsignedByte());

				if (t != NbtTagType.Compound) return null;
				Position--;

				NbtFile file = new() { BigEndian = true };

				file.LoadFromStream(this, NbtCompression.None);

				return (NbtCompound)file.RootTag;
			}
			catch(Exception e)
            {
				Logger.LogError("NBT Error. fNBT might not implement long arrays: " + e.Message);
				return null;
            }
		}

		public void WriteNbtCompound(NbtCompound compound)
		{
			if (compound == null)
			{
				WriteByte(0);
				return;
			}

			NbtFile f = new NbtFile(compound) { BigEndian = true };
			f.SaveToStream(this, NbtCompression.None);
			//WriteByte(0);
		}
	}
}
