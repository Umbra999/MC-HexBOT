using HexBOT.Utils;

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
        }

        public void ClearPlayer(Player player)
        {
            AllPlayers.Remove(player);
        }

        public void AddPlayer(Player player)
        {
            AllPlayers.Add(player);
        }
    }
}
