namespace HexBOT.Core.Minecraft.Helper
{
    public class MovementManager
    {
        public MovementManager(MinecraftClient Client)
        {
            Bot = Client;
            StartMovementLoop();
        }

        private readonly MinecraftClient Bot;

        private void StartMovementLoop()
        {
            
        }
    }
}
