using System;
using NetBeta.IO.Util;

namespace NetBeta.Net.Packets;

public class MapChunk(int X, short Y, int Z, byte[] CompressedData) : Packet
{
    public override byte GetID()
    {
        return (byte)PacketTypes.MapChunk;
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
        writer.Write(Converter.WriteShort(Y));
        writer.Write(Converter.WriteInt(Z));
        writer.Write((byte)15);
        writer.Write((byte)127);
        writer.Write((byte)15);
        writer.Write(Converter.WriteInt(CompressedData.Length));
        writer.Write(CompressedData);

        return memoryStream.ToArray();
    }
}
