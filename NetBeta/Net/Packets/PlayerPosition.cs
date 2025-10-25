using System;
using NetBeta.IO.Util;

namespace NetBeta.Net.Packets;

public class PlayerPosition : Packet
{
    public double X;
    public double Y;
    public double Stance;
    public double Z;
    public bool OnGround;

    public override byte GetID()
    {
        return (byte)Packet.PacketTypes.PlayerPosition;
    }

    public override void Load(BinaryReader reader)
    {
        X = Converter.GetDouble(reader);
        Y = Converter.GetDouble(reader);
        Stance = Converter.GetDouble(reader);
        Z = Converter.GetDouble(reader);
        OnGround = reader.ReadBoolean();
    }

    public override byte[] Send()
    {
        throw new NotImplementedException();
    }
}