using MCHexBOT.Core;
using MCHexBOT.Packets.Server.Play;
using MCHexBOT.Utils;
using System.Numerics;

namespace MCHexBOT.Features
{
    internal class Combat
    {
        private static readonly List<string> TargetNames = new();
        private static readonly bool RangeLimit = true;
        private static readonly float Range = 3.5f; 

        public static void ToggleAttack(string Name)
        {
            if (TargetNames.Contains(Name))
            {
                TargetNames.Remove(Name);
                Logger.Log($"Removed {Name} from Target List");
            }
            else
            {
                int count = TargetNames.Count;
                TargetNames.Add(Name);
                Logger.Log($"Added {Name} to Target List");
                if (count < 1)
                {
                    foreach (MinecraftClient Bot in Main.Clients)
                    {
                        Task.Run(() => CombatLoop(Bot));
                    }
                }
            }
        }

        public static async Task CombatLoop(MinecraftClient Bot)
        {
            while (TargetNames.Count > 0)
            {
                Player[] Founds = Bot.Players.Where(x => TargetNames.Contains(x.PlayerInfo?.Name)).ToArray();
                if (Founds.Length == 0) return;

                foreach (Player player in Founds)
                {
                    if (RangeLimit)
                    {
                        if (Vector3.Distance(Bot.GetLocalPlayer().Position, player.Position) < Range)
                        {
                            Movement.LookAtPosition(Bot, player.Position);
                            Bot.SendEntityInteraction(player.EntityID, Bot.GetLocalPlayer().IsSneaking, InteractEntityPacket.EntityInteractType.Attack, InteractEntityPacket.EntityInteractHandType.Main);
                            Bot.SendAnimation(AnimationPacket.HandType.Main);
                        }
                    }
                    else
                    {
                        Movement.LookAtPosition(Bot, player.Position);
                        Bot.SendEntityInteraction(player.EntityID, Bot.GetLocalPlayer().IsSneaking, InteractEntityPacket.EntityInteractType.Attack, InteractEntityPacket.EntityInteractHandType.Main);
                        Bot.SendAnimation(AnimationPacket.HandType.Main);
                    }
                }

                await Task.Delay(60);
            }
        }
    }
}
