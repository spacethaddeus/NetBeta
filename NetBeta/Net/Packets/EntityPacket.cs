using System;
using NetBeta.IO.Util;

namespace NetBeta.Net.Packets;

public class EntityPacket(int EntityID) : Packet
{
    public override byte GetID()
    {
        return (byte)PacketTypes.Entity;
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

        return memoryStream.ToArray();
    }
}
