using System;
using NetBeta.IO.Util;

namespace NetBeta.Net.Packets;

public class PlayerLook : Packet
{
    public float Yaw;
    public float Pitch;
    public bool OnGround;

    public override byte GetID()
    {
        return (byte)Packet.PacketTypes.PlayerLook;
    }

    public override void Load(BinaryReader reader)
    {
        Yaw = Converter.GetFloat(reader);
        Pitch = Converter.GetFloat(reader);
        OnGround = reader.ReadBoolean();
    }

    public override byte[] Send()
    {
        throw new NotImplementedException();
    }
}
