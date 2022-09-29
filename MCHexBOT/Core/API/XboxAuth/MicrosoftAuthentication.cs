using System.Text.RegularExpressions;
using System.Collections.Specialized;
using MCHexBOT.Utils;

namespace MCHexBOT.XboxAuth
{
    static class XboxLive
    {
        private static readonly string userAgent = "Mozilla/5.0 (XboxReplay; XboxLiveAuth/3.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36";

        public static string GetXboxToken(string email, string password)
        {
            PreAuthResponse preAuth = PreAuth();
            if (preAuth == null) return null;

            string accessToken = UserLogin(email, password, preAuth);
            if (accessToken == null) return null;

            string xblResponse = XblAuthenticate(accessToken);
            if (xblResponse == null) return null;

            XSTSAuthenticateResponse xsts = XSTSAuthenticate(xblResponse);
            if (xsts == null) return null;

            return "XBL3.0 x=" + xsts.UserHash + ";" + xsts.Token;
        }

        public static PreAuthResponse PreAuth()
        {
            ProxiedWebRequest request = new("https://login.live.com/oauth20_authorize.srf?client_id=000000004C12AE6F&redirect_uri=https://login.live.com/oauth20_desktop.srf&scope=service::user.auth.xboxlive.com::MBI_SSL&display=touch&response_type=token&locale=en")
            {
                UserAgent = userAgent
            };
            var response = request.Get();

            if (response.StatusCode == 200)
            {
                string html = response.Body;

                string PPFT = new Regex("sFTTag:'.*value=\"(.*)\"\\/>'").Match(html).Groups[1].Value;
                string urlPost = new Regex("urlPost:'(.+?(?=\'))").Match(html).Groups[1].Value;

                return new PreAuthResponse()
                {
                    UrlPost = urlPost,
                    PPFT = PPFT,
                    Cookie = response.Cookies
                };
            }

            Logger.LogError("Xbox PreAuth failed");
            return null;
        }

        public static string UserLogin(string email, string password, PreAuthResponse preAuth)
        {
            ProxiedWebRequest request = new(preAuth.UrlPost, preAuth.Cookie)
            {
                UserAgent = userAgent
            };

            string postData = "login=" + Uri.EscapeDataString(email)
                 + "&loginfmt=" + Uri.EscapeDataString(email)
                 + "&passwd=" + Uri.EscapeDataString(password)
                 + "&PPFT=" + Uri.EscapeDataString(preAuth.PPFT);

            var response = request.Post("application/x-www-form-urlencoded", postData);

            if (response.StatusCode == 302)
            {
                string url = response.Headers.Get("Location");
                string hash = url.Split('#')[1];

                var request2 = new ProxiedWebRequest(url);
                var response2 = request2.Get();

                if (response2.StatusCode == 200)
                {
                    Dictionary<string, string> dict = hash.Split('&').ToDictionary(c => c.Split('=')[0], c => Uri.UnescapeDataString(c.Split('=')[1]));
                    return dict["access_token"];
                }
            }

            Logger.LogError("Xbox User Login failed");
            return null;
        }

        public static string XblAuthenticate(string accessToken)
        {
            ProxiedWebRequest request = new("https://user.auth.xboxlive.com/user/authenticate")
            {
                UserAgent = userAgent,
                Accept = "application/json"
            };
            request.Headers.Add("x-xbl-contract-version", "0");

            string payload = "{"
                + "\"Properties\": {"
                + "\"AuthMethod\": \"RPS\","
                + "\"SiteName\": \"user.auth.xboxlive.com\","
                + "\"RpsTicket\": \"" + accessToken + "\""
                + "},"
                + "\"RelyingParty\": \"http://auth.xboxlive.com\","
                + "\"TokenType\": \"JWT\""
                + "}";
            var response = request.Post("application/json", payload);

            if (response.StatusCode == 200)
            {
                string jsonString = response.Body;

                JsonUtils.JSONData json = JsonUtils.ParseJson(jsonString);
                return json.Properties["Token"].StringValue;
            }

            Logger.LogError("Xbox Live Login failed");
            return null;
        }

        public static XSTSAuthenticateResponse XSTSAuthenticate(string Token)
        {
            ProxiedWebRequest request = new("https://xsts.auth.xboxlive.com/xsts/authorize")
            {
                UserAgent = userAgent,
                Accept = "application/json"
            };
            request.Headers.Add("x-xbl-contract-version", "1");

            string payload = "{"
                + "\"Properties\": {"
                + "\"SandboxId\": \"RETAIL\","
                + "\"UserTokens\": ["
                + "\"" + Token + "\""
                + "]"
                + "},"
                + "\"RelyingParty\": \"rp://api.minecraftservices.com/\","
                + "\"TokenType\": \"JWT\""
                + "}";
            var response = request.Post("application/json", payload);

            if (response.StatusCode == 200)
            {
                string jsonString = response.Body;
                JsonUtils.JSONData json = JsonUtils.ParseJson(jsonString);
                string token = json.Properties["Token"].StringValue;
                string userHash = json.Properties["DisplayClaims"].Properties["xui"].DataArray[0].Properties["uhs"].StringValue;
                return new XSTSAuthenticateResponse()
                {
                    Token = token,
                    UserHash = userHash
                };
            }

            Logger.LogError("Xbox XSTS Login failed");
            return null;
        }

        public class PreAuthResponse
        {
            public string UrlPost;
            public string PPFT;
            public NameValueCollection Cookie;
        }

        public class XSTSAuthenticateResponse
        {
            public string Token;
            public string UserHash;
        }
    }
}
