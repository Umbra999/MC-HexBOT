using MCHexBOT.Network;

namespace MCHexBOT.Pakets.Client.Play
{
    internal class DeathCombatPaket : IPaket
    {
        public int TargetEntityID { get; set; }
        public int KillerEntityID  { get; set; }
        public string Message { get; set; }
        public void Decode(MinecraftStream minecraftStream)
        {
            TargetEntityID = minecraftStream.ReadVarInt();
            KillerEntityID = minecraftStream.ReadInt();
            Message = minecraftStream.ReadChatObject();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(TargetEntityID);
            minecraftStream.WriteInt(KillerEntityID);
            minecraftStream.WriteString(Message);
        }
    }
}
