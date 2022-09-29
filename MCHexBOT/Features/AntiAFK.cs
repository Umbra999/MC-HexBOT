using MCHexBOT.Core.Minecraft;
using MCHexBOT.HexServer;
using MCHexBOT.Protocol;
using MCHexBOT.Utils;
using System.Numerics;

namespace MCHexBOT.Features
{
    internal class AntiAFK
    {
        private static readonly int AFKDelay = 300000; // 5 min
        private static bool AntiAFKToggle = false;

        private static bool AntiChatAFK = false;
        private static bool AntiLookAFK = true;
        private static bool AntiMovementAFK = false;

        public static void ToggleAntiAFK(MinecraftClient Bot)
        {
            if (!AntiAFKToggle) Task.Run(() => AntiAFKLoop(Bot));
            else AntiAFKToggle = false;
        }

        private static async Task AntiAFKLoop(MinecraftClient Bot)
        {
            AntiAFKToggle = true;
            Logger.LogDebug("AntiAFK enabled");

            while (AntiAFKToggle)
            {
                if (AntiChatAFK) Bot.SendChat(Encryption.RandomString(15));

                if (AntiLookAFK)
                {
                    Movement.LookAtDirection(Bot, Direction.North);
                    await Task.Delay(50);
                    Movement.LookAtDirection(Bot, Direction.East);
                }

                if (AntiMovementAFK)
                {
                    await Movement.MoveToPosition(Bot, Bot.GetLocalPlayer().Position + new Vector3(0, 0, -0.1f));
                    await Task.Delay(50);
                    await Movement.MoveToPosition(Bot, Bot.GetLocalPlayer().Position + new Vector3(0.1f, 0, 0));
                }

                await Task.Delay(AFKDelay);
            }

            AntiAFKToggle = false;
            Logger.LogDebug("AntiAFK disabled");
        }
    }
}
