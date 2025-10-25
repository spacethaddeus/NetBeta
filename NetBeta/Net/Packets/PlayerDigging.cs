using System;
using NetBeta.IO.Util;

namespace NetBeta.Net.Packets;

public class PlayerDigging : Packet
{
    public byte Status;
    public int X;
    public byte Y;
    public int Z;
    public byte Face;

    public override byte GetID()
    {
        return (byte)PacketTypes.PlayerDigging;
    }

    public override void Load(BinaryReader reader)
    {
        Status = reader.ReadByte();
        X = Converter.GetInt(reader);
        Y = reader.ReadByte();
        Z = Converter.GetInt(reader);
        Face = reader.ReadByte();
    }

    public override byte[] Send()
    {
        using MemoryStream memoryStream = new();
        using BinaryWriter writer = new(memoryStream);

        writer.Write(GetID());
        writer.Write(Status);
        writer.Write(Converter.WriteInt(X));
        writer.Write(Y);
        writer.Write(Converter.WriteInt(Z));
        writer.Write(Face);

        return memoryStream.ToArray();
    }
}
