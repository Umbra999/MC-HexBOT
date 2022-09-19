﻿using MCHexBOT.Core;
using MCHexBOT.HexServer;
using MCHexBOT.Utils;

namespace MCHexBOT.Features
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
                    Bot.SendChat($"Health: {Bot.GetLocalPlayer().Health} | Food: {Bot.GetLocalPlayer().Food} | Saturation {Bot.GetLocalPlayer().Saturation}");
                    break;
            }
        }
    }
}
