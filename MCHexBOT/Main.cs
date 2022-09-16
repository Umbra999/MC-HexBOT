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
            "XBL3.0 x=3482949924638196216;eyJlbmMiOiJBMTI4Q0JDLUhTMjU2IiwiYWxnIjoiUlNBLU9BRVAiLCJjdHkiOiJKV1QiLCJ6aXAiOiJERUYiLCJ4NXQiOiJzYVkzV1ZoQzdnMmsxRW9FU0Jncm9Ob2l3MVEifQ.wgqPhA2DcyZr5VwSpYCzi965ELlMQEkjm9tQFgbc5fZ08FpKoBCrKlrA90aQ2_xS9ql9eyZ2We0FYgTeqmwKs9DEOUK6pm9AzrZiyXoB9orqaKqdXVf1sL_Hywk7ALKku9EQoajtU5Bai4b48WUlq2nmqmbehvwMbvx-OAQ_xw1SNIXUNL62RucwBTuDHX2CQDo1mLK0Pws4JtDKh7FFml_V_vY6MYNoY4JS52xGZcUbu1Vb9BkRELOdNtWlI_nz96D66Qng6hGHzMohR973bBFKuljRkAPS_ecppZpi1cpBOumEENipo31gC93LChA5VFRYqgv99LWlMwk7xU6b9g.OVUCBVeZiPHumq9_wNm_1Q.9VAv5SBHTZHqhTGeOU6Wj1rRUZ_QJG5zwGefg-Ya91X4JSKFBwpzU1S7WrDrpCI_zk-Mhq6VRFv8S3v1co7Eytq-VCUpNmIQuWX2sC9wpSY5s-IQWM4RgsJ0ZZjJOtxqXDT7ZHg212noifNHOiqXor6vtzmEO8XbX0qp0-AjP6CspzWYuf8oMbJ2mcK9RpW6R2p54_X0-7tet7iRpuKId90TXqMRGQiM6EzFR3evHomZ0eo_VNye3tE4rSNKZifLGqMNKk5ruVGxaUsXe5eWZxk_6q01jOxPyeMNykNaoWtpCb7dC6ZXwSgFHGtIwINPDwUIPmwoPmwp_EPkQuP9ANF3Af4EKwkpKpwfTlxc5WNol2V67N2TLENgTry5xiIPMwEw1w3rly6cLdkUVp6Ue74nB5kAYnsJEdjw_lkkfo2sz11sGyYC-cQ944jJgZPoV4Dypz9v1i4pK0CVdhJsN35WHTf6XgOhbmDcaST_XPcAo1jswUdCsSfoGw1jMOxZURRQhCp1k_fkV98eCvgIb7AQ5eSzBeiTm-BvWO9bHiB8c5zdh0Iw5HphBKX6WSaw9N7oDaPp4Zrcqg0_WLhNuoZPiBhp14MvCWkq6LO9wCBvBUY0vB_MoNDNLNBpvDE3I1NnfSDEOgvj-C_Pmdu2_GyK6hmLRbU77-A2otT7fzNe38d-GKuzfFIP3bPvMfRUzNR1hIMKA8472Lco7TdL5UXOzF6ftDznA46El5Po1atZRsR3Hqb3bzzj6L9aLkFjD3iDYRUrssb58-jjlO0fxDBG7dSP3jXjEqX0busooZ5uNdLUPsamNH2EYZ2yeAK4WJgOqUaJXFTDUEB1yi7XCgzIJ9vK2BSzmFEG8ybsHNZFmrM9w0cbP7IJOG7hN6U0DPySbafNYmedJVe5aV3Wqhbe20nwR2__ToW4hLY8ai5MPkcqdkBBfOm2aZj4BhHnfqoRguBloCIa-Ae8X73LJKV6Qj4ESFQt9v7dgsYBv-n75GwsD3dC0hNzC8JGSztSA4f6vFY3TmDPfXRO3391hSGfsTtx2tOK8_qhd81lzZSRtV-ADire5pZ6EnmW9TcFW5ldUgG3hR9f-DU8SwXQceOWpdAkUD3_YSLUNTMig6r_oXpJ7kxi8UZau7NPr3jMA1nCIDQocQjbxWPtSUI_f-vdcZdXpU97mXfsltG1mLziI0b6QlkVgR54z2A-lO--BxG_SECIh6kmsq656n0-f7IhJJdLlzZzQVNEYd9jFdI1lJoVJhliWXNOgNLGjf73WF-GX6PWhdOagSvuASvbMZOHmRj4ezAiTipVw2X-B_r4FnsR2nZuitGeiN9U26CBwsB2EqN8tB520kHXSlCHQmOEbC7jHSdbK7M1Nop8ZVP6hqfc-pnuC-A3HDm5P1OuANkPCitQCIswojkBIkISQxcLL0cTK6eQGWijUJjl8BjQY7GrDbZ8I_BlKptr_3l8csf17ZqHwZ25yLBQzCmaQ-Cmgr_2OzdWIe8fjHS10n6-xE6sm3NWBE76Z9wnzRyRnC4oIleydJ-XY4HcQ74IYv_ZxMkXH9DlxQ6BBn990DE12Y29j6LckDmdFuaZX2rn.GpOwQ_Kditfmc1-P39cz_g",
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
