namespace HexBOT.Protocol
{
    public enum ConnectionState
    {
        Handshaking,
        Status,
        Login,
        Play
    }

    public enum ProtocolType
    {
        Minecraft,
        Labymod
    }

    public enum Direction
    {
        Down = 0,
        Up = 1,
        North = 2,
        South = 3,
        West = 4,
        East = 5,
        NorthEast = 6,
        SouthEast = 7,
        SouthWest = 8,
        NorthWest = 9,
    }
}
