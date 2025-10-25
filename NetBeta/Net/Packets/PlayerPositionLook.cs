using System;
using NetBeta.IO.Util;

namespace NetBeta.Net.Packets;

public class PlayerPositionLook : Packet
{
    public double X;
    public double Y;
    public double Stance;
    public double Z;
    public float Yaw;
    public float Pitch;
    public bool OnGround;

    public override byte GetID()
    {
        return (byte)Packet.PacketTypes.PlayerPositionLook;
    }

    public override void Load(BinaryReader reader)
    {
        X = Converter.GetDouble(reader);
        Stance = Converter.GetDouble(reader);
        Y = Converter.GetDouble(reader);
        Z = Converter.GetDouble(reader);
        Yaw = Converter.GetFloat(reader);
        Pitch = Converter.GetFloat(reader);
        OnGround = reader.ReadBoolean();
    }

    public override byte[] Send()
    {
        using MemoryStream memoryStream = new();
        using BinaryWriter writer = new(memoryStream);

        writer.Write(GetID());
        writer.Write(Converter.WriteDouble(X));
        writer.Write(Converter.WriteDouble(Y));
        writer.Write(Converter.WriteDouble(Stance));
        writer.Write(Converter.WriteDouble(Z));
        writer.Write(Converter.WriteFloat(Yaw));
        writer.Write(Converter.WriteFloat(Pitch));
        writer.Write(OnGround);

        return memoryStream.ToArray();
    }
}
