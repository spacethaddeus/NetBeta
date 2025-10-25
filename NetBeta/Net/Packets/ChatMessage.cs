using System;
using NetBeta.IO.Util;

namespace NetBeta.Net.Packets;

public class ChatMessage : Packet
{
    public string Message = "";
    public override byte GetID()
    {
        return (byte)PacketTypes.ChatMessage;
    }

    public string GetMessage()
    {
        return Message;
    }

    public override void Load(BinaryReader reader)
    {
        Message = Converter.GetString(reader);
    }

    public override byte[] Send()
    {
        using MemoryStream memoryStream = new();
        using BinaryWriter writer = new(memoryStream);

        writer.Write(GetID());
        writer.Write(Converter.WriteString(Message));

        return memoryStream.ToArray();
    }
}
