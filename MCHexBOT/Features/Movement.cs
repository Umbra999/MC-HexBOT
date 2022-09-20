using MCHexBOT.Core;
using MCHexBOT.Utils;
using System.Numerics;

namespace MCHexBOT.Features
{
    internal class Movement
    {
        public enum MovementPosition
        {
            None,
            Forward,
            Backward,
        }

        public static string CopyMovementTarget = "";

        private static float WalkSpeed = 0.21585f;
        private static float SprintSpeed = 0.2806f;
        private static float SneakSpeed = 0.0655f;
        private static float SwimmingSpeed = 0.11f;
        private static float JumpSpeed = 0.1f;

        public static MovementPosition WalkX = MovementPosition.None;
        public static MovementPosition WalkY = MovementPosition.None;
        public static MovementPosition WalkZ = MovementPosition.None;


        public static async Task MovementLoop(MinecraftClient Bot)
        {
            for (; ; )
            {
                if (Bot.MCConnection.State == Protocol.ConnectionState.Play)
                {
                    Vector3 Positions = Bot.GetLocalPlayer().Position;
                    Vector2 Rotations = Bot.GetLocalPlayer().Rotation;
                    bool IsGround = Bot.GetLocalPlayer().IsOnGround;

                    if (CopyMovementTarget != "")
                    {
                        Player[] Players = Bot.Players.Where(x => x.PlayerInfo?.Name == CopyMovementTarget).ToArray();
                        if (Players.Length > 0)
                        {
                            Vector3 Distance = Players.First().Position - Bot.GetLocalPlayer().Position;

                            if (Distance.X < 0) Positions.X += -WalkSpeed;
                            else if (Distance.X > 0) Positions.X += WalkSpeed;

                            if (Distance.Y < 0) Positions.Y += -JumpSpeed;
                            else if (Distance.Y > 0) Positions.Y += JumpSpeed;

                            if (Distance.Z < 0) Positions.Z += -WalkSpeed;
                            else if (Distance.Z > 0) Positions.Z = WalkSpeed;

                            Rotations = Players.First().Rotation;
                        }
                    }
                    else
                    {
                        if (WalkX != MovementPosition.None) Positions.X += WalkX == MovementPosition.Forward ? WalkSpeed : -WalkSpeed;
                        if (WalkY != MovementPosition.None) Positions.Y += WalkY == MovementPosition.Forward ? JumpSpeed : -JumpSpeed;
                        if (WalkZ != MovementPosition.None) Positions.Z += WalkZ == MovementPosition.Forward ? WalkSpeed : -WalkSpeed;
                    }

                    SendMovement(Bot, Positions, Rotations, IsGround);
                }

                await Task.Delay(50);
            }
        }

        public static void SendMovement(MinecraftClient Bot, Vector3 Position, Vector2 Rotation, bool IsGround)
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
            Bot.MCConnection.SendPacket(new Packets.Server.Play.PlayerPositionAndRotationPacket()
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
            Bot.MCConnection.SendPacket(new Packets.Server.Play.PlayerPositionPacket()
            {
                X = Position.X,
                Y = Position.Y,
                Z = Position.Z,
                OnGround = IsGround,
            });
        }

        private static void SendRotation(MinecraftClient Bot, Vector2 Rotation, bool IsGround)
        {
            Bot.MCConnection.SendPacket(new Packets.Server.Play.PlayerRotationPacket()
            {
                Pitch = Rotation.Y,
                Yaw = Rotation.X,
                OnGround = IsGround,
            });
        }

        private static void SendOnGround(MinecraftClient Bot, bool IsGround)
        {
            Bot.MCConnection.SendPacket(new Packets.Server.Play.PlayerMovementPacket()
            {
                OnGround = IsGround,
            });
        }

        private static void LookAtPosition(MinecraftClient Bot, Vector3 position)
        {
            var pos = position + new Vector3(0.5f, 0.5f, 0.5f);
            var r = pos - Bot.GetLocalPlayer().Position + new Vector3(0, 1, 0);
            double yaw = -Math.Atan2(r.X, r.Z) / Math.PI * 180;
            if (yaw < 0) yaw = 360 + yaw;
            double pitch = -Math.Asin(r.Y / r.Length()) / Math.PI * 180;
            Bot.GetLocalPlayer().Rotation = new Vector2((float)yaw, (float)pitch);
        }
    }
}
