using MCHexBOT.Core;
using MCHexBOT.Features;
using MCHexBOT.Utils;

namespace MCHexBOT
{
    internal class Main
    {
        public static List<MinecraftClient> Clients = new();
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
            APIClient Client = new();

            if (await Client.Login("XBL3.0 x=2276113179421336668;eyJlbmMiOiJBMTI4Q0JDLUhTMjU2IiwiYWxnIjoiUlNBLU9BRVAiLCJjdHkiOiJKV1QiLCJ6aXAiOiJERUYiLCJ4NXQiOiJzYVkzV1ZoQzdnMmsxRW9FU0Jncm9Ob2l3MVEifQ.PtSEQvIm2Ad8cC_FMKi8hbtSJeJ2vFjZecL11MeLaIGQ5BMd1in3bUyLv5_c6Hu2nKMvrvYC7isTDewhVZ5Ee0mmVeX8D_YiLxc0LQN-5wcKfQXOBhS44Y1TlOLomxArVvuEAbuYtPAs3lkrmkDwgEQKFgP9AZP2x0sPAL5Y9zo8rbJadTzV3sUEV_Ie4Tmq4mi-RGPhfpPs6Rl2okFJ2rgdwMK4kxK4fxDkSG3Gr6NaAUyCz2wuWISahqlkRH9-V4prgKFq-P0thkzP67lUCsh230QlA9vG1tSr6rD3NSWDFH0N8Mh_jzEfPqG3qlfDruGfMvEaT5wtrHkEozBkmg.fEi3ENT5ExQI7_WJ6_0PvQ.fKS8Fz1QgspbylXsiyEpIQM6SiL8m1a07wXZqqxoQiLVpWDIh0nKPcQiJEkOs2Uf1XT_Sw1nK87ZsmT03zJ8l57gDLN8TgnIJlXMbc8kU-K9z_AkeEnsBg_k-VHogMZciMbViNjHm6yAEnbQibgWGh9He1AbBQA9RIEH1jUzBrt_6zphGFsK_ImbcxqkGN-pTyt12dE8TujmObiekV7Sa3PpAX2KWHc0uW1PAJa-_93TjOl-P34bmSNB_6oL49V1hs1_Vm8Q5LSTQAjgWhLFWdVSo1PHQYbbAftWj559d3FfR19i9fyNrwtBzY3FkIlr76my1GWtpkNPkZZyWWCgGjMtjA58d2pO0au2Ns4ZaViJZ-IBSAWbrZ5D7QmkCNz4eMfqPZKQpLAG0VGWrc63F5rPxyOdRxyjrgYCz73MwlNH1_09FA6fbiEfWM1Ix_xrGHi6RWcNQaG9fjfbq9SrR_CWaio8Ht_u8_Jk708xCpN3gFGhQE1qLzaFNSRfwo8R25IhGzWpvFwC1itFYh2xCCjJtHKjCudiggMDM0UvkCOHXpupvudtZwoLfWyTujhl2Dpyq9S-q6cdRpdN7IRFVhypP9fjxNEQ2HTYb5XFB5P8KSw5ABd8SDInnY009XhNeMTpJ9T5GZNtSB4zI3HtkuPKxcjixznwALsyhBv2LMPjE2mQ6yb9xp872W1DPiieZG0DxWXki8LAQkoxrLBm5doKOL1NoTLQo8YwUb9JJsBMCPgMCC4iqalpJsT5usjPFpFUs1IfcA34xR7Q-s_q2QpOVLgUIKz6lY8MC9mJEuEskdJa4cTJKHukCVLJpyA5QN2QOXGJLZLOgqhr34U5hMKPPXNbtSPCvSIwBIuQXAq4CrMe1aMHWVpsCWYlXQweCC2sFtHP_12BtxavRBC70jmcg_XzWp-FtHuEAsF4UH0k5CsfA-VA7QIgdjAMTqNP_qPsEoqXyrUMaM2PYcJth_wxpLYDz8mn2QHM1_Kl6KG1TUkfZkrZiYkJYQSKEm4m2TGHHkbSsbGqxqW4aGWFpTz8bYGXNMcm6O__fJoibCClDiNX47Lm6WUBpth5GlxcX5uEVn1ukepBcOn84W45MVY6xwCqRqiVk64oBxOqdtSuozifszFhhcyz_mREvxmzr1fBz2waE02tLCYRdGpS5BKAgWtlbE-33QfO8KD4WRAvNarnrFwEjXzjsv8wgzn-so2dRmXlMZ99Mc7Rhyvy8dreIQGD6yjk2D1RaE4EH7XXvPh5ySMGNUF-DBK_zwYEf52BWps8dhQkrBbzrdRg6pc_98i-VtADFMxNJjMzjwCeI3fy1zn8uhuI6jhzJJFkPTX07MOzXCCp_j_T_NpeRSL0Ofp7d5eaRJFg_2YhuXbKd8cDvB8JSmhOmJrmiCub_3iy6qyU9IQHFA0DO2bE2jU29P06Ic51aauQlaoYFbTVMs1FYyp92Sc0d0FEy4f_HngmzYdfNeeGXHn36j-CfxsnVMyg-Fzj2II4WMXAWAL0rPWqdztspDoDojqgD1DuONvJTcXWI_okJBgqx_VdPGkipW59jjTalJh4iYOMs8RpKkHRArrQ5YiYaEyhmUT_.lxUqHtrpjZiOe-lTZS2CKg"))
            {
                Clients.Add(new MinecraftClient(Client));
            }
        }

        private static void RunGUI()
        {
            for (; ; )
            {
                Logger.LogImportant("-----------------");
                Logger.LogImportant("J [IP:PORT] - Join a Server");
                Logger.LogImportant("C [MESSAGE] - Send a Chat Message");
                Logger.LogImportant("R - Respawn");
                Logger.LogImportant("S [true / false] - Skinblinker");

                string input = Console.ReadLine();
                new Thread(() => { HandleInput(input); Thread.CurrentThread.IsBackground = true; }).Start();
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
                        Client.Connect("1.18", Server.Split(':')[0], Convert.ToInt32(Server.Split(':')[1]));
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
                        new Thread(() => { Client.SendMovement(); Thread.CurrentThread.IsBackground = true; }).Start();
                    }
                    break;

                case "s":
                    foreach (MinecraftClient Client in Clients)
                    {
                        SkinBlinker.ToggleSkinBlinker(Client, input.Substring(2) == "true");
                    }
                    break;
            }
        }
    }
}
