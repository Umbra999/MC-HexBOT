using HexBOT.Packets;
using HexBOT.Protocol;
using System.Numerics;
using static HexBOT.Packets.Client.Play.PlayerListItemPacket;

namespace HexBOT.Utils
{
    public class Player
    {
        public int EntityID { get; set; }
        public string UUID { get; set; }
        public string Name { get; set; }
        public bool IsLocal { get; set; }
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
        public int NumberOfProperties { get; set; }
        public List<PlayerInfoProperty> Properties { get; set; }
        public int GameMode { get; set; }
        public int Ping { get; set; }
        public bool HasDisplayName { get; set; }
        public string DisplayName { get; set; }

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
        public ChatMessage[] extra { get; set; }
        public ChatMessage[] with { get; set; }
    }

    public class ChatClickEvent
    {
        public string action { get; set; }
        public string value { get; set; }
    }

    public class PacketQueueItem
    {
        public IPacket Packet { get; set; }
        public ConnectionState State { get; set; }
    }
}
