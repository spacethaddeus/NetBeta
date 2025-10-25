using System;
using System.Net;

namespace NetBeta.IO.Tags;

public class List : Tag
{
    public override byte GetID()
    {
        throw new NotImplementedException();
    }

    public override void Load(BinaryReader binaryReader)
    {
        List<Tag> List = [];
        byte TypeID = binaryReader.ReadByte();
        int ArrayLength = IPAddress.NetworkToHostOrder(
            BitConverter.ToInt32(binaryReader.ReadBytes(4)));

        for (int i = 0; i < ArrayLength; i++)
        {
            Tag tag = GetTag(TypeID);
            tag.Load(binaryReader);
            List.Add(tag);
        }

        Data = List;
    }

    public override MemoryStream Save()
    {
        throw new NotImplementedException();
    }
}
