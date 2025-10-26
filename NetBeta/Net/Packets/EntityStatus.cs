using System;
using NetBeta.IO.Util;

namespace NetBeta.Net.Packets;

public class EntityStatus(int EntityID, byte Status) : Packet
{
    public override byte GetID()
    {
        return (byte)PacketTypes.EntityStatus;
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
        writer.Write(Status);

        return memoryStream.ToArray();
    }
}
