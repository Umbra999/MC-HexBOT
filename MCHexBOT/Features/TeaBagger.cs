using MCHexBOT.Core;
using MCHexBOT.Protocol;

namespace MCHexBOT.Features
{
    internal class TeaBagger
    {
        private static bool TeaBaggerToggle = false;

        public static void ToggleTeaBagger(MinecraftClient Bot, bool State)
        {
            if (State) new Thread(() => { TeaBag(Bot); Thread.CurrentThread.IsBackground = true; }).Start();
            else TeaBaggerToggle = false;
        }

        private static void TeaBag(MinecraftClient Bot)
        {
            TeaBaggerToggle = true;

            while (TeaBaggerToggle)
            {
                Bot.SendEntityAction(PlayerAction.StartSneaking);
                Thread.Sleep(40);
                Bot.SendEntityAction(PlayerAction.StopSneaking);
                Thread.Sleep(40);
            }

            TeaBaggerToggle = false;
        }
    }
}
