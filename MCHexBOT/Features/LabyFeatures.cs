using MCHexBOT.Core;
using MCHexBOT.Utils;

namespace MCHexBOT.Features
{
    internal class LabyFeatures
    {
        public static async Task CollectCoinLoop(LabyClient client, string Pin)
        {
            if (!await client.APIClient.LoginToLaby(Pin))
            {
                Logger.LogError($"{client.APIClient.CurrentUser.name} failed to Login into Laby Dashboard");
                return;
            }

            for (; ; )
            {
                string Coins = await client.APIClient.ClaimDailyCoins();
                if (Coins != null) Logger.LogSuccess($"{client.APIClient.CurrentUser.name} claimed {Coins} Labycoins");
                await Task.Delay(21600000);
            }
        }
    }
}
