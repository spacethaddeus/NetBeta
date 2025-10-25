using System;
using NetBeta.IO.Util;

namespace NetBeta.Net.Packets;

public class Animation : Packet
{
    int EntityID = 0;
    byte Animate = 0;
    public override byte GetID()
    {
        return (byte)PacketTypes.Animation;
    }

    public override void Load(BinaryReader reader)
    {
        EntityID = Converter.GetInt(reader);
        Animate = reader.ReadByte();
    }

    public override byte[] Send()
    {
        using MemoryStream memoryStream = new();
        using BinaryWriter writer = new(memoryStream);

        writer.Write(GetID());
        writer.Write(Converter.WriteInt(EntityID));
        writer.Write(Animate);

        return memoryStream.ToArray();
    }
}
