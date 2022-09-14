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
                    X = Bot.CurrentPlayer.Position.X + OffsetX,
                    FeetY = Bot.CurrentPlayer.Position.Y + OffsetY,
                    Z = Bot.CurrentPlayer.Position.Z + OffsetZ,
                    OnGround = true,
                    //Pitch = Bot.CurrentPlayer.PositionPitch,
                    //Yaw = Bot.CurrentPlayer.PositionYaw,
                });

                Thread.Sleep(50);
            }
        }
    }
}
