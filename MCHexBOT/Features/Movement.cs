using MCHexBOT.Core;
using MCHexBOT.Utils;
using System.Numerics;

namespace MCHexBOT.Features
{
    internal class Movement
    {
        public static string CopyMovementTarget = "";

        public static void MovementLoop(MinecraftClient Bot)
        {
            new Thread(() =>
            {
                for (; ; )
                {
                    if (Bot.MCConnection.State == Protocol.ConnectionState.Play) SendOnGround(Bot, Bot.GetLocalPlayer().IsOnGround);
                    Thread.Sleep(1000);
                }
            }).Start();
        }

        public static void HandleMovement(MinecraftClient Bot, Player player)
        {
            if (player.PlayerInfo.Name == CopyMovementTarget)
            {
                SendMovement(Bot, player.Position, player.Rotation, player.IsOnGround);
            }
        }

        public static void AcceptTeleport(MinecraftClient Bot, int ID)
        {
            Bot.MCConnection.SendPaket(new Pakets.Server.Play.TeleportConfirmPaket()
            {
                TeleportID = ID
            });
        }

        private static void SendMovement(MinecraftClient Bot, Vector3 Position, Vector2 Rotation, bool IsGround)
        {
            Player Local = Bot.GetLocalPlayer();
            if (Local.Position != Position && Local.Rotation != Rotation) SendPositionAndRotation(Bot, Position, Rotation, IsGround);
            else if (Local.Position != Position) SendPosition(Bot, Position, IsGround);
            else if (Local.Rotation != Rotation) SendRotation(Bot, Rotation, IsGround);
            else if (Local.IsOnGround != IsGround) SendOnGround(Bot, IsGround);

            Local.Position = Position;
            Local.Rotation = Rotation;
            Local.IsOnGround = IsGround;
        }

        private static void SendPositionAndRotation(MinecraftClient Bot, Vector3 Position, Vector2 Rotation, bool IsGround)
        {
            Bot.MCConnection.SendPaket(new Pakets.Server.Play.PlayerPositionAndRotationPaket()
            {
                X = Position.X,
                Y = Position.Y,
                Z = Position.Z,
                Pitch = Rotation.Y,
                Yaw = Rotation.X,
                OnGround = IsGround,
            });
        }

        private static void SendPosition(MinecraftClient Bot, Vector3 Position, bool IsGround)
        {
            Bot.MCConnection.SendPaket(new Pakets.Server.Play.PlayerPositionPaket()
            {
                X = Position.X,
                Y = Position.Y,
                Z = Position.Z,
                OnGround = IsGround,
            });
        }

        private static void SendRotation(MinecraftClient Bot, Vector2 Rotation, bool IsGround)
        {
            Bot.MCConnection.SendPaket(new Pakets.Server.Play.PlayerRotationPaket()
            {
                Pitch = Rotation.Y,
                Yaw = Rotation.X,
                OnGround = IsGround,
            });
        }

        private static void SendOnGround(MinecraftClient Bot, bool IsGround)
        {
            Bot.MCConnection.SendPaket(new Pakets.Server.Play.PlayerMovementPaket()
            {
                OnGround = IsGround,
            });
        }
    }
}
