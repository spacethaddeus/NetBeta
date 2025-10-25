using System;
using NetBeta.IO.Util;

namespace NetBeta.Net.Packets;

public class Time(long TimeLong = 0L) : Packet
{
    public override byte GetID()
    {
        return (byte)PacketTypes.TimeUpdate;
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
        writer.Write(Converter.WriteLong(TimeLong));

        return memoryStream.ToArray();
    }
}
