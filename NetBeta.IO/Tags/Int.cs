using System;
using System.Net;

namespace NetBeta.IO.Tags;

public class Int : Tag
{
    public override byte GetID()
    {
        throw new NotImplementedException();
    }

    public override void Load(BinaryReader binaryReader)
    {
        Data = IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());
    }

    public override MemoryStream Save()
    {
        throw new NotImplementedException();
    }
}
