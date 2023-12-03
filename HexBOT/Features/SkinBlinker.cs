using HexBOT.Core.Minecraft;
using HexBOT.HexedServer;
using HexBOT.Utils;

namespace HexBOT.Features
{
    internal class SkinBlinker
    {
        private static bool SkinBlinkToggle = false;

        public static void ToggleSkinBlinker(MinecraftClient Bot)
        {
            if (!SkinBlinkToggle) Task.Run(() => SkinBlinkLoop(Bot));
            else SkinBlinkToggle = false;
        }

        private static async Task SkinBlinkLoop(MinecraftClient Bot)
        {
            SkinBlinkToggle = true;
            Logger.LogDebug("SkinBlinker enabled");

            while (SkinBlinkToggle)
            {
                byte Random = (byte)Encryption.Random.Next(0, 255);

                Bot.SendPlayerSetings(true, Packets.Server.Play.ClientSettingsPacket.ChatType.Enabled, Random, "en_us", 64);

                await Task.Delay(50);
            }

            SkinBlinkToggle = false;
            Logger.LogDebug("SkinBlinker disabled");
        }
    }
}
