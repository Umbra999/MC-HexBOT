using MCHexBOT.Core;
using MCHexBOT.Utils;

namespace MCHexBOT.Features
{
    internal class ChatCommands
    {
        public static void HandleChat(MinecraftClient Bot, ChatMessage MessageObject)
        {
            string Message = MessageObject.text;
            foreach (ChatMessage Extra in MessageObject.extra)
            {
                Message += Extra.text;
            }

            string[] Splitted = Message.Split('!');
            if (Splitted.Length > 1) HandleCommmand(Bot, Splitted[1]);
        }

        private static void HandleCommmand(MinecraftClient Bot, string Command)
        {
            string[] CommandArguments = Command.Split(' ');
            switch (CommandArguments[0].ToLower())
            {
                case "dice":
                    Bot.SendChat("Dice rolled a: " + Misc.RandomNumber(1, 6).ToString());
                    break;

                case "health":
                    Bot.SendChat($"Health: {Bot.LocalPlayer.Health} | Food: {Bot.LocalPlayer.Food} | Saturation {Bot.LocalPlayer.FoodSaturation}");
                    break;
            }
        }
    }
}
