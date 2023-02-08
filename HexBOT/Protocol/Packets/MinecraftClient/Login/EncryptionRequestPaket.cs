using HexBOT.Network;

namespace HexBOT.Packets.Client.Login
{
    public class EncryptionRequestPacket : IPacket
    {
        public string ServerId { get; set; }
        public int PublicKeyLenght { get; set; }
        public byte[] PublicKey { get; set; }
        public int VerifyTokenLenght { get; set; }
        public byte[] VerifyToken { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            ServerId = minecraftStream.ReadString();
            PublicKeyLenght = minecraftStream.ReadVarInt();
            PublicKey = new byte[PublicKeyLenght];
            minecraftStream.Read(PublicKey, PublicKeyLenght);
            VerifyTokenLenght = minecraftStream.ReadVarInt();
            VerifyToken = new byte[VerifyTokenLenght];
            minecraftStream.Read(VerifyToken, VerifyTokenLenght);
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
