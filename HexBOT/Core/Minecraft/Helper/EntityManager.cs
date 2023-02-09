using HexBOT.Utils;
using System.Numerics;

namespace HexBOT.Core.Minecraft.Helper
{
    public class EntityManager
    {
        public EntityManager(MinecraftClient Client)
        {
            Bot = Client;
        }

        private readonly MinecraftClient Bot;

        public readonly List<Player> AllPlayers = new();

        public Player LocalPlayer;

        public void ClearAllPlayers()
        {
            AllPlayers.Clear();
            LocalPlayer = null;
        }

        public void ClearPlayer(Player player)
        {
            AllPlayers.Remove(player);
        }

        public void AddPlayer(Player player)
        {
            if (player.IsLocal)
            {
                if (LocalPlayer != null) // idk if thats a good idea tho
                {
                    Vector3 CachedPosition = LocalPlayer.Position;
                    Vector2 CachedRotation = LocalPlayer.Rotation;

                    LocalPlayer = player;
                    LocalPlayer.Position = CachedPosition;
                    LocalPlayer.Rotation = CachedRotation;
                }
                else LocalPlayer = player;
            }

            AllPlayers.Add(player);
        }
    }
}
