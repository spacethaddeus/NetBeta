using System;
using System.Net;
using System.Text;
using System.Text.Unicode;

namespace NetBeta.IO.Util;

public class Converter
{

    //https://minecraft.wiki/w/Minecraft_Wiki:Projects/wiki.vg_merge/Alpha_Map_Format
    public static string Base36Encode(long input)
    {
        if (input == 0) { return "0"; }
        string chars = "0123456789abcdefghijklmnopqrstuvwxyz";
        bool negative = (input < 0);

        StringBuilder sb = new();

        if (negative)
        {
            input = -input;
            sb.Append('-');
        }
        while (input > 0)
        {
            sb.Insert(negative ? 1 : 0, chars[(int)(input % 36)]);
            input /= 36;
        }
        return sb.ToString();
    }

    public static string GetString(BinaryReader reader)
    {
        short Length = GetShort(reader);
        byte[] StringBytes = reader.ReadBytes(Length);
        return Encoding.UTF8.GetString(StringBytes);
    }

    public static byte[] WriteString(string input)
    {
        short Length = (short)input.Length;

        MemoryStream memoryStream = new();
        memoryStream.Write(WriteShort(Length));
        memoryStream.Write(Encoding.UTF8.GetBytes(input));
        memoryStream.Close();

        return memoryStream.ToArray();

    }

    public static byte[] WriteInt(int input)
    {
        byte[] InputBytes = BitConverter.GetBytes(input);
        if (IsLittleEndian())
            Array.Reverse(InputBytes);
        return InputBytes;
    }

    public static int GetInt(BinaryReader binaryReader)
    {
        byte[] InputBytes = binaryReader.ReadBytes(4);
        if (IsLittleEndian())
            Array.Reverse(InputBytes);
        return BitConverter.ToInt32(InputBytes);
    }

    public static byte[] WriteShort(short input)
    {
        if (IsLittleEndian())
            input = IPAddress.HostToNetworkOrder(input);

        return BitConverter.GetBytes(input);
    }

    public static short GetShort(BinaryReader binaryReader)
    {
        byte[] InputBytes = binaryReader.ReadBytes(2);
        if (IsLittleEndian())
            Array.Reverse(InputBytes);
        return BitConverter.ToInt16(InputBytes, 0);
    }

    public static long GetLong(BinaryReader binaryReader)
    {
        byte[] InputBytes = binaryReader.ReadBytes(8);
        if (IsLittleEndian())
            Array.Reverse(InputBytes);
        return BitConverter.ToInt64(InputBytes, 0);
    }

    public static byte[] WriteLong(long input)
    {
        if (IsLittleEndian())
            IPAddress.HostToNetworkOrder(input);
        return BitConverter.GetBytes(input);
    }

    public static double GetDouble(BinaryReader binaryReader)
    {
        byte[] InputBytes = binaryReader.ReadBytes(8);
        if (IsLittleEndian())
            Array.Reverse(InputBytes);
        return BitConverter.ToDouble(InputBytes, 0);
    }

    public static byte[] WriteDouble(double input)
    {
        byte[] bytes = BitConverter.GetBytes(input);
        if (IsLittleEndian())
            Array.Reverse(bytes);
        return bytes;
    }

    public static byte[] WriteFloat(float input)
    {
        byte[] bytes = BitConverter.GetBytes(input);
        if (IsLittleEndian())
            Array.Reverse(bytes);
        return bytes;
    }

    public static float GetFloat(BinaryReader binaryReader)
    {
        byte[] InputBytes = binaryReader.ReadBytes(4);
        if (IsLittleEndian())
            Array.Reverse(InputBytes);
        return BitConverter.ToSingle(InputBytes, 0);
    }
    
    public static bool IsLittleEndian()
    {
        if (BitConverter.IsLittleEndian)
            return true;
        return false;
    }
}
