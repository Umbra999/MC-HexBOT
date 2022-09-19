using MCHexBOT.Core;
using MCHexBOT.HexServer;
using MCHexBOT.Protocol;

namespace MCHexBOT.Features
{
    internal class SkinBlinker
    {
        private static bool SkinBlinkToggle = false;

        public static void ToggleSkinBlinker(MinecraftClient Bot, bool State)
        {
            if (State) new Thread(() => { SkinBlink(Bot); Thread.CurrentThread.IsBackground = true; }).Start();
            else SkinBlinkToggle = false;
        }

        private static void SkinBlink(MinecraftClient Bot)
        {
            SkinBlinkToggle = true;

            while (SkinBlinkToggle)
            {
                byte Random = Encryption.RandomByte();

                Bot.SendPlayerSetings(true, true, ChatMode.Enabled, Random, Random > 127 ? MainHandType.Left : MainHandType.Right, false, "en_us", 64);

                Thread.Sleep(50);
            }

            SkinBlinkToggle = false;
        }
    }
}
