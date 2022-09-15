using MCHexBOT.Utils.Math;
using System.Numerics;

namespace MCHexBOT.Utils
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

    public class ChatMessage
    {
        public string translate { get; set; }
        public string text { get; set; }
        public bool bold { get; set; }
        public bool italic { get; set; }
        public bool underlined { get; set; }
        public bool strikethrough { get; set; }
        public bool obfuscated { get; set; }
        public string font { get; set; }
        public string insertion { get; set; }
        public string color { get; set; }
        public ChatClickEvent clickEvent { get; set; }
        public ChatMessage[] extra { get; set; } = new ChatMessage[0];
        public ChatMessage[] with { get; set; } = new ChatMessage[0];
    }

    public class ChatClickEvent
    {

    }
}
