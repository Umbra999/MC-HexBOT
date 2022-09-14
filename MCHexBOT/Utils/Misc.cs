using System.Collections;
using System.Text;

namespace MCHexBOT.Utils
{
    public class Misc
    {
        public static string FromBase64(string Data)
        {
            var base64EncodedBytes = Convert.FromBase64String(Data);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string ToBase64(string Data)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(Data));
        }

        public class SelfAuthUser
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string[] roles { get; set; } // im not sure if string because always empty
            public string token_type { get; set; }
            public string username { get; set; }
        }

        public class SelfAPIUser
        {
            public Cape[] capes { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public Skin[] skins { get; set; }
        }

        public class Cape
        {
            public string alias { get; set; }
            public string id { get; set; }
            public string state { get; set; }
            public string url { get; set; }
        }

        public class Skin
        {
            public string id { get; set; }
            public string state { get; set; }
            public string url { get; set; }
            public string variant { get; set; }
        }

        public class APIUser
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public static Hashtable ProtocolVersions = new()
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
            { "1.15", 578 },
            { "1.16", 754 },
            { "1.17", 756 },
            { "1.18", 758 },
            { "1.19", 760 },
        };
    }
}
