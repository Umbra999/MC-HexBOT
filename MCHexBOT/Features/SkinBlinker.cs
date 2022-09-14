using MCHexBOT.Core;
using MCHexBOT.Utils;

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
                byte Random = Misc.RandomByte();

                Bot.MCConnection.SendPaket(new Pakets.Server.Play.ClientSettingsPaket()
                {
                    AllowServerListings = true,
                    ChatColors = true,
                    ChatMode = 0,
                    DisplayedSkinParts = Random,
                    MainHand = Random > 127 ? 1 : 0,
                    EnableTextFiltering = false,
                    Locale = "en_us",
                    ViewDistance = 64
                });

                Thread.Sleep(50);
            }

            SkinBlinkToggle = false;
        }
    }
}
