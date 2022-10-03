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

        public static byte[] GetVarInt(int paramInt)
        {
            List<byte> bytes = new();
            while ((paramInt & -128) != 0)
            {
                bytes.Add((byte)(paramInt & 127 | 128));
                paramInt = (int)(((uint)paramInt) >> 7);
            }
            bytes.Add((byte)paramInt);
            return bytes.ToArray();
        }

        public static byte[] ConcatBytes(params byte[][] bytes)
        {
            List<byte> result = new();
            foreach (byte[] array in bytes)
            {
                result.AddRange(array);
            }
            return result.ToArray();
        }
    }
}
