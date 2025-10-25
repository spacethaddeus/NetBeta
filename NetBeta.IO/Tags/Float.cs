using System;
using System.Net;

namespace NetBeta.IO.Tags;

public class Float : Tag
{
    public override byte GetID()
    {
        throw new NotImplementedException();
    }

    public override void Load(BinaryReader binaryReader)
    {
        byte[] FloatBytes = binaryReader.ReadBytes(4);
        Array.Reverse(FloatBytes);
        Data = BitConverter.ToSingle(FloatBytes);
    }

    public override MemoryStream Save()
    {
        throw new NotImplementedException();
    }
}
