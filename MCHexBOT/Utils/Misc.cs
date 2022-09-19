using MCHexBOT.Packets.Client.Play;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections;
using System.Numerics;
using System.Text;

namespace MCHexBOT.Utils
{
    public static class Misc
    {
        public static double DistanceSquared(Vector3 From, Vector3 To)
        {
            var diff = new Vector3(From.X - To.X, From.Y - To.Y, From.Z - To.Z);
            return diff.X * diff.X + diff.Y * diff.Y + diff.Z * diff.Z;
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
