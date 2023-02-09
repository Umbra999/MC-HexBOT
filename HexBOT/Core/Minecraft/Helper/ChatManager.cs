using HexBOT.Packets.Client.Play;

namespace HexBOT.Core.Minecraft.Helper
{
    public class ChatManager
    {
        public ChatManager(MinecraftClient Client)
        {
            Bot = Client;
        }

        private readonly MinecraftClient Bot;

        public bool EnableChatCommands = true;

        public void OnChatMessageReceived(ChatMessagePacket packet)
        {

        }

        private void HandleChatCommand(string Message)
        {
            if (!EnableChatCommands) return;
        }
    }
}
