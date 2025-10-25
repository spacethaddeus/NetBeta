using System;
using System.Net;
using NetBeta.IO.Util;

namespace NetBeta.IO.Tags;

public class ByteArray : Tag
{
    public override byte GetID()
    {
        throw new NotImplementedException();
    }

    public override void Load(BinaryReader binaryReader)
    {
        int ArrayLength = IPAddress.NetworkToHostOrder(
            BitConverter.ToInt32(binaryReader.ReadBytes(4)));

        Data = new byte[ArrayLength];

        for(int i = 0; i < ArrayLength; i++)
        {
            Data[i] = binaryReader.ReadByte();
        }
    }

    public override MemoryStream Save()
    {
        throw new NotImplementedException();
    }
}
