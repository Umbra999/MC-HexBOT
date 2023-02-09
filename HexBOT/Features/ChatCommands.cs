using HexBOT.Core.Minecraft;
using HexBOT.HexServer;
using HexBOT.Utils;

namespace HexBOT.Features
{
    internal class ChatCommands
    {
        public static void HandleChat(MinecraftClient Bot, ChatMessage MessageObject)
        {
            string Message = MessageObject.text ?? "";

            if (MessageObject.extra != null)
            {
                foreach (ChatMessage Extra in MessageObject.extra)
                {
                    if (Extra.text != null) Message += Extra.text;
                }
            }

            string[] Splitted = Message.Split('!');
            if (Splitted.Length > 1) HandleCommmand(Bot, Splitted[1]);
        }

        private static void HandleCommmand(MinecraftClient Bot, string Command)
        {
            string[] CommandArguments = Command.Split(' ');

            switch (CommandArguments[0])
            {
                case "dice":
                    Bot.SendChat("Dice: " + Encryption.RandomNumber(1, 6).ToString());
                    break;

                case "health":
                    Bot.SendChat($"Health: {Bot.EntityManager.LocalPlayer.Health} | Food: {Bot.EntityManager.LocalPlayer.Food} | Saturation {Bot.EntityManager.LocalPlayer.Saturation}");
                    break;
            }
        }
    }
}
