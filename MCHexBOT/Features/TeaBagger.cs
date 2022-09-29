using MCHexBOT.Core.Minecraft;
using MCHexBOT.Packets.Server.Play;
using MCHexBOT.Utils;

namespace MCHexBOT.Features
{
    internal class TeaBagger
    {
        private static bool TeaBaggerToggle = false;

        public static void ToggleTeaBagger(MinecraftClient Bot)
        {
            if (!TeaBaggerToggle) Task.Run(() => TeaBagLoop(Bot));
            else TeaBaggerToggle = false;
        }

        private static async Task TeaBagLoop(MinecraftClient Bot)
        {
            TeaBaggerToggle = true;
            Logger.LogDebug("TeaBagger enabled");

            while (TeaBaggerToggle)
            {
                Bot.SendEntityAction(EntityActionPacket.Action.StartSneaking);
                await Task.Delay(50);
                Bot.SendEntityAction(EntityActionPacket.Action.StopSneaking);
                await Task.Delay(50);
            }

            TeaBaggerToggle = false;
            Logger.LogDebug("TeaBagger disabled");
        }
    }
}
