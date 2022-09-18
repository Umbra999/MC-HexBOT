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
            "XBL3.0 x=15468014319842099618;eyJlbmMiOiJBMTI4Q0JDLUhTMjU2IiwiYWxnIjoiUlNBLU9BRVAiLCJjdHkiOiJKV1QiLCJ6aXAiOiJERUYiLCJ4NXQiOiJzYVkzV1ZoQzdnMmsxRW9FU0Jncm9Ob2l3MVEifQ.4N158f3WcZcWuKjO77lynKufq9xfvhbRFqUfYzJxAHbZXJ09jqSw_X9CAtvK78sWlknBHY8SuJBEBZklbjLXvsPVFNCEu62lN7BQSWlnNXHcXZ4FkGYjgHegWyttp3GpVawWHFOdqHgv5NfRb1yCfUUELylDoVRZYkprm1a1WpynvJ4va0WPSHuDAtIesqk6nOxINGzDcxPPVB_MAW7QQvwbXEq9rRv6nTM3OSmUxqCNq2WdSiNn5jCKoIhZZYRHD7KD9rQlyb4Vx6X8j7zns0o7wf_vCJe3wHT8TmiRRURLUBsTRo8ZDfLtg0YFaL4D2RqQkCLULjHWJGNb_j6OdA.AQcUMNybt3bL0FukzvZGwg.OG7r_Pg7t3i6V87NB8j8N0XtXgRn5Wk5oKeQ3pg9di0xxaJ0DKDKLaTVZD9Nwgf-rvma0FpqL-63ba7HcoQAR1emJ0T67Hs8lMxfrK6vbpq4Cf3_FbRBSjZ0h-zoM-8RdKcTgFCVhXGJ4BjzXsh3S_5KWxf7U53iUJx1exY7txrwOQdcJv_gIK8ZJB-LYxPuH9dM8nyza36-1YzoCdEjm8IKLXppI84jSCHV-8fqz1pBsoRqgaxn3qJbBUvs0ifeAfsbPgE34t1TkAgne-1LhdR2Jm9WoDvoPWNeBCKzX13Wd8pXilFKVj1FxDCTZtjtl7p5ql3dqZftPJhUpE8PlXc9OdAPNJdoTjTsgOSZH6V5KDy8rQasITO6Qr3nuo8oxoHiVMoSXI3bYyGe9EFEFNHZbiays-JoB9CtGBxmA_s7GjjMSJuwiENjkoIe3XB4-l0A1hH-eKba1LutpQPiJJKwNhkLWMpdycezoVVjm7QXBGLLJrLR2cvnm4AYUQQw0ORcOcUPXjriMiY7eKPfxK16PNDLPwYWqESlSdNxWKLEZonIGVyu5ANcF0qtigIvyJaozK1QmSyZ_e8tzGT6aLAi5DgAw61BNBXOR_roND_L--dxZ42DoUofSVaSKRbN-mPNvArp_XOvbGEBYhlqmK_WUmGs1_hbHxfTj2xn9L-cqabakWUkO1x-AC_JlTspLntGSe-8X4YWAGAr4ag0L4SgSm_jog0fwNsD5YrCozLQuakkHQDQVHACiJfSHG6_Mk8VLbwd_GqjidVao0rfEzSUi9EIHtbfJwQaDnGkrhBXKpkI1UvfbK_5WbTbpJWG4dpE_cP4BzfkT5u5V9YxDQUZ6CZVEq525XWUpQdIN0cdmw6ROZQZt9gj8oWXFSzTXWAxwf_mNiVUyuXhgzZPQiJFYa0ChQQwM-7Awh1mR8kHSrbs1wKopF9U5qfOWv8aDDq-NFGggFzS4z7gNXbZnjmMZTx-ZfSPKKEiepRJxunCR7ZtWF-8a8fDwX9gqLZioYX52yqkP1kiZ1OB28Eu4mLbqCnVM4u2PacdKx1QlgX8yRBFAR5bI5omHQfeZjG3cx4ZZkDoNAApAC3KLRNrapEmd-y0QQ5ueNZ8jDEkaUh1Xwd-zsF3XWVOqLliAPxp_qRMrkj8CkopGvMNAzMcHk-q9MdYDjmSsAf5hKW5OWwX2P7SLWK6yYKcANh6_bjOJLXB-_1EH1zNlwI1nY-6YGGF3wlC9EgWCnEffBvUQLV2FbGow8qb8U4OKndww1saesX7tEd_4GDkyxJrPQHUbPPTE4_UUFy3tb_kKW1pgmqQnhGmzREEKq7H8zr-8tVkeMEsrVZL0ymtKwJXHt9BbELPNjSv3wgqopqA6YGUa5Nj0_R2psjFYb2f-057JNzaYpzR_zxMiyUFMkXgV_Rx_TqxWcmeWsiPZvaUY5CqIWp1_9xSCUAf0vTNvySgkLtOV-FBaaHr_J4UvQRskBAsucGVt_6sZcXCZbZwxIwOIH2_5X760ERJmMl8fIEykasgUhth4rnvknaeLy2tbCqyXsB2GZgrDFbM7nbN396FtgRhlayGoXquA2bBHqrAIh6H.tv_Yn1UgZWasDU5b3JJfCA",
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

            CreateLaby();
            //CreateBots();
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
            Console.Title = $"HexBOT | {Clients.Count} Bots";
        }

        private static async void CreateLaby()
        {
            foreach (string Token in AccountTokens)
            {
                APIClient Client = new();
                if (await Client.Login(Token))
                {
                    LabyClient LabySession = new(Client);
                    LabySession.Connect("chat.labymod.net", 30336);
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
                Logger.LogImportant("J [IP:PORT] - Join a Server");
                Logger.LogImportant("C [MESSAGE] - Send a Chat Message");
                Logger.LogImportant("-----------------");
                Logger.LogImportant("S [true / false] - Skinblinker");
                Logger.LogImportant("T [true / false] - Teabagger");
                Logger.LogImportant("K [true / false] - Sneak");
                Logger.LogImportant("O [NAME] - Target the Player");
                Logger.LogImportant("-----------------");
                Logger.LogImportant("F [Name] - Follow a Player");
                Logger.LogImportant("X [+,-,/] - Move the X Cordinate");
                Logger.LogImportant("Y [+,-,/] - Move the Y Cordinate");
                Logger.LogImportant("Z [+,-,/] - Move the Z Cordinate");
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

                case "k":
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

                case "f":
                    Movement.CopyMovementTarget = input.Substring(2);
                    break;

                case "o":
                    Combat.ToggleAttack(input.Substring(2));
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
            }
        }
    }
}
