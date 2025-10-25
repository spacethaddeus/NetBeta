using System;
using System.Diagnostics.CodeAnalysis;

namespace NetBeta.IO.Tags;

public abstract class Tag
{
    public dynamic Data { get; set; } = null!;
    public string Name = "";
    public abstract byte GetID();
    public abstract MemoryStream Save();
    public abstract void Load(BinaryReader binaryReader);

    public enum TypeID : byte
    {
        TAG_End = 0x00,
        TAG_Byte = 0x01,
        TAG_Short = 0x02,
        TAG_Int = 0x03,
        TAG_Long = 0x04,
        TAG_Float = 0x05,
        TAG_Double = 0x06,
        TAG_Byte_Array = 0x07,
        TAG_String = 0x08,
        TAG_List = 0x09,
        TAG_Compound = 0x0A,
        TAG_Int_Array = 0x0B,
        TAG_Long_Array = 0x0C,
    }

    public static Tag GetTag(byte TypeID)
    {
        return TypeID switch
        {
            (byte)Tag.TypeID.TAG_Byte => new Byte(),
            (byte)Tag.TypeID.TAG_Compound => new Compound(),
            (byte)Tag.TypeID.TAG_String => new String(),
            (byte)Tag.TypeID.TAG_Byte_Array => new ByteArray(),
            (byte)Tag.TypeID.TAG_Short => new Short(),
            (byte)Tag.TypeID.TAG_Int => new Int(),
            (byte)Tag.TypeID.TAG_Float => new Float(),
            (byte)Tag.TypeID.TAG_Long => new Long(),
            (byte)Tag.TypeID.TAG_List => new List(),
            (byte)Tag.TypeID.TAG_Double => new Double(),
            _ => throw new Exception($"Tag not found! Tag ({Enum.GetName(typeof(Tag.TypeID), TypeID)}): {TypeID}"),
        };
    }
}
