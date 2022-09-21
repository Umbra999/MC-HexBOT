using Newtonsoft.Json;
using System.Numerics;
using System.Text;

namespace MCHexBOT.Utils
{
    public static class Misc
    {
        public static double Distance(Vector3 location1, Vector3 location2)
        {
            return Math.Sqrt(
                ((location1.X - location2.X) * (location1.X - location2.X)) +
                ((location1.Y - location2.Y) * (location1.Y - location2.Y)) +
                ((location1.Z - location2.Z) * (location1.Z - location2.Z)));
        }

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
