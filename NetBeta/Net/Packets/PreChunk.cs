using System;
using NetBeta.IO.Util;

namespace NetBeta.Net.Packets;

public class PreChunk(int X, int Z, bool Mode) : Packet
{
    public override byte GetID()
    {
        return (byte)PacketTypes.PreChunk;
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
        writer.Write(Converter.WriteInt(Z));
        writer.Write(Mode);

        return memoryStream.ToArray();
    }
}
