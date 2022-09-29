using MCHexBOT.Core.Laby;
using MCHexBOT.Utils;

namespace MCHexBOT.Features
{
    internal class LabyFeatures
    {
        public static async Task CollectCoins(LabyClient client, string Pin)
        {
            if (!await client.MinecraftClient.APIClient.LoginToLaby(Pin))
            {
                Logger.LogError($"{client.MinecraftClient.APIClient.CurrentUser.name} failed to Login into Laby Dashboard");
                return;
            }

            string Coins = await client.MinecraftClient.APIClient.ClaimDailyCoins();
            if (Coins != null) Logger.LogSuccess($"{client.MinecraftClient.APIClient.CurrentUser.name} claimed {Coins} Labycoins");
        }
    }
}
