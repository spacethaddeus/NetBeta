using System;
using NetBeta.IO.Util;

namespace NetBeta.Net.Packets;

public class Login(int EntityID, long MapSeed, byte Dimension) : Packet
{
    public int PlayerProtocolVersion;
    public string Username = "";
    public string Password = "";

    public override byte GetID()
    {
        return (byte)PacketTypes.Login;
    }

    public override void Load(BinaryReader reader)
    {
        PlayerProtocolVersion = Converter.GetInt(reader);
        Username = Converter.GetString(reader);
        Password = Converter.GetString(reader);

        //For the last two values, we don't need it.
        Converter.GetLong(reader);
        reader.ReadByte();
    }

    public override byte[] Send()
    {
        using MemoryStream memoryStream = new(); 
        using BinaryWriter writer = new(memoryStream);

        writer.Write(GetID());
        writer.Write(Converter.WriteInt(EntityID));
        writer.Write(Converter.WriteString(""));
        writer.Write(Converter.WriteString(""));
        writer.Write(Converter.WriteLong(MapSeed));
        writer.Write(Dimension);

        return memoryStream.ToArray();
    }
}
