using HexBOT.Network;

namespace HexBOT.Packets.Server.Login
{
    public class EncryptionResponsePacket : IPacket
    {
        public int SharedKeyLenght { get; set; }
        public byte[] SharedKey { get; set; }
        public int VerifyTokenLenght { get; set; }
        public byte[] VerifyToken { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(SharedKeyLenght);
            minecraftStream.Write(SharedKey);
            minecraftStream.WriteVarInt(VerifyTokenLenght);
            minecraftStream.Write(VerifyToken);
        }
    }
}
