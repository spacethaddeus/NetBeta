using System;
using NetBeta.IO.Util;

namespace NetBeta.Net.Packets;

public class BlockChange(int X, byte Y, int Z, byte BlockType, byte BlockMetaData) : Packet
{
    public override byte GetID()
    {
        return (byte)PacketTypes.BlockChange;
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
        writer.Write(Y);
        writer.Write(Converter.WriteInt(Z));
        writer.Write(BlockType);
        writer.Write(BlockMetaData);

        return memoryStream.ToArray();
    }
}
