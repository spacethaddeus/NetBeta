using System;
using NetBeta.IO.Util;

namespace NetBeta.Net.Packets;

public class EntityTeleport(int EntityID, int X, int Y, int Z, byte Yaw, byte Pitch) : Packet
{
    public override byte GetID()
    {
        return (byte)PacketTypes.EntityTeleport;
    }

    public override void Load(BinaryReader reader)
    {
        throw new NotImplementedException();
    }

    public override byte[] Send()
    {
        using MemoryStream memoryStream = new();
        using BinaryWriter writer = new(memoryStream);

        writer.Write(GetID());
        writer.Write(Converter.WriteInt(EntityID));
        writer.Write(Converter.WriteInt(X * 32));
        writer.Write(Converter.WriteInt((Y - 1) * 32));
        writer.Write(Converter.WriteInt(Z * 32));
        writer.Write(Yaw / 360);
        writer.Write(Pitch / 360);

        return memoryStream.ToArray();
    }
}
