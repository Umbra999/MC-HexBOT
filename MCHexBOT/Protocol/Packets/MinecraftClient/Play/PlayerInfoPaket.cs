using MCHexBOT.Network;
using MCHexBOT.Utils;

namespace MCHexBOT.Packets.Client.Play
{
    public class PlayerInfoPacket : IPacket
    {
        public int Action { get; set; }
        public int NumberOfPlayers { get; set; }
        public List<PlayerInfo> Players { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            Action = minecraftStream.ReadVarInt();
            NumberOfPlayers = minecraftStream.ReadVarInt();
            Players = new List<PlayerInfo>();
            for(int i = 0; i < NumberOfPlayers; i++)
            {
                var pi = new PlayerInfo
                {
                    UUID = minecraftStream.ReadUuid()
                };

                switch (Action)
                {
                    case 0:
                        pi.Name = minecraftStream.ReadString();
                        pi.NumberOfProperties = minecraftStream.ReadVarInt();
                        pi.Properties = new List<PlayerInfoProperty>();

                        for (int ii = 0; ii < pi.NumberOfProperties; ii++)
                        {
                            var prop = new PlayerInfoProperty
                            {
                                Name = minecraftStream.ReadString(),
                                Value = minecraftStream.ReadString(),
                                Singed = minecraftStream.ReadBool(),
                            };
                            if (prop.Singed) prop.Signature = minecraftStream.ReadString();

                            pi.Properties.Add(prop);
                        }

                        //pi.GameMode = minecraftStream.ReadVarInt();
                        //pi.Ping = minecraftStream.ReadVarInt();
                        //pi.HasDisplayName = minecraftStream.ReadBool();
                        //if (pi.HasDisplayName) pi.DisplayName = minecraftStream.ReadString();
                        break;

                    case 1:
                        pi.GameMode = minecraftStream.ReadVarInt();
                        break;
                    case 2:
                        pi.Ping = minecraftStream.ReadVarInt();
                        break;
                    case 3:
                        pi.HasDisplayName = minecraftStream.ReadBool();
                        if (pi.HasDisplayName) pi.DisplayName = minecraftStream.ReadString();
                        break;
                }

                Players.Add(pi);
            }
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }
    }
}
