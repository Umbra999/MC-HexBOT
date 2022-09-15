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
            }
        }
    }
}
