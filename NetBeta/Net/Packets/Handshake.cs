using System;
using NetBeta.IO.Util;

namespace NetBeta.Net.Packets;

public class Handshake : Packet
{
    public string Username = "";

    public override byte GetID()
    {
        return (byte)PacketTypes.Handshake;
    }

    public override void Load(BinaryReader reader)
    {
        Username = Converter.GetString(reader);
    }

    public override byte[] Send()
    {
        using MemoryStream memoryStream = new();
        using BinaryWriter writer = new(memoryStream);

        writer.Write(GetID());
        writer.Write(Converter.WriteString("-"));

        return memoryStream.ToArray();
    }
}
