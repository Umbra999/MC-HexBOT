using Newtonsoft.Json;
using System.Text;

namespace MCHexBOT.Utils
{
    public static class Misc
    {
        public static void SendEmbedWebHook(string URL, object[] MSG)
        {
            Task.Run(async delegate
            {
                var req = new
                {
                    embeds = MSG
                };

                HttpClient CurrentClient = new(new HttpClientHandler { UseCookies = false });
                HttpRequestMessage Payload = new(HttpMethod.Post, URL);
                string joinWorldBody = JsonConvert.SerializeObject(req);
                Payload.Content = new StringContent(joinWorldBody, Encoding.UTF8, "application/json");
                Payload.Headers.Add("User-Agent", "Hexed");
                HttpResponseMessage Response = await CurrentClient.SendAsync(Payload);
            });
        }
    }
}
