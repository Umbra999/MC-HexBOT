﻿using MCHexBOT.Network;

namespace MCHexBOT.Packets.Client.Play
{
    internal class EntityVelocityPacket : IPacket
    {
        public int EntityId { get; set; }
        public short XVelocity { get; set; }
        public short YVelocity { get; set; }
        public short ZVelocity { get; set; }

        public void Decode(MinecraftStream minecraftStream)
        {
            EntityId = minecraftStream.ReadVarInt();
            XVelocity = minecraftStream.ReadShort();
            YVelocity = minecraftStream.ReadShort();
            ZVelocity = minecraftStream.ReadShort();
        }

        public void Encode(MinecraftStream minecraftStream)
        {
            minecraftStream.WriteVarInt(EntityId);
            minecraftStream.WriteShort(XVelocity);
            minecraftStream.WriteShort(YVelocity);
            minecraftStream.WriteShort(ZVelocity);
        }
    }
}
