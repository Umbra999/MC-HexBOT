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

        public static Dictionary<string, int> ProtocolVersions = new()
        {
            { "1.7", 4 },
            { "1.8", 47 },
            { "1.9", 110 },
            { "1.10", 210 },
            { "1.11", 316 },
            { "1.12", 340 },
            { "1.13", 404 },
            { "1.14", 498 },
            { "1.15", 578 },
            { "1.16", 754 },
            { "1.17", 756 },
            { "1.18", 757 },
            { "1.19", 760 },
        };

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
