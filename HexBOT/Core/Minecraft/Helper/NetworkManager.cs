using HexBOT.Packets.Client.Play;
using HexBOT.Utils;
using System.Text;

namespace HexBOT.Core.Minecraft.Helper
{
    public class NetworkManager
    {
        public NetworkManager(MinecraftClient Client)
        {
            Bot = Client;
        }

        private readonly MinecraftClient Bot;

        public void OnConnectedToServer(JoinGamePacket packet)
        {
            Logger.LogSuccess($"{Bot.APIClient.CurrentUser.name} connected to Server");

            Logger.LogDebug("Gamemode: " + packet.Gamemode);
            Logger.LogDebug("Level: " + packet.LevelType);
            Logger.LogDebug("Dimension: " + packet.Dimension);
            Logger.LogDebug("Reduced Debug: " + packet.ReducedDebugInfo);
            Logger.LogDebug("Difficulty: " + packet.Difficulty);
            Logger.LogDebug("EntityID: " + packet.EntityId);

            Bot.EntityManager.ClearAllPlayers();

            //Bot.EntityManager.LocalPlayer.EntityID = packet.EntityId;

            Bot.SendPlayerSetings(true, Packets.Server.Play.ClientSettingsPacket.ChatType.Enabled, byte.MaxValue, "en_GB", 64);
        }

        public void OnDisconnectedFromServer(string Reason)
        {
            Logger.LogError($"{Bot.APIClient.CurrentUser.name} disconnected from Server: {Reason}");
            Bot.EntityManager.ClearAllPlayers();
        }

        public static uint PingDelay = 0; 
        public void OnPingReceived(KeepAlivePacket packet)
        {
            Task.Run(async () =>
            {
                await Task.Delay((int)PingDelay);
                Bot.MCConnection.SendPacket(new Packets.Server.Play.KeepAlivePacket()
                {
                    KeepAliveID = packet.KeepAliveID
                });
            });
        }

        public void OnPluginMessageReceived(PluginMessagePacket packet)
        {
            switch (packet.Channel)
            {
                case "minecraft:brand":
                    Bot.MCConnection.SendPacket(new Packets.Server.Play.PluginMessagePacket()
                    {
                        Channel = packet.Channel,
                        Data = Encoding.UTF8.GetBytes("vanilla")
                    });
                    break;

                case "minecraft:register":
                    Bot.MCConnection.SendPacket(new Packets.Server.Play.PluginMessagePacket()
                    {
                        Channel = packet.Channel,
                        Data = packet.Data
                    });
                    break;
            }
        }
    }
}
