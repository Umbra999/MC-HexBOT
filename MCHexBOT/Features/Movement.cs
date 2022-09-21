using MCHexBOT.Core;
using MCHexBOT.Protocol;
using MCHexBOT.Utils;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.X509;
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
            double dx = location.X - Bot.GetLocalPlayer().Position.X;
            double dy = location.Y - Bot.GetLocalPlayer().Position.Y;
            double dz = location.Z - Bot.GetLocalPlayer().Position.Z;

            double r = Math.Sqrt(dx * dx + dy * dy + dz * dz);

            float yaw = Convert.ToSingle(-Math.Atan2(dx, dz) / Math.PI * 180);
            float pitch = Convert.ToSingle(-Math.Asin(dy / r) / Math.PI * 180);
            if (yaw < 0) yaw += 360;

            SendMovement(Bot, Bot.GetLocalPlayer().Position, new Vector2(yaw, pitch), Bot.GetLocalPlayer().IsOnGround);
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
            }

            SendMovement(Bot, Bot.GetLocalPlayer().Position, new Vector2(yaw, pitch), Bot.GetLocalPlayer().IsOnGround);
        }


        public static async Task MoveToPosition(MinecraftClient Bot, Vector3 Target, bool ShouldLook = true, CancellationTokenSource token = null)
        {
            Vector3 Pos1 = Bot.GetLocalPlayer().Position;
            Vector3 Pos2 = Target;

            float SneakSpeed = 1.3f;
            float WalkSpeed = 4.3f; // 4 Units/Time (Blocks/Second) 
            float SprintSpeed = 5.6f;
            float FallingSpeed = 0; // Gravity 28 m/s^2
            float JumpSpeed = 5f; // Unknown

            float CurrentSpeed = WalkSpeed;
            if (Bot.GetLocalPlayer().IsSneaking) CurrentSpeed = SneakSpeed;
            else if (Bot.GetLocalPlayer().IsSprinting) CurrentSpeed = SprintSpeed;

            int Timing = 50; // MS Between events.
            int TPS = 1000 / Timing;

            Vector3 DistanceVector = Pos2 - Pos1;
            float distance = Vector3.Distance(Pos2, Pos1);
            float distancePerTiming = (float)CurrentSpeed / TPS;

            //Console.WriteLine($" Start -> {Pos1}");
            //Console.WriteLine($" End -> {Pos2}");
            //Console.WriteLine($" Distance -> {distance} units | {DistanceVector}");
            //Console.WriteLine($" [B/T] Sneek -> {SneakSpeed}, Walk -> {WalkSpeed}, Sprint -> {SprintSpeed}");
            //Console.WriteLine($" Time Unit -> {Timing} (TPS -> {TPS})");
            //Console.WriteLine($" DistancePerTiming -> {distancePerTiming}");

            int Events = (int)(distance / (float)distancePerTiming);
            int RequiredTime = Events * Timing;

            //Console.WriteLine($" Number of Events -> {Events}");
            //Console.WriteLine($" Takes -> {RequiredTime} MS");

            Vector3 CurrentPosition = Pos1;
            Vector3 CurrentDistance = DistanceVector;

            if (ShouldLook) LookAtPosition(Bot, Pos2);

            for (int i = 0; i < Events; i++)
            {
                if (token != null && token.IsCancellationRequested) return;

                CurrentPosition += new Vector3(
                    DistanceVector.X / Events,
                    DistanceVector.Y / Events,
                    DistanceVector.Z / Events);
                CurrentDistance = Pos2 - CurrentPosition;

                //Console.WriteLine($" [{i}] Moving -> {CurrentPosition} | Distance -> {CurrentDistance}");

                SendMovement(Bot, CurrentPosition, Bot.GetLocalPlayer().Rotation, Bot.GetLocalPlayer().IsOnGround);

                await Task.Delay(Timing);
            }
            Logger.LogSuccess($"Reached Destination");
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
                CurrentPosition += new Vector3(
                    DistanceVector.X / Events,
                    DistanceVector.Y / Events,
                    DistanceVector.Z / Events);
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
                CurrentPosition += new Vector3(
                    DistanceVector.X / Events,
                    DistanceVector.Y / Events,
                    DistanceVector.Z / Events);
                SendMovement(Bot, CurrentPosition, Bot.GetLocalPlayer().Rotation, Bot.GetLocalPlayer().IsOnGround);
                await Task.Delay(Timing);
            }
            CurrentPosition = new Vector3(CurrentPosition.X, MathF.Floor(CurrentPosition.Y), CurrentPosition.Z);
            SendMovement(Bot, CurrentPosition, Bot.GetLocalPlayer().Rotation, Bot.GetLocalPlayer().IsOnGround);
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

            CancellationTokenSource tokenSource = new CancellationTokenSource();

            while (Target != null && FollowTarget == Target.PlayerInfo.Name)
            {
                CurrentTargetPos = Target.Position;
                if (CurrentTargetPos != LastTargetPos)
                {
                    LastTargetPos = CurrentTargetPos;
                    tokenSource.Cancel();
                    tokenSource = new CancellationTokenSource();
                    Task.Run(() => MoveToPosition(Bot, CurrentTargetPos, true, tokenSource));
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
