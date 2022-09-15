namespace MCHexBOT.Utils
{
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
}
