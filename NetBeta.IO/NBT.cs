using System;
using System.IO;
using NetBeta.IO.Util;
using NetBeta.IO.Tags;
using System.IO.Compression;
using System.Text;

namespace NetBeta.IO;

public class NBT
{
    readonly string FilePath = "";
    public Compound Root = new();

    public NBT(string FilePath)
    {
        this.FilePath = FilePath;
        Load();
    }

    private void Load()
    {
        FileStream file = File.Open(FilePath, FileMode.Open);
        GZipStream gZipStream = new(file, CompressionMode.Decompress, true);

        BinaryReader binaryReader = new(gZipStream, Encoding.UTF8, true);
        byte TypeID = binaryReader.ReadByte();

        if (TypeID != (byte)Tag.TypeID.TAG_Compound)
            throw new Exception("There is no compound tag");

        byte[] BytesName = binaryReader.ReadBytes(Converter.GetShort(binaryReader));

        Compound Root = new()
        {
            Name = Encoding.UTF8.GetString(BytesName),
        };

        Root.Load(binaryReader);
        this.Root = Root;
        file.Close();
    }
    
    private void Save(string FilePath)
    {
        //First, we get the entire data first.
        MemoryStream memoryStream = new();

        memoryStream = Root.Save();
    }
}
