using MCHexBOT.Core;
using MCHexBOT.Features;
using MCHexBOT.HexServer;
using MCHexBOT.Packets.Server.Play;
using MCHexBOT.Protocol;
using MCHexBOT.Utils;
using Newtonsoft.Json.Linq;
using System.Numerics;

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

            if (!File.Exists("Accounts.txt"))
            {
                Logger.LogError("No Accounts found");
                Thread.Sleep(5000);
                return;
            }

            foreach (string Account in File.ReadAllLines("Accounts.txt"))
            {
                string Token = XboxAuth.Microsoft.GetXboxToken(Account.Split(':')[0], Account.Split(':')[1]);

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
                        Logger.LogImportant("F [Name] - Walk to Player");
                        Logger.LogImportant("L [N / E / S / W] - Look at the Direction");
                        Logger.LogImportant("M [N / E / S / W] - Move at the Direction");
                        Logger.LogImportant("H - Hit Animation");
                        Logger.LogImportant("S - Toggle Sneak");
                        Logger.LogImportant("R - Toggle Sprint");
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
                        Task.Run(() => Client.Connect(Server.Split(':')[0], Server.Split(':').Length > 1 ? Convert.ToInt32(Server.Split(':')[1]) : 25565));
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
                    Movement.FollowTarget = input.Substring(2);
                    foreach (MinecraftClient Client in Clients)
                    {
                        Task.Run(() => Movement.LoopPlayerMovement(Client));
                    }
                    break;

                case "w":
                    foreach (MinecraftClient Client in Clients)
                    {
                        Vector3 Target = Client.Players.Where(x => x.PlayerInfo.Name == input.Substring(2)).First().Position;
                        Task.Run(() => Movement.MoveToPosition(Client, Target));
                    }
                    break;

                case "l":
                    {
                        switch (input.Substring(2).ToLower())
                        {
                            case "n":
                                foreach (MinecraftClient Client in Clients)
                                {
                                    Movement.LookAtDirection(Client, Direction.North);
                                }
                                break;

                            case "e":
                                foreach (MinecraftClient Client in Clients)
                                {
                                    Movement.LookAtDirection(Client, Direction.East);
                                }
                                break;

                            case "s":
                                foreach (MinecraftClient Client in Clients)
                                {
                                    Movement.LookAtDirection(Client, Direction.South);
                                }
                                break;

                            case "w":
                                foreach (MinecraftClient Client in Clients)
                                {
                                    Movement.LookAtDirection(Client, Direction.West);
                                }
                                break;
                        }
                    }
                    break;

                case "m":
                    switch (input.Substring(2).ToLower())
                    {
                        case "n":
                            foreach (MinecraftClient Client in Clients)
                            {
                                Vector3 Target = Client.GetLocalPlayer().Position + Movement.CalculateDirections(Direction.North);
                                Task.Run(() => Movement.MoveToPosition(Client, Target));
                            }
                            break;

                        case "e":
                            foreach (MinecraftClient Client in Clients)
                            {
                                Vector3 Target = Client.GetLocalPlayer().Position + Movement.CalculateDirections(Direction.East);
                                Task.Run(() => Movement.MoveToPosition(Client, Target));
                            }
                            break;

                        case "s":
                            foreach (MinecraftClient Client in Clients)
                            {
                                Vector3 Target = Client.GetLocalPlayer().Position + Movement.CalculateDirections(Direction.South);
                                Task.Run(() => Movement.MoveToPosition(Client, Target));
                            }
                            break;

                        case "w":
                            foreach (MinecraftClient Client in Clients)
                            {
                                Vector3 Target = Client.GetLocalPlayer().Position + Movement.CalculateDirections(Direction.West);
                                Task.Run(() => Movement.MoveToPosition(Client, Target));
                            }
                            break;
                    }
                    break;

                case "h":
                    foreach (MinecraftClient Client in Clients)
                    {
                        Client.SendAnimation(AnimationPacket.HandType.Main);
                    }
                    break;

                case "s":
                    foreach (MinecraftClient Client in Clients)
                    {
                        Client.SendEntityAction(Client.GetLocalPlayer().IsSneaking ? EntityActionPacket.Action.StopSneaking : EntityActionPacket.Action.StartSneaking);
                    }
                    break;

                case "r":
                    foreach (MinecraftClient Client in Clients)
                    {
                        Client.SendEntityAction(Client.GetLocalPlayer().IsSprinting ? EntityActionPacket.Action.StopSprinting : EntityActionPacket.Action.StartSprinting);
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
