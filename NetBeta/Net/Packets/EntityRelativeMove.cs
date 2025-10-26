using System;
using NetBeta.IO.Util;

namespace NetBeta.Net.Packets;

public class EntityRelativeMove(int EntityID, byte DX, byte DY, byte DZ) : Packet
{
    public override byte GetID()
    {
        return (byte)PacketTypes.EntityRelativeMove;
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
        writer.Write(DX / 32);
        writer.Write((DY - 1) / 32);
        writer.Write(DZ / 32);

        return memoryStream.ToArray();
    }
}
