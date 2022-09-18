using MCHexBOT.Network;
using MCHexBOT.Utils;

namespace MCHexBOT.Packets.Client.Play
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
            minecraftStream.WriteVarInt(EntityID);
            minecraftStream.WriteInt(KillerEntityID);
            minecraftStream.WriteString(Message);
        }
    }
}
