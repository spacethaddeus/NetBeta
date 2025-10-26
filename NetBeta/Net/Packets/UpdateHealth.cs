using System;
using NetBeta.IO.Util;

namespace NetBeta.Net.Packets;

public class UpdateHealth(short Health) : Packet
{
    public override byte GetID()
    {
        return (byte)PacketTypes.UpdateHealth;
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
        writer.Write(Converter.WriteShort(Health));

        return memoryStream.ToArray();
    }
}
