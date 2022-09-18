using MCHexBOT.Network;
using MCHexBOT.Utils;
using MCHexBOT.Packets;
using MCHexBOT.Protocol;
using MCHexBOT.Protocol.Packets.LabyServer;
using MCHexBOT.Utils.Data;
using MCHexBOT.Packets.Server.Login;

namespace MCHexBOT.Core
{
    internal class LabyHandler : IPacketHandler
    {
        private LabyClient MinecraftClient { get; set; }
        public MinecraftConnection Connection { get; set; }

        public LabyHandler(LabyClient minecraft)
        {
            MinecraftClient = minecraft;
        }

        public void Handshake(IPacket Packet)
        {
            if (Packet is Protocol.Packets.LabyClient.HelloPacket pongPacket)
            {
                Connection.State = ConnectionState.Login;

                Connection.SendPacket(new LoginDataPacket()
                {
                    name = "_H_E_X_E_D_",
                    id = new UUID("d9900fed-1a1b-4305-9ad2-eead0b1af234"),
                    motd = "www.logout.space"
                });

                Connection.SendPacket(new LoginOptionPacket()
                {
                    ShowServer = false,
                    Status = 0,
                    TimeZone = "UTC"
                });

                Connection.SendPacket(new LoginVersionPacket()
                {
                    Version = 27,
                    Name = "1.8.9_3.9.46",
                    UpdateUrl = ""
                });
            }
        }

        public void Login(IPacket Packet)
        {
            
        }

        public void Play(IPacket Packet)
        {
            
        }

        public void Status(IPacket Packet)
        {
            
        }
    }
}
