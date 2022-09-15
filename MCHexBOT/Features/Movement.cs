using MCHexBOT.Core;
using MCHexBOT.Utils;

namespace MCHexBOT.Features
{
    internal class Movement
    {
        private static bool CopyUserMovementToggle = false;
        public static int OffsetX = 0;
        public static int OffsetY = 0;
        public static int OffsetZ = 0;

        public static void StartMovementLoop(MinecraftClient Bot)
        {
            new Thread(() => { SendMovement(Bot); Thread.CurrentThread.IsBackground = true; }).Start();;
        }

        private static void SendMovement(MinecraftClient Bot)
        {
            for (; ; )
            {
                Bot.MCConnection.SendPaket(new Pakets.Server.Play.PlayerPositionAndRotationPaket()
                {
                    X = Bot.LocalPlayer.Position.X + OffsetX,
                    FeetY = Bot.LocalPlayer.Position.Y + OffsetY,
                    Z = Bot.LocalPlayer.Position.Z + OffsetZ,
                    OnGround = true,
                    Pitch = Bot.LocalPlayer.PositionPitch,
                    Yaw = Bot.LocalPlayer.PositionYaw,
                });

                Thread.Sleep(50);
            }
        }
    }
}
