using System;
using NetBeta.Core.Entities;

namespace NetBeta.Net.Packets;

public class AddObjectVehicle(int EntityID, byte Type, int X, int Y, int Z) : Packet
{
    public override byte GetID()
    {
        return (byte)PacketTypes.AddObjectVehicle;
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
        writer.Write(IO.Util.Converter.WriteInt(EntityID));
        writer.Write(Type);
        writer.Write(IO.Util.Converter.WriteInt(X * 32));
        writer.Write(IO.Util.Converter.WriteInt(Y * 32));
        writer.Write(IO.Util.Converter.WriteInt(Z * 32));

        return memoryStream.ToArray();
    }
}
