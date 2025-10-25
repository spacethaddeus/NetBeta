using System;
using System.Net;

namespace NetBeta.IO.Tags;

public class Short : Tag
{
    public override byte GetID()
    {
        throw new NotImplementedException();
    }

    public override void Load(BinaryReader binaryReader)
    {
        Data = (int)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());
    }

    public override MemoryStream Save()
    {
        throw new NotImplementedException();
    }
}
