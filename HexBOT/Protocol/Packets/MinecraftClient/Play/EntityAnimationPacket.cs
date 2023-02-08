using HexBOT.Network;

namespace HexBOT.Packets.Client.Play
{
    internal class EntityAnimationPacket : IPacket
    {
        public int EntityId { get; set; }
        public byte AnimationId { get; set; }
        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
            AnimationId = (byte)minecraftStream.ReadByte();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
