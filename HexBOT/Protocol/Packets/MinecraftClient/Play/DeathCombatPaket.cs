using HexBOT.Network;
using HexBOT.Utils;

namespace HexBOT.Packets.Client.Play
{
    internal class DeathCombatPacket : IPacket
    {
        public int EntityID { get; set; }
        public int KillerEntityID  { get; set; }
        public string Message { get; set; }
        public void Decode(MinecraftStream minecraftStream)
        {
            EntityID = minecraftStream.ReadVarInt();
            KillerEntityID = minecraftStream.ReadInt();
            Message = minecraftStream.ReadString();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
