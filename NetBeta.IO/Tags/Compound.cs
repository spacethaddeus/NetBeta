using System;
using System.Collections;
using System.Text;
using NetBeta.IO.Util;

namespace NetBeta.IO.Tags;

public class Compound : Tag
{
    public List<Tag> arrayList = [];
    public override byte GetID()
    {
        return (byte)TypeID.TAG_Compound;
    }

    public override void Load(BinaryReader binaryReader)
    {
        Data = new ArrayList();
        while(true)
        {
            byte TypeID = binaryReader.ReadByte();

            if (TypeID == (byte)Tag.TypeID.TAG_End)
                return;

            byte[] BytesName = binaryReader.ReadBytes(Converter.GetShort(binaryReader));

            Tag tag = GetTag(TypeID);
            tag.Name = Encoding.UTF8.GetString(BytesName);

            tag.Load(binaryReader);
            arrayList.Add(tag);
        }
    }

    public override MemoryStream Save()
    {
        MemoryStream memoryStream = new();
        memoryStream.WriteByte(GetID());
        foreach (Tag tag in arrayList)
        {
            tag.Save().CopyTo(memoryStream);
        }
        return memoryStream;
    }

    public dynamic Get(string PropertyName)
    {
        try
        {
            foreach (Tag tag in arrayList)
            {
                if (tag.Name == PropertyName)
                {
                    return tag.Data;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        throw new Exception($"Not found! {PropertyName}");
    }

    public Compound GetCompound(string PropertyName)
    {
        try
        {
            foreach (Compound tag in arrayList.Cast<Compound>())
            {
                if (tag.Name == PropertyName)
                {
                    return tag;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        throw new Exception("Not found!");
    }
}
