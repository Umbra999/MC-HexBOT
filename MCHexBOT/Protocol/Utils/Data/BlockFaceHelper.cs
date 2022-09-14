using MCHexBOT.Utils.Math;
using System.Numerics;

namespace MCHexBOT.Utils.Data
{
    public static class BlockFaceHelper
    {
        public static Vector3 GetVector3(this BlockFace face)
        {
            switch (face)
            {
                case BlockFace.Down:
                    return new Vector3(0, -1, 0);
                case BlockFace.Up:
                    return new Vector3(0, 1, 0);
                case BlockFace.East:
                    return new Vector3(1, 0, 0);
                case BlockFace.West:
                    return new Vector3(-1, 0, 0);
                case BlockFace.North:
                    return new Vector3(0, 0, 1);
                case BlockFace.South:
                    return new Vector3(0, 0, -1);
                default:
                    return Vector3.Zero;
            }
        }

        public static BlockCoordinates GetBlockCoordinates(this BlockFace face)
        {
            switch (face)
            {
                case BlockFace.Down:
                    return BlockCoordinates.Down;
                case BlockFace.Up:
                    return BlockCoordinates.Up;
                case BlockFace.East:
                    return BlockCoordinates.Right;
                case BlockFace.West:
                    return BlockCoordinates.Left;
                case BlockFace.North:
                    return BlockCoordinates.Backwards;
                case BlockFace.South:
                    return BlockCoordinates.Forwards;
                default:
                    return BlockCoordinates.Zero;
            }
        }

        public static BlockFace Opposite(this BlockFace face)
        {
            switch (face)
            {
                case BlockFace.Down:
                    return BlockFace.Up;
                case BlockFace.Up:
                    return BlockFace.Down;
                case BlockFace.East:
                    return BlockFace.West;
                case BlockFace.West:
                    return BlockFace.East;
                case BlockFace.North:
                    return BlockFace.South;
                case BlockFace.South:
                    return BlockFace.North;

                default:
                    return BlockFace.None;
            }
        }
    }
}
