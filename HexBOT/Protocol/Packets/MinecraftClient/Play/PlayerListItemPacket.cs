using HexBOT.Network;

namespace HexBOT.Packets.Client.Play
{
    public class PlayerListItemPacket : IPacket
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
                PlayerInfo pi = new()
                {
                    UUID = minecraftStream.ReadUuid()
                };

                switch (Action)
                {
                    case 0:
                        pi.Name = minecraftStream.ReadString();;
                        pi.NumberOfProperties = minecraftStream.ReadVarInt();
                        pi.Properties = new List<PlayerInfoProperty>();

                        for (int ii = 0; ii < pi.NumberOfProperties; ii++)
                        {
                            PlayerInfoProperty prop = new()
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
                        //if (pi.HasDisplayName) pi.DisplayName = minecraftStream.ReadString(); // chat object
                        break;

                    case 1:
                        pi.GameMode = minecraftStream.ReadVarInt();
                        break;
                    case 2:
                        pi.Ping = minecraftStream.ReadVarInt();
                        break;

                    case 3:
                        pi.HasDisplayName = minecraftStream.ReadBool();
                        if (pi.HasDisplayName) pi.DisplayName = minecraftStream.ReadString(); // chat object
                        break;

                    case 4:
                        break;
                }

                Players.Add(pi);
            }
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            throw new NotImplementedException();
        }

        public class PlayerInfo
        {
            public string UUID { get; set; }
            public string Name { get; set; }
            public int NumberOfProperties { get; set; }
            public List<PlayerInfoProperty> Properties { get; set; }
            public int GameMode { get; set; }
            public int Ping { get; set; }
            public bool HasDisplayName { get; set; }
            public string DisplayName { get; set; }
        }

        public class PlayerInfoProperty
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public bool Singed { get; set; }
            public string Signature { get; set; }
        }
    }
}
