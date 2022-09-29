using MCHexBOT.Core.Minecraft;
using MCHexBOT.Protocol;
using MCHexBOT.Utils;
using System.Numerics;

namespace MCHexBOT.Features
{
    internal class Movement
    {
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

        // Helper


        // Movement
        public static void LookAtPosition(MinecraftClient Bot, Vector3 location)
        {
            SendMovement(Bot, Bot.GetLocalPlayer().Position, GetLookAtPosition(Bot, location), Bot.GetLocalPlayer().IsOnGround);
        }

        public static Vector2 GetLookAtPosition(MinecraftClient Bot, Vector3 location)
        {
            double dx = location.X - Bot.GetLocalPlayer().Position.X;
            double dy = location.Y - Bot.GetLocalPlayer().Position.Y;
            double dz = location.Z - Bot.GetLocalPlayer().Position.Z;

            double r = Math.Sqrt(dx * dx + dy * dy + dz * dz);

            float yaw = Convert.ToSingle(-Math.Atan2(dx, dz) / Math.PI * 180);
            float pitch = Convert.ToSingle(-Math.Asin(dy / r) / Math.PI * 180);
            if (yaw < 0) yaw += 360;

            return new Vector2(yaw, pitch);
        }

        public static void LookAtDirection(MinecraftClient Bot, Direction direction)
        {
            float yaw = Bot.GetLocalPlayer().Rotation.X;
            float pitch = Bot.GetLocalPlayer().Rotation.Y;

            switch (direction)
            {
                case Direction.Up:
                    pitch = -90;
                    break;

                case Direction.Down:
                    pitch = 90;
                    break;

                case Direction.East:
                    yaw = 270;
                    break;

                case Direction.West:
                    yaw = 90;
                    break;

                case Direction.North:
                    yaw = 180;
                    break;

                case Direction.South:
                    yaw = 360;
                    break;

                case Direction.SouthWest:
                    yaw = 45;
                    break;

                case Direction.SouthEast:
                    yaw = 315;
                    break;

                case Direction.NorthWest:
                    yaw = 135;
                    break;

                case Direction.NorthEast:
                    yaw = 225;
                    break;
            }

            SendMovement(Bot, Bot.GetLocalPlayer().Position, new Vector2(yaw, pitch), Bot.GetLocalPlayer().IsOnGround);
        }

        public static async Task MoveToPosition(MinecraftClient Bot, Vector3 Target, CancellationTokenSource token = null)
        {
            Vector3 CurrentPosition = Bot.GetLocalPlayer().Position;

            float SneakSpeed = 1.3f;
            float WalkSpeed = 4.31f;
            float SprintSpeed = 5.61f;
            float FallingSpeed = 28; // Gravity 28 m/s^2
            float JumpSpeed = 5f; // Unknown

            float CurrentSpeed = WalkSpeed;
            if (Bot.GetLocalPlayer().IsSneaking) CurrentSpeed = SneakSpeed;
            else if (Bot.GetLocalPlayer().IsSprinting) CurrentSpeed = SprintSpeed;

            int Timing = 50; // MS Between events.
            int TPS = 1000 / Timing;

            Vector3 DistanceVector = Target - CurrentPosition;
            float distance = Vector3.Distance(Target, CurrentPosition);
            float distancePerTiming = (float)CurrentSpeed / TPS;

            int Events = (int)(distance / (float)distancePerTiming);

            LookAtPosition(Bot, Target); // needs smothing

            for (int i = 0; i < Events; i++)
            {
                if (token != null && token.IsCancellationRequested) return;

                //CurrentPosition += new Vector3(DistanceVector.X / Events, DistanceVector.Y / Events, DistanceVector.Z / Events);
                CurrentPosition += new Vector3(DistanceVector.X / Events, 0, DistanceVector.Z / Events);

                SendMovement(Bot, CurrentPosition, Bot.GetLocalPlayer().Rotation, Bot.GetLocalPlayer().IsOnGround);

                await Task.Delay(Timing);
            }
        }

        public static async Task Jump(MinecraftClient Bot)
        {
            float JumpHeight = 1.25f;
            Vector3 Pos1 = Bot.GetLocalPlayer().Position;
            Vector3 Pos2 = Pos1 + new Vector3(0, JumpHeight, 0);
            float JumpSpeed = 3.5f;
            float CurrentSpeed = JumpSpeed;
            int Timing = 50; // MS Between events.
            int TPS = 1000 / Timing;
            Vector3 DistanceVector = Pos2 - Pos1;
            float distance = 1;
            float distancePerTiming = (float)CurrentSpeed / TPS;
            int Events = (int)(distance / (float)distancePerTiming);
            int RequiredTime = Events * Timing;
            Vector3 CurrentPosition = Pos1;
            for (int i = 0; i < Events; i++)
            {
                CurrentPosition += new Vector3(DistanceVector.X / Events, DistanceVector.Y / Events, DistanceVector.Z / Events);
                SendMovement(Bot, CurrentPosition, Bot.GetLocalPlayer().Rotation, false);
                await Task.Delay(Timing);
            }
            Logger.LogSuccess($"Reached Jump Destination");
            await Fall(Bot, CurrentPosition, JumpHeight);
        }

        public static async Task Fall(MinecraftClient Bot, Vector3 Pos, float Height = 1f)
        {
            Vector3 Pos1 = Pos;
            Vector3 Pos2 = Pos1 + new Vector3(0, -Height, 0);
            float FallingSpeed = 4f;
            float CurrentSpeed = FallingSpeed;
            int Timing = 50; // MS Between events.
            int TPS = 1000 / Timing;
            Vector3 DistanceVector = Pos2 - Pos1;
            float distance = 1;
            float distancePerTiming = (float)CurrentSpeed / TPS;
            int Events = (int)(distance / (float)distancePerTiming);
            int RequiredTime = Events * Timing;
            Vector3 CurrentPosition = Pos1;
            for (int i = 0; i < Events; i++)
            {
                CurrentPosition += new Vector3(DistanceVector.X / Events, DistanceVector.Y / Events, DistanceVector.Z / Events);
                SendMovement(Bot, CurrentPosition, Bot.GetLocalPlayer().Rotation, false);
                await Task.Delay(Timing);
            }
            CurrentPosition = new Vector3(CurrentPosition.X, MathF.Floor(CurrentPosition.Y), CurrentPosition.Z);
            SendMovement(Bot, CurrentPosition, Bot.GetLocalPlayer().Rotation, true);
            Logger.LogSuccess($"Reached Fall Destination");
        }

        public static string FollowTarget = "";
        public static async Task LoopPlayerMovement(MinecraftClient Bot)
        {
            Player[] Founds = Bot.Players.Where(x => x.PlayerInfo.Name == FollowTarget).ToArray();
            if (Founds.Length == 0) return;
            Player Target = Founds.First();

            Vector3 CurrentTargetPos = Target.Position;
            Vector3 LastTargetPos = Target.Position;

            CancellationTokenSource tokenSource = new();

            while (Target != null && FollowTarget == Target.PlayerInfo.Name)
            {
                CurrentTargetPos = Target.Position;
                if (CurrentTargetPos != LastTargetPos)
                {
                    LastTargetPos = CurrentTargetPos;
                    tokenSource.Cancel();
                    tokenSource = new CancellationTokenSource();
                    MoveToPosition(Bot, CurrentTargetPos, tokenSource);
                }

                await Task.Delay(50);
            }

            tokenSource.Cancel();
        }

        // Math Helper

        public static Vector3 CalculateDirections(Vector3 location, Direction direction, int length = 1)
        {
            return location + CalculateDirections(direction) * length;
        }

        public static Vector3 CalculateDirections(float Distance, Direction direction)
        {
            return new Vector3(Distance, Distance, Distance) * CalculateDirections(direction);
        }

        public static Vector3 CalculateDirections(Direction direction)
        {
            switch (direction)
            {
                // Move vertical
                case Direction.Down:
                    return new Vector3(0, -1, 0);
                case Direction.Up:
                    return new Vector3(0, 1, 0);

                // Move horizontal straight
                case Direction.East:
                    return new Vector3(1, 0, 0);
                case Direction.West:
                    return new Vector3(-1, 0, 0);
                case Direction.South:
                    return new Vector3(0, 0, 1);
                case Direction.North:
                    return new Vector3(0, 0, -1);

                // Move horizontal diagonal
                case Direction.NorthEast:
                    return CalculateDirections(Direction.North) + CalculateDirections(Direction.East);
                case Direction.SouthEast:
                    return CalculateDirections(Direction.South) + CalculateDirections(Direction.East);
                case Direction.SouthWest:
                    return CalculateDirections(Direction.South) + CalculateDirections(Direction.West);
                case Direction.NorthWest:
                    return CalculateDirections(Direction.North) + CalculateDirections(Direction.West);
            }

            return new Vector3(0, 0, 0);
        }
    }
}
