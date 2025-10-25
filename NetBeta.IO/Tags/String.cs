using System;
using System.Text;
using NetBeta.IO.Util;

namespace NetBeta.IO.Tags;

public class String : Tag
{
    public override byte GetID()
    {
        throw new NotImplementedException();
    }

    public override void Load(BinaryReader binaryReader)
    {
        Data = Converter.GetString(binaryReader);
    }

    public override MemoryStream Save()
    {
        throw new NotImplementedException();
    }
}
