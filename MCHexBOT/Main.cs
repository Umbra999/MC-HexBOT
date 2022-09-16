using MCHexBOT.Core;
using MCHexBOT.Features;
using MCHexBOT.Protocol;
using MCHexBOT.Utils;

namespace MCHexBOT
{
    internal class Main
    {
        public static List<MinecraftClient> Clients = new();
        public static List<string> AccountTokens = new()
        {
            "XBL3.0 x=3054988950680487743;eyJlbmMiOiJBMTI4Q0JDLUhTMjU2IiwiYWxnIjoiUlNBLU9BRVAiLCJjdHkiOiJKV1QiLCJ6aXAiOiJERUYiLCJ4NXQiOiJzYVkzV1ZoQzdnMmsxRW9FU0Jncm9Ob2l3MVEifQ.14iAssjRWq1-MyyonevSEqDjgsQlJ8E9yj4fgQQ84v8jLKPtmjWADAiQjZ-xLNNnvOHJz6LmHBo8TjVXllTyXra9M1DRRcnRPPPW2J3mCIlRV4DEBFo7GovxuwYn9f5rnTngDF1maCc0yijtol4FgRMI8Mac9l8mgrnvqDm9drU-LI_KeTQ9VDXM4KZLre5p0saTBuLKrHKVodlohE1ccEDOFOlx2QdQuecqtzTI7g0b76vTfonU2_uMplbaPin4Vyxa71cC08pi_haEQpVf9GUyZWGxKZ0y8Yx44AxpjsX17QZ44HqdQdy82iWdOKOaBXZqe3fWmaYQsIWrkdrpKw.aTW_5IJywfyHbipougZRtg.D4XldsFb_3oWq0BPlk8aePcDXo5gCWKDM3j-r7UanwrdShYMxepWHRTY2ObmOgyqIMGeE5Ce4pY95KyD7ZFWXZB-WSdjQuffS9siXsFrPS-BdIqPlP2RBQrl7Hr2h2sV_WFx7lyU2iyVKO3QWJPi-gUrFEjKPP6jFLxpzvfV4cSk53z_RbJ4b7y7H1lCDYD7nkpe1AKCC-C107J1X2DiQSDLwAMGY2eAk7WFZeGsL7CvAwSOAyJpj6aoP1n2T85fBRjZ5MtvhIWIBZkLNQYUMBPBs4Z6QBjn5hkaHisJ0PhwBgpenURIHDV6DW7KS0Dfw4fT52W6c2dY7y_US-fXW_IZk9FIscbhzQZ3MUEOI0jmYrlxeLtffrW6EvIrr873Ks7a_d17kyh-RZrVCvwj2YJl0dj1mUKBylhE3PyjaZSk_rYApamV_G40HIcuu_Cq1vonUtdmKYoiMsqudbI6vOQ6-ao_8FkTHc0suAEgsh62nK34a5GwOVNX14vP_BExU4stGBctdlfbPx7V9qlaPugGoH3Q573GLOn08SeCE2hWzpr7QVtfvrnE7oUXN3hv9eNbfCvwY6frSBxFN-Nq1sZ_OYVJlfrrzJm71BcBHSUb3RMtaNF38Ovk19oXZ4magTyQHAJtTtsPtYwS6TTc2qnll9pQUSi1O0nIljglWsYXo0czf8EVwqqpVOUN4vt4LHqfLaM-_o4AcGwZNkyBjMxUh3ro6T0KK7J11OsJwJpZirKbXV60Tke9j5btnEB-4Tsic03J18wZhOl2fHfAt5cwVXwJLLVgpzdahQcD1zStiyfx6it07sEz6XbbhJygl9AqMJ6ADB5IPWYIN1KtWyll1Uva8CRJ56Hy9HdeY8SP3l-d9JWr7h2ZhQivUlLZgbTOw-P64rf5I_dvDzk1tMmKgURnNFr_snDe-QGwSCZt7Gx-GgxeTyiRSzMBysp-eVUFl96ALJYgaLZFRNpgyT7kZc-PjSNeAln84SEPxgxyZuONeJgPAqreujmxux2ydLuaWZAApwLHHn7Qb8WY52XVyi2a6y48jabYWo8z3jBQ19XdlVR8ffeDAGXo_5GmNrXfDI-C38meprxFsZLl5PcXGQaSwMKTMAOYwbAdqLtKrkN8IlwGQARuOdD28N_TVqE2DzCO1sR3Le9adzILaQ-1O5NWO5BB-rhJMQSi8e6xDYqWPgpF-x2yvJkUtwFirreAZBQJW5Q-Ecq7YsK-_JCp6krJHmjavW-KNcXj1ZoPbmTj8wo_zSA_ZNaMJzyTE91XvlDauFluxG9JXLtf-Zam1KF6bmGm_oHediTzx4jzTbfwyJ-LCTmmGKHFYwdZ023ONuwC2VGeo-Tub9N2skkEiu02_5biItJu3jJtYVcKQsVANpYSLtt-VMps0LmwDdRBYAKDkBO1oRdS0xC4YhM_1p96dOH_PccxzxNgVjQa-ROo_Qw36w09UsbTnMBWPgXycf1qHZsU0EobMWkvFehGJCTlUpXKS4I6EQ5eVLlAYGkgu19FRm6MGi8llArm-1djgRTnuzjpkQYi-NRFeDBWPmKzz8AoGs_yxHv_Jt6elWQVd1MRiGiO2Nat72XD.fpJD1KwOy2_nhVGAABMvAQ",
        };

        public static void Init()
        {
            Console.Title = "HexBOT | Minecraft";
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(@"
          
 ▄▀▀▄ ▄▄   ▄▀▀█▄▄▄▄  ▄▀▀▄  ▄▀▄  ▄▀▀█▄▄▄▄  ▄▀▀█▄▄  
█  █   ▄▀ ▐  ▄▀   ▐ █    █   █ ▐  ▄▀   ▐ █ ▄▀   █ 
▐  █▄▄▄█    █▄▄▄▄▄  ▐     ▀▄▀    █▄▄▄▄▄  ▐ █    █ 
   █   █    █    ▌       ▄▀ █    █    ▌    █    █ 
  ▄▀  ▄▀   ▄▀▄▄▄▄       █  ▄▀   ▄▀▄▄▄▄    ▄▀▄▄▄▄▀ 
 █   █     █    ▐     ▄▀  ▄▀    █    ▐   █     ▐  
 ▐   ▐     ▐         █    ▐     ▐        ▐        
      _
     |u|
   __|m|__
   \+-b-+/       Beautiful girls, dead feelings...
    ~|r|~        one day the sun is gonna explode and all this was for nothing.
     |a|
      \|
");

            CreateBots();
            RunGUI();
        }

        private static async void CreateBots()
        {
            foreach (string Token in AccountTokens)
            {
                APIClient Client = new();
                if (await Client.Login(Token)) Clients.Add(new MinecraftClient(Client));
                else Logger.LogError("Failed to Validate Token");
            }
        }

        private static void RunGUI()
        {
            for (; ; )
            {
                Logger.LogImportant("-----------------");
                Logger.LogImportant("J [IP:PORT] - Join a Server");
                Logger.LogImportant("C [MESSAGE] - Send a Chat Message");
                Logger.LogImportant("-----------------");
                Logger.LogImportant("S [true / false] - Skinblinker");
                Logger.LogImportant("T [true / false] - Teabagger");
                Logger.LogImportant("Z [true / false] - Sneak");
                Logger.LogImportant("-----------------");

                string input = Console.ReadLine();
                HandleInput(input);
            }
        }

        private static void HandleInput(string input)
        {
            string InputStart = input.Split(' ')[0];
            switch (InputStart.ToLower())
            {
                case "j":
                    string Server = input.Substring(2);
                    foreach (MinecraftClient Client in Clients)
                    {
                        Client.Connect("1.18", Server.Split(':')[0], Server.Split(':').Length > 1 ? Convert.ToInt32(Server.Split(':')[1]) : 25565);
                    }
                    break;

                case "c":
                    foreach (MinecraftClient Client in Clients)
                    {
                        Client.SendChat(input.Substring(2));
                    }
                    break;

                case "r":
                    foreach (MinecraftClient Client in Clients)
                    {
                        Client.SendRespawn();
                    }
                    break;

                case "z":
                    foreach (MinecraftClient Client in Clients)
                    {
                        Client.SendEntityAction(input.Substring(2) == "true" ? PlayerAction.StartSneaking : PlayerAction.StopSneaking);
                    }
                    break;

                case "t":
                    foreach (MinecraftClient Client in Clients)
                    {
                        TeaBagger.ToggleTeaBagger(Client, input.Substring(2) == "true");
                    }
                    break;

                case "s":
                    foreach (MinecraftClient Client in Clients)
                    {
                        SkinBlinker.ToggleSkinBlinker(Client, input.Substring(2) == "true");
                    }
                    break;

                case "v":
                    foreach (MinecraftClient Client in Clients)
                    {
                        Client.MCConnection.SendPaket(new Pakets.Server.Play.PlayerPositionAndRotationPaket()
                        {
                            X = Client.LocalPlayer.Position.X + 1,
                            Y = Client.LocalPlayer.Position.Y,
                            Z = Client.LocalPlayer.Position.Z,
                            Pitch = Client.LocalPlayer.Rotation.Y,
                            Yaw = Client.LocalPlayer.Rotation.X,
                            OnGround = true,
                        });
                    }
                    break;
            }
        }
    }
}
