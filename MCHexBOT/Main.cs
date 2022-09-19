using MCHexBOT.Core;
using MCHexBOT.Features;
using MCHexBOT.HexServer;
using MCHexBOT.Protocol;
using MCHexBOT.Utils;

namespace MCHexBOT
{
    internal class Main
    {
        public static List<MinecraftClient> Clients = new();
        public static List<string> AccountTokens = new()
        {
            "XBL3.0 x=6453272288188589253;eyJlbmMiOiJBMTI4Q0JDLUhTMjU2IiwiYWxnIjoiUlNBLU9BRVAiLCJjdHkiOiJKV1QiLCJ6aXAiOiJERUYiLCJ4NXQiOiJzYVkzV1ZoQzdnMmsxRW9FU0Jncm9Ob2l3MVEifQ.pyrqKLW9c_7cf10Lb4K5EkiRmTvu5qVU8ffR61I5-xU0-WFNCO3ojhlXEu1-nBr1JSXiPH-hxQAbYIH36uFNPXVh7g3TKzjdYAjvXBD0GbihgLZrbROP-GOOxBkiv9WJ5vOoOHnm0FGJZu5GqXs4G3CpA1tZueAjQ5MnmBmtu9m_paOvpM4tI_cpDVFgcmRIAfgro8GC9GdeL5xn5ZhyXeJ8PqXBxWT7y6EBQ4joQtZyWHPrQwJBwjZxIkht5kFd-JCvTRdaQkmPoRcVOXbEF-hR25uw1owr74tVsJrDEyX1KfTlXJzevVMcHObL6_gOQxQgGUBjL2ZNjwJBZ0CIpw.Nu65t1s7YC4SiZB4BJglOA.ufFf2sjmGNSse4ch2kA1NG69UDZCNN94erfyGMr_w95v-4NLILv27MzeNdEE8oLCkIyQKkosB9IEc9FrCBU3xGHVLDN2utdb0owQke8BsfKsZic42rOrarmbkqmsgkDJBbuyZ_2tqJz067CaUKmR3vmdfyiCq3gIQncBL2B0NQQtJcIJYjhgVAWORdwBF5QbLaPw7iIb-XbrJBJ0DkW5ZcaCaLU8ky6zJ5Et4lDXuY45piYsRNaIsujGPIheUqPVIdCXPgmo8FvS2teDI-jPEdWrVgZjm-lC9R7J1GLv_eyQMUFk1WftVVGQJcb7ARk3-ggTV4o_nw4lkv7Oj9HCKQPghVGk2dZp-mSZvl0snGyCrNyQtkf8HmZ70MshQ07ONR5HMwt5xwATnxddIRlcWMHlDaMqIGOtftoxJ1Va1JpXx8WVLRzhykFJZbD_Z_J62SN_yWibmFv5KnLr7pOSwBdQWOfWp0uuE7U_3HDREax6YkfjPTxOWNyrplfWo8q_5AsUCNC1y-IWuQZwuTAFGiBqqpQ98YJfUZCLZt7ckZxJ595-38HMx_fsEu5nC2b95g38iB-kVQRmoUUnB_8WgmFhxYb-2OO99S-LBksikCEdReZABv4b_jYwZC2Z9Vz7_QvWcS0bQmhHJjCJmpuSe72FGFnyFD98j3gsp00xGhFmv6jn2sYgdq-fNnDpQJkJJCFJy_GJ8WaQx_DPcCAzpRDEX9IJS1tz2pQ9JEXs0wqaH1HEwFnTJkXLv_QzsT4o0C_SojL1zGzAtfYwKXFvBRHvYWqHuuq8sku1rIIG9wVk6CojgqrdgwGrieuQ5WRbbhrPgzO3-PmNlACrD8lg27PzXGTEcnTBVNv6PPokkDRefyOozvHbokO26krWoBB2wfLDQg9gyxKG1YYYEsozCZ0k6UCEihVuWV41s5rPIuJwqGu5x6TncyiQfKZXeY8MzcbEUtZNaMawMasltE5rbw4VROx17h0v840mdH6exSspXHsfHdxNh4ZOq6T_yE9KvXZt-ZY-7uykQRobbAifUmlKm4-ftW8itvrfo8Urzw_4G-XPBBjg3-yn7WA1QHjKzp8QH3N0TKboHX6koplM0Yb_beB8yJ4DPYWAXm5OUFL0zIPEctjWLaBxkMCBGMYU70jSp8c5DLIWk6t0CTjp22dly_99oYrkyXwYwxeInlQgbWY7dajF5a3nqapZi0zvsLloSmMja0cNOd4RlwmX13TNM3sUrvT-BVwclp4mXPLH62dLZXIscNPlulXqM-gpiYJ5EWMxNDTUZVDY_TYK2OI5RpRKcTl9YwmgqRRmIzDzgpzVkos6GyD71USi5HagO4HqtP6dp0N-tvoVWE0sJtuwMmC3jKNo_gDU3qSOcP8UbEXULNNIHGuHGmqVRZiiCLQ9CaCHo2RJRI1ZgyZilOu3XdMw8Gk6dMRF1EPwtNImaMkqUwaPAPIpRw2v1mxBrD-VOaDGn9paLOx9AxzwSa8yws2Z6kAI8qA7jn-2vXbSyIuMSGUq7ihuYXY0SYE5paso6hpDw0YcHI90nzvCL2HSWyVP8O8cMGQ1yF2IOFeseflSCj2WvIVCBZ9aIrS-.BejSmusYquDN9kJp-pUWNw"
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

            ServerHandler.Init();
            CreateBots();
            RunGUI();
        }

        private static async void CreateBots()
        {
            foreach (string Token in AccountTokens)
            {
                APIClient Client = new();
                if (await Client.LoginToMinecraft(Token))
                {
                    //new LabyClient(Client);
                    Clients.Add(new MinecraftClient(Client, null));
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
                Logger.LogImportant("SAY [MESSAGE] - Send a Chat Message");
                Logger.LogImportant("-----------------");
                Logger.LogImportant("SB [true / false] - Skinblinker");
                Logger.LogImportant("TB [true / false] - Teabagger");
                Logger.LogImportant("KILL [NAME] - Target the Player");
                Logger.LogImportant("-----------------");
                Logger.LogImportant("F [Name] - Follow a Player");
                Logger.LogImportant("X [+,-,/] - Move the X Cordinate");
                Logger.LogImportant("Y [+,-,/] - Move the Y Cordinate");
                Logger.LogImportant("Z [+,-,/] - Move the Z Cordinate");
                Logger.LogImportant("S [true / false] - Sneak");
                Logger.LogImportant("-----------------");
                Logger.LogImportant("CNAME [NAME] - Change the Name");
                Logger.LogImportant("CSKIN [URL] - Change the Skin");
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

                case "say":
                    foreach (MinecraftClient Client in Clients)
                    {
                        Client.SendChat(input.Substring(2));
                    }
                    break;

                case "sb":
                    foreach (MinecraftClient Client in Clients)
                    {
                        SkinBlinker.ToggleSkinBlinker(Client, input.Substring(2) == "true");
                    }
                    break;

                case "tb":
                    foreach (MinecraftClient Client in Clients)
                    {
                        TeaBagger.ToggleTeaBagger(Client, input.Substring(2) == "true");
                    }
                    break;

                case "kill":
                    Combat.ToggleAttack(input.Substring(2));
                    break;

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
                        Client.SendEntityAction(input.Substring(2) == "true" ? PlayerAction.StartSneaking : PlayerAction.StopSneaking);
                    }
                    break;

                case "cname":
                    foreach (MinecraftClient Client in Clients)
                    {
                        Task.Run(() => Client.APIClient.ChangeName(input.Substring(2)));
                    }
                    break;

                case "cskin":
                    foreach (MinecraftClient Client in Clients)
                    {
                        Task.Run(() => Client.APIClient.ChangeSkin(input.Substring(2), true));
                    }
                    break;
            }
        }
    }
}
