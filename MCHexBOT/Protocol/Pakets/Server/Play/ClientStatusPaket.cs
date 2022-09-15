using MCHexBOT.Network;

namespace MCHexBOT.Pakets.Server.Play
{
    public class ClientStatusPaket : IPaket
    {
        public int ActionID { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            ActionID = minecraftStream.ReadVarInt();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(ActionID);
        }
    }
}
