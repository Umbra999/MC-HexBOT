using MCHexBOT.Core;
using MCHexBOT.Utils;

namespace MCHexBOT.Features
{
    internal class Combat
    {
        private static readonly List<string> TargetNames = new();
        private static readonly bool RangeLimit = true;
        private static readonly float Range = 4; 

        public static void ToggleAttack(string Name)
        {
            if (TargetNames.Contains(Name)) TargetNames.Remove(Name);
            else TargetNames.Add(Name);
        }

        public static void CombatLoop(MinecraftClient Bot)
        {
            new Thread(() =>
            {
                for (; ; )
                {
                    if (Bot.MCConnection.State == Protocol.ConnectionState.Play)
                    {
                        try
                        {
                            foreach (Player player in Bot.Players.Where(x => TargetNames.Contains(x.PlayerInfo?.Name)))
                            {
                                if (RangeLimit)
                                {
                                    if (Misc.Distance(Bot.GetLocalPlayer().Position, player.Position) < Range) Bot.SendEntityInteraction(player.EntityID, false, Protocol.EntityInteractType.Attack, Protocol.EntityInteractHandType.Main);
                                }
                                else Bot.SendEntityInteraction(player.EntityID, false, Protocol.EntityInteractType.Attack, Protocol.EntityInteractHandType.Main);
                            }
                        }
                        catch { }
                    }

                    Thread.Sleep(50);
                }
            }).Start();
        }
    }
}
