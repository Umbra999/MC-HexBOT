using MCHexBOT.Core;
using MCHexBOT.Packets.Server.Play;

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
                Bot.SendEntityAction(EntityActionPacket.Action.StartSneaking);
                Thread.Sleep(50);
                Bot.SendEntityAction(EntityActionPacket.Action.StopSneaking);
                Thread.Sleep(50);
            }

            TeaBaggerToggle = false;
        }
    }
}
