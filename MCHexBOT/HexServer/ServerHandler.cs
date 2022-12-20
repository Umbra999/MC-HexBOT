using MCHexBOT.Utils;
using Newtonsoft.Json;
using System.Text;

namespace MCHexBOT.HexServer
{
    internal class ServerHandler
    {
        private static string Key = "";
        private static string HWID = "";
        public static Dictionary<string, string> OverseeUsers = new();

        public static async Task Init()
        {
            Logger.Log("Authenticating...");
            if (!File.Exists("Key.Hexed"))
            {
                Logger.LogWarning("Enter Key:");
                string NewKey = Console.ReadLine();
                File.WriteAllText("Key.Hexed", Encryption.ToBase64(NewKey));
            }

            Key = Encryption.FromBase64(File.ReadAllText("Key.Hexed"));
            HWID = Encryption.GetHWID(); 

            if (!await IsValidKey())
            {
                Logger.LogError("Key is not Valid");
                await Task.Delay(3000);
                Environment.Exit(0);
            }

            await FetchSearchList();
        }

        private static async Task<string> FetchTime()
        {
            HttpClient Client = new(new HttpClientHandler { UseCookies = false });
            Client.DefaultRequestHeaders.Add("User-Agent", "Hexed");

            HttpRequestMessage Payload = new(HttpMethod.Get, Encryption.FromBase64("aHR0cDovLzYyLjY4Ljc1LjUyOjk5OS9TZXJ2ZXIvVGltZQ=="));
            HttpResponseMessage Response = await Client.SendAsync(Payload);
            if (Response.IsSuccessStatusCode) return await Response.Content.ReadAsStringAsync();
            return null;
        }

        private static async Task<bool> IsValidKey()
        {
            string Timestamp = await FetchTime();

            HttpClient Client = new(new HttpClientHandler { UseCookies = false });
            Client.DefaultRequestHeaders.Add("User-Agent", "Hexed");

            HttpRequestMessage Payload = new(HttpMethod.Post, Encryption.FromBase64("aHR0cDovLzYyLjY4Ljc1LjUyOjk5OS9TZXJ2ZXIvSXNWYWxpZA=="))
            {
                Content = new StringContent(JsonConvert.SerializeObject(new { Auth = Encryption.EncryptAuthKey(Key, Timestamp, "XD6V", HWID) }), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage Response = await Client.SendAsync(Payload);

            if (Response.IsSuccessStatusCode)
            {
                return Convert.ToBoolean(await Response.Content.ReadAsStringAsync());
            }
            return false;
        }

        public static async Task FetchSearchList()
        {
            OverseeUsers.Clear();

            string Timestamp = await FetchTime();

            HttpClient Client = new(new HttpClientHandler { UseCookies = false });
            Client.DefaultRequestHeaders.Add("User-Agent", "Hexed");

            HttpRequestMessage Payload = new(HttpMethod.Post, Encryption.FromBase64("aHR0cDovLzYyLjY4Ljc1LjUyOjk5OS9NaW5lY3JhZnQvR2V0T3ZlcnNlZUxpc3Q="))
            {
                Content = new StringContent(JsonConvert.SerializeObject(new { Auth = Encryption.EncryptAuthKey(Key, Timestamp, "UX2V", HWID) }), Encoding.UTF8, "application/json")
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
            string Timestamp = await FetchTime();

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

            Misc.DiscordEmbedField[] Fields = new Misc.DiscordEmbedField[]
            {
                PlayerInfos,
                ServerInfos
            };

            Misc.DiscordPayload EmbedPayload = new()
            {
                EncryptedURL = "MinecraftOversee",
                Public = false,
                Title = $"{Type} Found",
                Color = Color,
                Fields = Fields
            };

            HttpClient Client = new(new HttpClientHandler { UseCookies = false });
            Client.DefaultRequestHeaders.Add("User-Agent", "Hexed");

            HttpRequestMessage Payload = new(HttpMethod.Post, Encryption.FromBase64("aHR0cDovLzYyLjY4Ljc1LjUyOjk5OS9TZXJ2ZXIvV2ViaG9vaw=="))
            {
                Content = new StringContent(JsonConvert.SerializeObject(new { Auth = Encryption.EncryptAuthKey(Key, Timestamp, "OLKX", HWID), Payload = JsonConvert.SerializeObject(EmbedPayload) }), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage Response = await Client.SendAsync(Payload);
            if (Response.IsSuccessStatusCode) return true;
            return false;
        }
    }
}
