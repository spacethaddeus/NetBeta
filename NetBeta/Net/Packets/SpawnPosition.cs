using System;
using NetBeta.IO.Util;

namespace NetBeta.Net.Packets;

public class SpawnPosition(int X, int Y, int Z) : Packet
{
    public override byte GetID()
    {
        return (byte)Packet.PacketTypes.SpawnPosition;
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
        writer.Write(Converter.WriteInt(X));
        writer.Write(Converter.WriteInt(Y));
        writer.Write(Converter.WriteInt(Z));

        return memoryStream.ToArray();
    }
}
