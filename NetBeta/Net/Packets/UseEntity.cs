using System;
using NetBeta.IO.Util;

namespace NetBeta.Net.Packets;

public class UseEntity : Packet
{
    public int User;
    public int Target;
    public bool LeftClick = false;
    
    public override byte GetID()
    {
        throw new NotImplementedException();
    }

    public override void Load(BinaryReader reader)
    {
        User = Converter.GetInt(reader);
        Target = Converter.GetInt(reader);
        LeftClick = reader.ReadBoolean();
    }

    public override byte[] Send()
    {
        throw new NotImplementedException();
    }
}
