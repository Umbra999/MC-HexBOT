using HexBOT.Utils;
using Newtonsoft.Json;
using System.Text;

namespace HexBOT.HexedServer
{
    internal class ServerHandler
    {
        public static Dictionary<string, string> OverseeUsers = new();

        private static ServerObjects.UserData UserData;

        public static async Task Init()
        {
            Logger.Log("Authenticating...");
            if (!File.Exists("Key.Hexed"))
            {
                Logger.LogWarning("Enter Key:");
                string NewKey = Console.ReadLine();
                File.WriteAllText("Key.Hexed", Encryption.ToBase64(NewKey));
            }

            Encryption.ServerThumbprint = Encryption.FromBase64(await FetchCert());
            Encryption.PublicEncryptionKey = Encryption.FromBase64(await FetchPublicKey());

            UserData = await Login(Encryption.FromBase64(File.ReadAllText("Key.Hexed")));

            if (UserData == null)
            {
                Logger.LogError("Key is not Valid");
                await Task.Delay(3000);
                Environment.Exit(0);
            }
        }

        private static async Task<string> FetchCert()
        {
            HttpClient Client = new(new HttpClientHandler { UseCookies = false });
            Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Hexed)");

            HttpRequestMessage Payload = new(HttpMethod.Get, "https://api.logout.rip/Server/Certificate");
            HttpResponseMessage Response = await Client.SendAsync(Payload);
            if (Response.IsSuccessStatusCode) return await Response.Content.ReadAsStringAsync();
            return null;
        }

        private static async Task<string> FetchPublicKey()
        {
            HttpClient Client = new(new HttpClientHandler { UseCookies = false, ServerCertificateCustomValidationCallback = Encryption.ValidateServerCertificate });
            Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Hexed)");

            HttpRequestMessage Payload = new(HttpMethod.Get, "https://api.logout.rip/Server/PublicKey");
            HttpResponseMessage Response = await Client.SendAsync(Payload);
            if (Response.IsSuccessStatusCode) return await Response.Content.ReadAsStringAsync();
            return null;
        }


        private static async Task<ServerObjects.UserData> Login(string Key)
        {
            HttpClient Client = new(new HttpClientHandler { UseCookies = false, ServerCertificateCustomValidationCallback = Encryption.ValidateServerCertificate });
            Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Hexed)");

            HttpRequestMessage Payload = new(HttpMethod.Post, "https://api.logout.rip/Server/Login")
            {
                Content = new StringContent(Encryption.EncryptData(JsonConvert.SerializeObject(new { Key = Key, HWID = Encryption.GetHWID(), ServerTime = Encryption.GetUnixTime() })), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage Response = await Client.SendAsync(Payload);

            if (Response.IsSuccessStatusCode)
            {
                string RawData = await Response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ServerObjects.UserData>(RawData);
            }

            return null;
        }

        public static async Task FetchSearchList()
        {
            OverseeUsers.Clear();

            HttpClient Client = new(new HttpClientHandler { UseCookies = false, ServerCertificateCustomValidationCallback = Encryption.ValidateServerCertificate });
            Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Hexed)");

            HttpRequestMessage Payload = new(HttpMethod.Post, "https://api.logout.rip/Minecraft/GetOverseeList")
            {
                Content = new StringContent(Encryption.EncryptData(JsonConvert.SerializeObject(new { Key = UserData.Token, HWID = Encryption.GetHWID(), ServerTime = Encryption.GetUnixTime() })), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage Response = await Client.SendAsync(Payload);
            if (Response.IsSuccessStatusCode)
            {
                string List = await Response.Content.ReadAsStringAsync();
                string[] UsersWithTags = List.Trim('\n', '\r', ' ').Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                for(int i = 0; i < UsersWithTags.Length; i++)
                {
                    string[] Users = UsersWithTags[i].Split(':');
                    OverseeUsers.Add(Users[0], Users[1]);
                }
                Logger.LogSuccess($"Fetched {OverseeUsers.Count} Oversee Users");
            }
        }

        public static void CheckOverseePlayer(string UUID, string Name, string Server)
        {
            if (OverseeUsers.ContainsKey(UUID))
            {
                int Color = 0;
                switch (OverseeUsers[UUID])
                {
                    case "User":
                        Color = 1752220;
                        break;

                    case "KOS":
                        Color = 10038562;
                        break;

                    case "Special":
                        Color = 7419530;
                        break;
                }

                Task.Run(() => SendOverseeEmbded(UUID, Name, Server, OverseeUsers[UUID], Color));
            }
        }

        private static async Task<bool> SendOverseeEmbded(string UUID, string Name, string Server, string Type, int Color)
        {
            Misc.DiscordEmbedField PlayerInfos = new()
            {
                name = "User",
                value = $"**{Name}** [**{UUID}**]"
            };

            Misc.DiscordEmbedField ServerInfos = new()
            {
                name = "Server",
                value = $"**{Server}**"
            };

            Misc.DiscordEmbedField[] Fields =
            [
                PlayerInfos,
                ServerInfos
            ];

            Misc.DiscordPayload EmbedPayload = new()
            {
                EncryptedURL = "MinecraftOversee",
                Public = false,
                Title = $"{Type} Found",
                Color = Color,
                Fields = Fields
            };

            HttpClient Client = new(new HttpClientHandler { UseCookies = false, ServerCertificateCustomValidationCallback = Encryption.ValidateServerCertificate });
            Client.DefaultRequestHeaders.Add("User-Agent", "Hexed");

            HttpRequestMessage Payload = new(HttpMethod.Post, "https://api.logout.rip/Server/Webhook")
            {
                Content = new StringContent(Encryption.EncryptData(JsonConvert.SerializeObject(new { Key = UserData.Token, HWID = Encryption.GetHWID(), ServerTime = Encryption.GetUnixTime(), Payload = EmbedPayload })), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage Response = await Client.SendAsync(Payload);

            return Response.IsSuccessStatusCode;
        }
    }
}
