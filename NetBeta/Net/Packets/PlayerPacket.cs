using System;

namespace NetBeta.Net.Packets;

public class PlayerPacket : Packet
{
    public bool OnGround;

    public override byte GetID()
    {
        return (byte)Packet.PacketTypes.Player;
    }

    public override void Load(BinaryReader reader)
    {
        OnGround = reader.ReadBoolean();
    }

    public override byte[] Send()
    {
        throw new NotImplementedException();
    }
}
