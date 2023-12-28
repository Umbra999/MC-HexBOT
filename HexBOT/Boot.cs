using HexBOT.Core.API;
using HexBOT.Core.Minecraft;
using HexBOT.Features;
using HexBOT.HexedServer;
using HexBOT.Packets.Server.Play;
using HexBOT.Protocol;
using HexBOT.Utils;
using HexBOT.XboxAuth;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace HexBOT
{
    internal class Boot
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleOutputCP(uint wCodePageID);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCP(uint wCodePageID);

        public static List<MinecraftClient> Clients = new();

        public sealed class HexedEntry : Attribute { }

        [HexedEntry]
        public static void Main(string[] args)
        {
            if (args.Length != 1) return;

            ServerHandler.Init(args[0]);

            SetConsoleOutputCP(65001);
            SetConsoleCP(65001);
            Console.OutputEncoding = Encoding.GetEncoding(65001);
            Console.InputEncoding = Encoding.GetEncoding(65001);

            Task.Run(CreateBots).Wait();

            RunGUI();
        }

        private static async Task CreateBots()
        {
            if (!File.Exists("Accounts.txt"))
            {
                Logger.LogError("No Accounts found");
                await Task.Delay(3000);
                return;
            }

            int NeedsDelayCount = 0;
            foreach (string Account in File.ReadAllLines("Accounts.txt"))
            {
                NeedsDelayCount++;
                if (NeedsDelayCount == 4)
                {
                    Logger.LogWarning("Logged into to many Accounts, delaying for 60 seconds");
                    await Task.Delay(60000);
                    NeedsDelayCount = 1;
                }

                string Token = XboxLive.GetXboxToken(Account.Split(':')[0], Account.Split(':')[1]);
                if (Token == null)
                {
                    Logger.LogError($"Failed to Validate Xbox Login: {Account}");
                    continue;
                }

                APIClient Client = await APIClient.LoginToMinecraft(Token);
                if (Client == null || Client.CurrentUser == null)
                {
                    Logger.LogError($"Failed to Validate Minecraft Login: {Account}");
                    continue;
                }

                Clients.Add(new MinecraftClient(Client));
            }

            Console.Title = $"HexBOT - {Clients.Count} Bots | {EncryptUtils.RandomString(20)}";

            await Task.Delay(2500);
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
                Logger.LogImportant("5 - LABYMOD");
                Logger.LogImportant("-----------------");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Logger.LogImportant("-----------------");
                        Logger.LogImportant("J [IP:PORT] - Join a Server");
                        Logger.LogImportant("L - Leave a Server");
                        Logger.LogImportant("C [MESSAGE] - Send a Chat Message");
                        Logger.LogImportant("-----------------");
                        HandleCoreInput(Console.ReadLine());
                        break;

                    case "2":
                        Logger.LogImportant("-----------------");
                        Logger.LogImportant("Z - Toggle Skinblinker");
                        Logger.LogImportant("T - Toggle Teabagger");
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
                        Logger.LogImportant("D - Dump all Players");
                        Logger.LogImportant("-----------------");
                        HandleDebugInput(Console.ReadLine());
                        break;

                    case "5":
                        Logger.LogImportant("-----------------");
                        Logger.LogImportant("F [NAME] - Friend the Player");
                        Logger.LogImportant("U [NAME] - Unfriend the Player");
                        Logger.LogImportant("-----------------");
                        HandleLabyInput(Console.ReadLine());
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

                case "l":
                    foreach (MinecraftClient Client in Clients)
                    {
                        Client.Disconnect();
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
                        SkinBlinker.ToggleSkinBlinker(Client);
                    }
                    break;

                case "t":
                    foreach (MinecraftClient Client in Clients)
                    {
                        TeaBagger.ToggleTeaBagger(Client);
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
                case "j":
                    foreach (MinecraftClient Client in Clients)
                    {
                        Task.Run(() => Movement.Jump(Client));
                    }
                    break;
                case "w":
                    foreach (MinecraftClient Client in Clients)
                    {
                        //Vector3 Target = Client.EntityManager.AllPlayers.Where(x => x.PlayerInfo.Name == input.Substring(2)).First().Position;
                        //Task.Run(() => Movement.MoveToPosition(Client, Target));
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
                                Vector3 Target = Client.EntityManager.LocalPlayer.Position + Movement.CalculateDirections(Direction.North);
                                Task.Run(() => Movement.MoveToPosition(Client, Target));
                            }
                            break;

                        case "e":
                            foreach (MinecraftClient Client in Clients)
                            {
                                Vector3 Target = Client.EntityManager.LocalPlayer.Position + Movement.CalculateDirections(Direction.East);
                                Task.Run(() => Movement.MoveToPosition(Client, Target));
                            }
                            break;

                        case "s":
                            foreach (MinecraftClient Client in Clients)
                            {
                                Vector3 Target = Client.EntityManager.LocalPlayer.Position + Movement.CalculateDirections(Direction.South);
                                Task.Run(() => Movement.MoveToPosition(Client, Target));
                            }
                            break;

                        case "w":
                            foreach (MinecraftClient Client in Clients)
                            {
                                Vector3 Target = Client.EntityManager.LocalPlayer.Position + Movement.CalculateDirections(Direction.West);
                                Task.Run(() => Movement.MoveToPosition(Client, Target));
                            }
                            break;
                    }
                    break;

                case "h":
                    foreach (MinecraftClient Client in Clients)
                    {
                        Client.SendAnimation();
                    }
                    break;

                case "s":
                    foreach (MinecraftClient Client in Clients)
                    {
                        Client.SendEntityAction(Client.EntityManager.LocalPlayer.IsSneaking ? EntityActionPacket.Action.StopSneaking : EntityActionPacket.Action.StartSneaking);
                    }
                    break;

                case "r":
                    foreach (MinecraftClient Client in Clients)
                    {
                        Client.SendEntityAction(Client.EntityManager.LocalPlayer.IsSprinting ? EntityActionPacket.Action.StopSprinting : EntityActionPacket.Action.StartSprinting);
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

                case "d":
                    foreach (MinecraftClient Client in Clients)
                    {
                        foreach (Player p in Client.EntityManager.AllPlayers)
                        {
                            Console.WriteLine("=================");
                            Console.WriteLine("Name: " + p.Name);
                            Console.WriteLine("UUID: " + p.UUID);
                            Console.WriteLine("Ping: " + p.Ping);
                            Console.WriteLine("Position: " + p.Position);
                            Console.WriteLine("Rotation: " + p.Rotation);
                            Console.WriteLine("Velocity: " + p.Velocity);
                            Console.WriteLine("=================");
                        }
                    }
                    break;
            }
        }

        private static void HandleLabyInput(string input)
        {
            string InputStart = input.Split(' ')[0];
            switch (InputStart.ToLower())
            {
                case "f":
                    foreach (MinecraftClient Client in Clients)
                    {
                        Client.LabyClient.MCConnection.SendPacket(new Protocol.Packets.LabyServer.Play.FriendRequestPacket()
                        {
                            Name = input.Substring(2)
                        });
                    }
                    break;

                case "u":
                    foreach (MinecraftClient Client in Clients)
                    {
                        Client.LabyClient.MCConnection.SendPacket(new Protocol.Packets.LabyServer.Play.FriendRemovePacket()
                        {
                            Name = input.Substring(2)
                        });
                    }
                    break;
            }
        }
    }
}
