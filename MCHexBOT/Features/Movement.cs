using MCHexBOT.Core;

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
            new Thread(() => { SendMovement(Bot); Thread.CurrentThread.IsBackground = true; }).Start();
        }

        private static void SendMovement(MinecraftClient Bot)
        {
            for (; ; )
            {
                Bot.MCConnection.SendPaket(new Pakets.Server.Play.PlayerPositionAndRotationPaket()
                {
                    X = Bot.LocalPlayer.Position.X + OffsetX,
                    Y = Bot.LocalPlayer.Position.Y + OffsetY,
                    Z = Bot.LocalPlayer.Position.Z + OffsetZ,
                    OnGround = true,
                    Yaw = Bot.LocalPlayer.Rotation.X,
                    Pitch = Bot.LocalPlayer.Rotation.Y,
                });

                Thread.Sleep(50);
            }
        }
    }
}
