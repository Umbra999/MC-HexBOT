using MCHexBOT.Utils;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static MCHexBOT.Utils.Misc;

namespace MCHexBOT.Core
{
    public class APIClient
    {
        private HttpClient Client;
        public SelfAuthUser AuthUser;
        public SelfAPIUser CurrentUser;

        public async Task<bool> Login(string Identity, WebProxy proxy = null)
        {
            Client = new HttpClient(new HttpClientHandler { UseCookies = false, Proxy = proxy, AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }, true);
            Client.DefaultRequestHeaders.Add("Accept", "*/*");
            Client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
            Client.DefaultRequestHeaders.Add("Accept-Language", "de-DE,de;q=0.9,en-US;q=0.8,en;q=0.7");
            Client.DefaultRequestHeaders.Add("Host", "api.minecraftservices.com");
            Client.DefaultRequestHeaders.Add("Origin", "https://www.minecraft.net");
            Client.DefaultRequestHeaders.Add("Referer", "https://www.minecraft.net/");
            Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/104.0.5112.102 Safari/537.36 OPR/90.0.4480.100");

            string Body = JsonConvert.SerializeObject(new { ensureLegacyEnabled = true, identityToken = Identity });

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

            string content = await Response.Content.ReadAsStringAsync();
            if (Response.IsSuccessStatusCode) return JsonConvert.DeserializeObject<SelfAPIUser>(content);
            return null;
        }


        public async Task<bool> ChangeSkin(string URL, bool Slim)
        {
            string Body = JsonConvert.SerializeObject(new { url = URL, variant = Slim ? "slim" : "classic" });

            HttpRequestMessage Payload = new HttpRequestMessage(HttpMethod.Post, $"https://api.minecraftservices.com/minecraft/profile/skins")
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
            string Body = JsonConvert.SerializeObject(new { serverId = serverHash, accessToken = AuthUser.access_token, selectedProfile = CurrentUser.id});

            HttpRequestMessage Payload = new(HttpMethod.Post, $"https://sessionserver.mojang.com/session/minecraft/join")
            {
                Content = new StringContent(Body, Encoding.UTF8, "application/json")
            };
            Payload.Content.Headers.ContentType.CharSet = "";

            HttpResponseMessage Response = await Client.SendAsync(Payload);
            return Response.IsSuccessStatusCode;
        }
    }
}
