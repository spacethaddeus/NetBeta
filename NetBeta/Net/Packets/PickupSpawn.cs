using System;
using NetBeta.Core.Entities;

namespace NetBeta.Net.Packets;

public class PickupSpawn(Pickup entity) : Packet
{
    public override byte GetID()
    {
        return (byte)PacketTypes.PickupSpawn;
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
        writer.Write(IO.Util.Converter.WriteInt(entity.GetEntityID()));
        writer.Write(IO.Util.Converter.WriteShort(entity.Item));
        writer.Write(entity.Count);
        writer.Write(IO.Util.Converter.WriteInt((int)entity.Position.X * 32));
        writer.Write(IO.Util.Converter.WriteInt((int)entity.Position.Y * 32));
        writer.Write(IO.Util.Converter.WriteInt((int)entity.Position.Z * 32));
        writer.Write(0);
        writer.Write(0);
        writer.Write(0);

        return memoryStream.ToArray();
    }
}
