namespace HexBOT.HexedServer
{
    internal class ServerObjects
    {
        public enum KeyPermissionType
        {
            MinecraftBot = 6,
        }

        public class UserData
        {
            public string Username { get; set; }
            public string Token { get; set; }
            public KeyPermissionType[] KeyAccess { get; set; }
        }
    }
}
