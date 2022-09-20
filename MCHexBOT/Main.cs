using MCHexBOT.Core;
using MCHexBOT.Features;
using MCHexBOT.HexServer;
using MCHexBOT.Packets.Server.Play;
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

            Task.Run(() => CreateBots());

            RunGUI();
        }

        private static async Task CreateBots()
        {
            await ServerHandler.Init();

            if (!File.Exists("Tokens.txt"))
            {
                Logger.LogError("No Tokens found");
                Thread.Sleep(5000);
                return;
            }

            foreach (string Token in File.ReadAllLines("Tokens.txt"))
            {
                APIClient Client = new();
                if (await Client.LoginToMinecraft(Token))
                {
                    Clients.Add(new MinecraftClient(Client));
                }
                else Logger.LogError("Failed to Validate Token");
            }
            Console.Title = $"HexBOT | {Clients.Count} Bots";
        }

        private static void RunGUI()
        {
            for (; ; )
            {
                Logger.LogImportant("-----------------");
                Logger.LogImportant("1 - CORE");
                Logger.LogImportant("2 - EXPLOITS");
                Logger.LogImportant("3 - PHYSICS");
                Logger.LogImportant("4 - DEBUG");
                Logger.LogImportant("-----------------");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Logger.LogImportant("-----------------");
                        Logger.LogImportant("J [IP:PORT] - Join a Server");
                        Logger.LogImportant("C [MESSAGE] - Send a Chat Message");
                        Logger.LogImportant("-----------------");
                        HandleCoreInput(Console.ReadLine());
                        break;

                    case "2":
                        Logger.LogImportant("-----------------");
                        Logger.LogImportant("Z [true / false] - Skinblinker");
                        Logger.LogImportant("T [true / false] - Teabagger");
                        Logger.LogImportant("K [NAME] - Target the Player");
                        Logger.LogImportant("-----------------");
                        HandleExploitInput(Console.ReadLine());
                        break;

                    case "3":
                        Logger.LogImportant("-----------------");
                        Logger.LogImportant("F [Name] - Follow a Player");
                        Logger.LogImportant("X [+,-,/] - Move the X Cordinate");
                        Logger.LogImportant("Y [+,-,/] - Move the Y Cordinate");
                        Logger.LogImportant("Z [+,-,/] - Move the Z Cordinate");
                        Logger.LogImportant("S - Toggle Sneak");
                        Logger.LogImportant("-----------------");
                        HandlePhysicInput(Console.ReadLine());
                        break;

                    case "4":
                        Logger.LogImportant("-----------------");
                        Logger.LogImportant("N [NAME] - Change the Name");
                        Logger.LogImportant("S [URL] - Change the Skin");
                        Logger.LogImportant("O - Refresh the Oversee");
                        Logger.LogImportant("-----------------");
                        HandleDebugInput(Console.ReadLine());
                        break;
                }
            }
        }

        private static void HandleCoreInput(string input)
        {
            string InputStart = input.Split(' ')[0];
            switch (InputStart.ToLower())
            {
                case "j":
                    string Server = input.Substring(2);
                    foreach (MinecraftClient Client in Clients)
                    {
                        Task.Run(() => Client.Connect("1.18", Server.Split(':')[0], Server.Split(':').Length > 1 ? Convert.ToInt32(Server.Split(':')[1]) : 25565));
                    }
                    break;

                case "c":
                    foreach (MinecraftClient Client in Clients)
                    {
                        Client.SendChat(input.Substring(2));
                    }
                    break;
            }
        }

        private static void HandleExploitInput(string input)
        {
            string InputStart = input.Split(' ')[0];
            switch (InputStart.ToLower())
            {
                case "z":
                    foreach (MinecraftClient Client in Clients)
                    {
                        SkinBlinker.ToggleSkinBlinker(Client, input.Substring(2) == "true");
                    }
                    break;

                case "t":
                    foreach (MinecraftClient Client in Clients)
                    {
                        TeaBagger.ToggleTeaBagger(Client, input.Substring(2) == "true");
                    }
                    break;

                case "k":
                    Combat.ToggleAttack(input.Substring(2));
                    break;
            }
        }

        private static void HandlePhysicInput(string input)
        {
            string InputStart = input.Split(' ')[0];
            switch (InputStart.ToLower())
            {
                case "f":
                    Movement.CopyMovementTarget = input.Substring(2);
                    break;

                case "x":
                    {
                        switch (input.Substring(2))
                        {
                            case "/":
                                Movement.WalkX = Movement.MovementPosition.None;
                                break;

                            case "+":
                                Movement.WalkX = Movement.MovementPosition.Forward;
                                break;

                            case "-":
                                Movement.WalkX = Movement.MovementPosition.Backward;
                                break;
                        }
                    }
                    break;

                case "y":
                    {
                        switch (input.Substring(2))
                        {
                            case "/":
                                Movement.WalkY = Movement.MovementPosition.None;
                                break;

                            case "+":
                                Movement.WalkY = Movement.MovementPosition.Forward;
                                break;

                            case "-":
                                Movement.WalkY = Movement.MovementPosition.Backward;
                                break;
                        }
                    }
                    break;

                case "z":
                    {
                        switch (input.Substring(2))
                        {
                            case "/":
                                Movement.WalkZ = Movement.MovementPosition.None;
                                break;

                            case "+":
                                Movement.WalkZ = Movement.MovementPosition.Forward;
                                break;

                            case "-":
                                Movement.WalkZ = Movement.MovementPosition.Backward;
                                break;
                        }
                    }
                    break;

                case "s":
                    foreach (MinecraftClient Client in Clients)
                    {
                        Client.SendEntityAction(Client.GetLocalPlayer().IsSneaking ? EntityActionPacket.Action.StopSneaking : EntityActionPacket.Action.StartSneaking);
                    }
                    break;
            }
        }

        private static void HandleDebugInput(string input)
        {
            string InputStart = input.Split(' ')[0];
            switch (InputStart.ToLower())
            {
                case "n":
                    foreach (MinecraftClient Client in Clients)
                    {
                        Task.Run(() => Client.APIClient.ChangeName(input.Substring(2)));
                    }
                    break;

                case "s":
                    foreach (MinecraftClient Client in Clients)
                    {
                        Task.Run(() => Client.APIClient.ChangeSkin(input.Substring(2), true));
                    }
                    break;

                case "o":
                    Task.Run(() => ServerHandler.FetchSearchList());
                    break;
            }
        }
    }
}
