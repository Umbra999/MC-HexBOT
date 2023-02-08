using HexBOT.Core.Minecraft;
using HexBOT.HexServer;
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
                byte Random = Encryption.RandomByte();

                Bot.SendPlayerSetings(true, true, Packets.Server.Play.ClientSettingsPacket.ChatType.Enabled, Random, Random > 127 ? Packets.Server.Play.ClientSettingsPacket.MainHandType.Left : Packets.Server.Play.ClientSettingsPacket.MainHandType.Right, false, "en_us", 64);

                await Task.Delay(50);
            }

            SkinBlinkToggle = false;
            Logger.LogDebug("SkinBlinker disabled");
        }
    }
}
