using System;

namespace NetBeta.IO.Tags;

public class Byte : Tag
{
    public override byte GetID()
    {
        throw new NotImplementedException();
    }

    public override void Load(BinaryReader binaryReader)
    {
        Data = binaryReader.ReadByte();
    }

    public override MemoryStream Save()
    {
        throw new NotImplementedException();
    }
}
