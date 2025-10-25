using System;

namespace NetBeta.Net.Packets;

public class KeepAlive : Packet
{
    public override byte GetID()
    {
        return (byte)Packet.PacketTypes.KeepAlive;
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

        return memoryStream.ToArray();
    }
}
