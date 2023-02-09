using HexBOT.Core.Minecraft;
using HexBOT.Packets.Server.Play;
using HexBOT.Utils;
using System.Numerics;

namespace HexBOT.Features
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
            //while (TargetNames.Count > 0)
            //{
            //    Player[] Founds = Bot.EntityManager.AllPlayers.Where(x => TargetNames.Contains(x.PlayerInfo?.Name)).ToArray();
            //    if (Founds.Length == 0) return;

            //    foreach (Player player in Founds)
            //    {
            //        if (RangeLimit)
            //        {
            //            if (Vector3.Distance(Bot.EntityManager.LocalPlayer.Position, player.Position) < Range)
            //            {
            //                Movement.LookAtPosition(Bot, player.Position);
            //                Bot.SendEntityInteraction(player.EntityID, UseEntityPacket.EntityInteractType.Attack);
            //                Bot.SendAnimation();
            //            }
            //        }
            //        else
            //        {
            //            Movement.LookAtPosition(Bot, player.Position);
            //            Bot.SendEntityInteraction(player.EntityID, UseEntityPacket.EntityInteractType.Attack);
            //            Bot.SendAnimation();
            //        }
            //    }

            //    await Task.Delay(60);
            //}
        }
    }
}
