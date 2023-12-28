using HexBOT.Utils;
using System.Text;
using System.Text.Json;

namespace HexBOT.HexedServer
{
    internal class ServerHandler
    {
        public static Dictionary<string, string> OverseeUsers = new();
        private static string Token;

        public static void Init(string Key)
        {
            Encryption.ServerThumbprint = EncryptUtils.FromBase64(FetchCert().Result);
            Encryption.EncryptionKey = EncryptUtils.FromBase64(FetchEncryptionKey().Result);
            Encryption.DecryptionKey = EncryptUtils.FromBase64(FetchDecryptionKey().Result);

            Token = Key;
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

        private static async Task<string> FetchEncryptionKey()
        {
            HttpClient Client = new(new HttpClientHandler { UseCookies = false, ServerCertificateCustomValidationCallback = Encryption.ValidateServerCertificate });
            Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Hexed)");

            HttpRequestMessage Payload = new(HttpMethod.Get, "https://api.logout.rip/Server/EncryptKey");
            HttpResponseMessage Response = await Client.SendAsync(Payload);
            if (Response.IsSuccessStatusCode) return await Response.Content.ReadAsStringAsync();
            return null;
        }

        private static async Task<string> FetchDecryptionKey()
        {
            HttpClient Client = new(new HttpClientHandler { UseCookies = false, ServerCertificateCustomValidationCallback = Encryption.ValidateServerCertificate });
            Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Hexed)");

            HttpRequestMessage Payload = new(HttpMethod.Get, "https://api.logout.rip/Server/DecryptKey");
            HttpResponseMessage Response = await Client.SendAsync(Payload);
            if (Response.IsSuccessStatusCode) return await Response.Content.ReadAsStringAsync();
            return null;
        }

        public static async Task FetchSearchList()
        {
            OverseeUsers.Clear();

            HttpClient Client = new(new HttpClientHandler { UseCookies = false, ServerCertificateCustomValidationCallback = Encryption.ValidateServerCertificate });
            Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Hexed)");

            HttpRequestMessage Payload = new(HttpMethod.Post, "https://api.logout.rip/Minecraft/GetOverseeList")
            {
                Content = new StringContent(DataEncryptBase.EncryptData(JsonSerializer.Serialize(new { Key = Token, HWID = Encryption.GetHWID(), ServerTime = EncryptUtils.GetUnixTime() }), Encryption.EncryptionKey), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage Response = await Client.SendAsync(Payload);
            if (Response.IsSuccessStatusCode)
            {
                string EncryptedData = await Response.Content.ReadAsStringAsync();
                string RawData = DataEncryptBase.DecryptData(EncryptedData, Encryption.DecryptionKey);
                string[] UsersWithTags = RawData.Trim('\n', '\r', ' ').Split(new[] { Environment.NewLine }, StringSplitOptions.None);

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
                Content = new StringContent(DataEncryptBase.EncryptData(JsonSerializer.Serialize(new { Key = Token, HWID = Encryption.GetHWID(), ServerTime = EncryptUtils.GetUnixTime(), Payload = EmbedPayload }), Encryption.EncryptionKey), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage Response = await Client.SendAsync(Payload);

            return Response.IsSuccessStatusCode;
        }
    }
}
