using MCHexBOT.Protocol;
using MCHexBOT.Protocol.Utils;
using System.Numerics;

namespace MCHexBOT.Utils
{
    public class Player
    {
        public int EntityID { get; set; }
        public bool IsLocal { get; set; }
        public PlayerInfo PlayerInfo { get; set; }
        public Vector3 Position { get; set; }
        public Vector2 Rotation { get; set; }
        public Vector3 Velocity { get; set; }
        public bool IsOnGround { get; set; }
        public int Food { get; set; }
        public float Health { get; set; }
        public float Saturation { get; set; }
        public bool IsSprinting { get; set; }
        public bool IsSneaking { get; set; }
        public bool IsBurning { get; set; }
        public bool IsInvisible { get; set; }
        public short HeldItemSlot { get; set; }

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
        ChatClickEvent clickEvent { get; set; }
        public ChatMessage[] extra { get; set; }
        public ChatMessage[] with { get; set; }
    }

    public class ChatClickEvent
    {
        string action { get; set; }
        string value { get; set; }
    }

    public class PlayerInfo
    {
        public UUID UUID { get; set; }
        public string Name { get; set; }
        public int NumberOfProperties { get; set; }
        public List<PlayerInfoProperty> Properties { get; set; }
        public int GameMode { get; set; }
        public int Ping { get; set; }
        public bool HasDisplayName { get; set; }
        public string DisplayName { get; set; }
    }

    public class PlayerInfoProperty
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Singed { get; set; }
        public string Signature { get; set; }
    }

    public class EntityMetadata
    {
        public EntityMetadataType Type { get; set; }
        public object Value { get; set; }
    }

    public class VillagerData
    {
        public int Type { get; set; }
        public int Profession { get; set; }
        public int Level { get; set; }
    }
}
