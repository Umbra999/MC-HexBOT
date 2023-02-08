namespace HexBOT.Packets
{
    public interface IPacketHandler
    {
        public void Handshake(IPacket Packet);
        public void Status(IPacket Packet);
        public void Login(IPacket Packet);
        public void Play(IPacket Packet);
    }
}
