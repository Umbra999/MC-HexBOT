using HexBOT.Protocol.Utils;
using HexBOT.Utils;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace HexBOT.Core.API
{
    public class APIClient
    {
        private HttpClient MinecraftClient;
        private HttpClient LabyClient;

        public SelfAuthUser AuthUser;
        public SelfAPIUser CurrentUser;

        public static async Task<APIClient> LoginToMinecraft(string Token, WebProxy proxy = null)
        {
            APIClient Client = new()
            {
                MinecraftClient = new HttpClient(new HttpClientHandler { UseCookies = false, Proxy = proxy, AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }, true),
                LabyClient = new HttpClient(new HttpClientHandler { UseCookies = true, Proxy = proxy, AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }, true)
            };

            Client.MinecraftClient.DefaultRequestHeaders.Add("Accept", "*/*");
            Client.MinecraftClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
            Client.MinecraftClient.DefaultRequestHeaders.Add("Accept-Language", "de-DE,de;q=0.9,en-US;q=0.8,en;q=0.7");
            Client.MinecraftClient.DefaultRequestHeaders.Add("Host", "api.minecraftservices.com");
            Client.MinecraftClient.DefaultRequestHeaders.Add("Origin", "https://www.minecraft.net");
            Client.MinecraftClient.DefaultRequestHeaders.Add("Referer", "https://www.minecraft.net/");
            Client.MinecraftClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.102 Safari/537.36 OPR/90.0.4480.100");

            string Body = JsonConvert.SerializeObject(new { ensureLegacyEnabled = true, identityToken = Token });

            HttpRequestMessage Payload = new(HttpMethod.Post, $"https://api.minecraftservices.com/authentication/login_with_xbox")
            {
                Content = new StringContent(Body, Encoding.UTF8, "application/json")
            };
            Payload.Content.Headers.ContentType.CharSet = "";

            HttpResponseMessage Response = await Client.MinecraftClient.SendAsync(Payload);

            if (Response.IsSuccessStatusCode)
            {
                string ResponseBody = await Response.Content.ReadAsStringAsync();
                Client.AuthUser = JsonConvert.DeserializeObject<SelfAuthUser>(ResponseBody);
                Client.MinecraftClient.DefaultRequestHeaders.Add("Authorization", $"{Client.AuthUser.token_type} {Client.AuthUser.access_token}");
                Client.CurrentUser = await Client.GetCurrentUser();
                return Client;
            }
            return null;
        }

        private async Task<SelfAPIUser> GetCurrentUser()
        {
            HttpRequestMessage Payload = new(HttpMethod.Get, $"https://api.minecraftservices.com/minecraft/profile");

            HttpResponseMessage Response = await MinecraftClient.SendAsync(Payload);

            if (Response.IsSuccessStatusCode) return JsonConvert.DeserializeObject<SelfAPIUser>(await Response.Content.ReadAsStringAsync());
            return null;
        }


        public async Task<bool> ChangeSkin(string URL, bool Slim)
        {
            string Body = JsonConvert.SerializeObject(new { url = URL, variant = Slim ? "slim" : "classic" });

            HttpRequestMessage Payload = new(HttpMethod.Post, $"https://api.minecraftservices.com/minecraft/profile/skins")
            {
                Content = new StringContent(Body, Encoding.UTF8, "application/json")
            };
            Payload.Content.Headers.ContentType.CharSet = "";

            HttpResponseMessage Response = await MinecraftClient.SendAsync(Payload);

            return Response.IsSuccessStatusCode;
        }

        public async Task<bool> ChangeName(string Name)
        {
            string Body = JsonConvert.SerializeObject(new { url = Name });

            HttpRequestMessage Payload = new(HttpMethod.Post, $"https://api.minecraftservices.com/minecraft/profile/namechange")
            {
                Content = new StringContent(Body, Encoding.UTF8, "application/json")
            };
            Payload.Content.Headers.ContentType.CharSet = "";

            HttpResponseMessage Response = await MinecraftClient.SendAsync(Payload);

            return Response.IsSuccessStatusCode;
        }

        public async Task<APIUser> GetUserByName(string Name)
        {
            HttpRequestMessage Payload = new(HttpMethod.Get, $"https://api.mojang.com/users/profiles/minecraft/{Name}");

            HttpResponseMessage Response = await MinecraftClient.SendAsync(Payload);

            string content = await Response.Content.ReadAsStringAsync();

            if (Response.IsSuccessStatusCode) return JsonConvert.DeserializeObject<APIUser>(content);
            return null;
        }

        public async Task<APIUser> GetUserByID(string ID)
        {
            HttpRequestMessage Payload = new(HttpMethod.Get, $"https://api.mojang.com/user/profile/{ID}");

            HttpResponseMessage Response = await MinecraftClient.SendAsync(Payload);

            string content = await Response.Content.ReadAsStringAsync();

            if (Response.IsSuccessStatusCode) return JsonConvert.DeserializeObject<APIUser>(content);
            return null;
        }

        public async Task<bool> JoinServer(string serverHash)
        {
            string Body = JsonConvert.SerializeObject(new { serverId = serverHash, accessToken = AuthUser.access_token, selectedProfile = CurrentUser.id });

            HttpRequestMessage Payload = new(HttpMethod.Post, $"https://sessionserver.mojang.com/session/minecraft/join")
            {
                Content = new StringContent(Body, Encoding.UTF8, "application/json")
            };
            Payload.Content.Headers.ContentType.CharSet = "";

            HttpResponseMessage Response = await MinecraftClient.SendAsync(Payload);
            return Response.IsSuccessStatusCode;
        }

        public async Task<bool> LoginToLaby(string Pin)
        {
            LabyClient.DefaultRequestHeaders.Add("Accept", "*/*");
            LabyClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
            LabyClient.DefaultRequestHeaders.Add("Accept-Language", "de-DE,de;q=0.9");
            LabyClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
            LabyClient.DefaultRequestHeaders.Add("Host", "www.labymod.net");
            LabyClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.102 Safari/537.36 OPR/90.0.4480.100");

            HttpRequestMessage Payload = new(HttpMethod.Get, $"https://www.labymod.net/key?id={UUID.AddDashesToUUID(CurrentUser.id)}&pin={Pin}");

            HttpResponseMessage Response = await LabyClient.SendAsync(Payload);

            if (Response.IsSuccessStatusCode) return true;
            return false;
        }

        public async Task<string> ClaimDailyCoins()
        {
            string Body = "------WebKitFormBoundaryheWrWGH62gjsqjdb\r\nContent-Disposition: form-data; name=\"streak\"\r\n\r\n0\r\n------WebKitFormBoundaryheWrWGH62gjsqjdb--";

            HttpRequestMessage Payload = new(HttpMethod.Post, $"https://www.labymod.net/api/dashboard/claim-streak-reward")
            {
                Content = new StringContent(Body, Encoding.UTF8, "multipart/form-data")
            };
            Payload.Content.Headers.ContentType.CharSet = "";

            HttpResponseMessage Response = await LabyClient.SendAsync(Payload);
            if (Response.IsSuccessStatusCode) return await Response.Content.ReadAsStringAsync();
            return null;
        }

        public async Task<LabyCoinCount> GetCoinsCount()
        {
            HttpRequestMessage Payload = new(HttpMethod.Post, $"https://www.labymod.net/api/coins/action/get-balance-to-send");

            HttpResponseMessage Response = await LabyClient.SendAsync(Payload);
            if (Response.IsSuccessStatusCode)
            {
                string content = await Response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<LabyCoinCount>(content);
            }
            return null;
        }
    }
}
