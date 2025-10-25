using System;
using NetBeta.IO.Util;

namespace NetBeta.IO.Tags;

public class Long : Tag
{
    public override byte GetID()
    {
        throw new NotImplementedException();
    }

    public override void Load(BinaryReader binaryReader)
    {
        Data = Converter.GetLong(binaryReader);
    }

    public override MemoryStream Save()
    {
        throw new NotImplementedException();
    }
}
