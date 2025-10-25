using System;
using NetBeta.IO.Util;

namespace NetBeta.Net.Packets;

public class NamedEntitySpawn(int EntityID, string PlayerName, int X, int Y, int Z, byte Rotation, byte Pitch, short CurrentItem) : Packet
{
    public override byte GetID()
    {
        return (byte)PacketTypes.NamedEntitySpawn;
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
        writer.Write(Converter.WriteInt(EntityID));
        writer.Write(Converter.WriteString(PlayerName));
        writer.Write(Converter.WriteInt(X));
        writer.Write(Converter.WriteInt(Y));
        writer.Write(Converter.WriteInt(Z));
        writer.Write(Rotation);
        writer.Write(Pitch);
        writer.Write(Converter.WriteShort(CurrentItem));

        return memoryStream.ToArray();
    }
}
