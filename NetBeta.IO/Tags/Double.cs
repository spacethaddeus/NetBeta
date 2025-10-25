using System;
using System.Net;
using NetBeta.IO.Util;

namespace NetBeta.IO.Tags;

public class Double : Tag
{
   public override byte GetID()
    {
        throw new NotImplementedException();
    }

    public override void Load(BinaryReader binaryReader)
    {
        Data = Converter.GetDouble(binaryReader);
    }

    public override MemoryStream Save()
    {
        throw new NotImplementedException();
    }
}
