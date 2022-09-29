using MCHexBOT.Protocol.Utils;
using MCHexBOT.Utils;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace MCHexBOT.Core.API
{
    public class APIClient
    {
        private HttpClient Client;
        private HttpClient LabyClient;
        private IWebProxy Proxy;

        public SelfAuthUser AuthUser;
        public SelfAPIUser CurrentUser;

        public APIClient(WebProxy proxy = null)
        {
            Proxy = proxy;
            Client = new HttpClient(new HttpClientHandler { UseCookies = false, Proxy = Proxy, AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }, true);
            LabyClient = new HttpClient(new HttpClientHandler { UseCookies = true, Proxy = Proxy, AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }, true);
        }

        public async Task<bool> LoginToMinecraft(string Token)
        {
            Client.DefaultRequestHeaders.Add("Accept", "*/*");
            Client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
            Client.DefaultRequestHeaders.Add("Accept-Language", "de-DE,de;q=0.9,en-US;q=0.8,en;q=0.7");
            Client.DefaultRequestHeaders.Add("Host", "api.minecraftservices.com");
            Client.DefaultRequestHeaders.Add("Origin", "https://www.minecraft.net");
            Client.DefaultRequestHeaders.Add("Referer", "https://www.minecraft.net/");
            Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.102 Safari/537.36 OPR/90.0.4480.100");

            string Body = JsonConvert.SerializeObject(new { ensureLegacyEnabled = true, identityToken = Token });

            HttpRequestMessage Payload = new(HttpMethod.Post, $"https://api.minecraftservices.com/authentication/login_with_xbox")
            {
                Content = new StringContent(Body, Encoding.UTF8, "application/json")
            };
            Payload.Content.Headers.ContentType.CharSet = "";

            HttpResponseMessage Response = await Client.SendAsync(Payload);

            if (Response.IsSuccessStatusCode)
            {
                string ResponseBody = await Response.Content.ReadAsStringAsync();
                AuthUser = JsonConvert.DeserializeObject<SelfAuthUser>(ResponseBody);
                Client.DefaultRequestHeaders.Add("Authorization", $"{AuthUser.token_type} {AuthUser.access_token}");
                CurrentUser = await GetCurrentUser();
                return true;
            }
            return false;
        }

        private async Task<SelfAPIUser> GetCurrentUser()
        {
            HttpRequestMessage Payload = new(HttpMethod.Get, $"https://api.minecraftservices.com/minecraft/profile");

            HttpResponseMessage Response = await Client.SendAsync(Payload);

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

            HttpResponseMessage Response = await Client.SendAsync(Payload);
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

            HttpResponseMessage Response = await Client.SendAsync(Payload);
            return Response.IsSuccessStatusCode;
        }

        public async Task<APIUser> GetUserByName(string Name)
        {
            HttpRequestMessage Payload = new(HttpMethod.Get, $"https://api.mojang.com/users/profiles/minecraft/{Name}");

            HttpResponseMessage Response = await Client.SendAsync(Payload);

            string content = await Response.Content.ReadAsStringAsync();
            if (Response.IsSuccessStatusCode) return JsonConvert.DeserializeObject<APIUser>(content);
            return null;
        }

        public async Task<APIUser> GetUserByID(string ID)
        {
            HttpRequestMessage Payload = new(HttpMethod.Get, $"https://api.mojang.com/user/profile/{ID}");

            HttpResponseMessage Response = await Client.SendAsync(Payload);

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

            HttpResponseMessage Response = await Client.SendAsync(Payload);
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

            HttpRequestMessage Payload = new(HttpMethod.Get, $"https://www.labymod.net/key?id={new UUID(CurrentUser.id)}&pin={Pin}");

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
    }
}
