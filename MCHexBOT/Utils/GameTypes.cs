using MCHexBOT.Utils.Math;
using System.Numerics;

namespace MCHexBOT.Utils
{
    public class GameTypes
    {
        public class Player
        {
            public int EntityID { get; set; }
            public int Gamemode { get; set; }
            public Vector3 Position { get; set; }
            public float PositionPitch { get; set; }
            public float PositionYaw { get; set; }
            public float Health { get; set; }
            public int Food { get; set; }
            public float FoodSaturation { get; set; }
        }
    }
}
