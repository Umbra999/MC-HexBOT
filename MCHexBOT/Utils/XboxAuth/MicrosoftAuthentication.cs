using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace MCHexBOT.XboxAuth
{
    static class Microsoft
    {
        public class LoginResponse
        {
            public string Email;
            public string AccessToken;
            public string RefreshToken;
            public int ExpiresIn;
        }

        public static string GetXboxToken(string email, string password)
        {
            var msaResponse = XboxLive.UserLogin(email, password, XboxLive.PreAuth());
            msaResponse.RefreshToken = string.Empty;
            var xblResponse = XboxLive.XblAuthenticate(msaResponse);
            var xsts = XboxLive.XSTSAuthenticate(xblResponse);

            return "XBL3.0 x=" + xsts.UserHash + ";" + xsts.Token;
        }
    }

    static class XboxLive
    {
        private static readonly string userAgent = "Mozilla/5.0 (XboxReplay; XboxLiveAuth/3.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/71.0.3578.98 Safari/537.36";

        public static PreAuthResponse PreAuth()
        {
            ProxiedWebRequest request = new("https://login.live.com/oauth20_authorize.srf?client_id=000000004C12AE6F&redirect_uri=https://login.live.com/oauth20_desktop.srf&scope=service::user.auth.xboxlive.com::MBI_SSL&display=touch&response_type=token&locale=en")
            {
                UserAgent = userAgent
            };
            var response = request.Get();

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

        public static Microsoft.LoginResponse UserLogin(string email, string password, PreAuthResponse preAuth)
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
                    var dict = hash.Split('&').ToDictionary(c => c.Split('=')[0], c => Uri.UnescapeDataString(c.Split('=')[1]));

                    return new Microsoft.LoginResponse()
                    {
                        Email = email,
                        AccessToken = dict["access_token"],
                        RefreshToken = dict["refresh_token"],
                        ExpiresIn = int.Parse(dict["expires_in"])
                    };
                }
            }

            throw new Exception("Failed Authentication to Microsoft");
        }

        public static XblAuthenticateResponse XblAuthenticate(Microsoft.LoginResponse loginResponse)
        {
            ProxiedWebRequest request = new("https://user.auth.xboxlive.com/user/authenticate")
            {
                UserAgent = userAgent,
                Accept = "application/json"
            };
            request.Headers.Add("x-xbl-contract-version", "0");

            var accessToken = loginResponse.AccessToken;

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
                string token = json.Properties["Token"].StringValue;
                string userHash = json.Properties["DisplayClaims"].Properties["xui"].DataArray[0].Properties["uhs"].StringValue;
                return new XblAuthenticateResponse()
                {
                    Token = token,
                    UserHash = userHash
                };
            }
            else
            {
                throw new Exception("XBL Authentication failed");
            }
        }

        public static XSTSAuthenticateResponse XSTSAuthenticate(XblAuthenticateResponse xblResponse)
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
                + "\"" + xblResponse.Token + "\""
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

            throw new Exception("XSTS Authentication failed");
        }

        public class PreAuthResponse
        {
            public string UrlPost;
            public string PPFT;
            public NameValueCollection Cookie;
        }

        public class XblAuthenticateResponse
        {
            public string Token;
            public string UserHash;
        }

        public class XSTSAuthenticateResponse
        {
            public string Token;
            public string UserHash;
        }
    }
}
