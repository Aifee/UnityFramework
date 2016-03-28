//----------------------------------------------
//            MobArts PiDan Project
// Copyright © 2010-2015 MobArts Entertainment
// Created by : Liu Aifei (329737941@qq.com)
//----------------------------------------------

using UnityEngine;
using System;
using System.Collections;
using System.IO;
using ProtoBuf;
using System.Net.Sockets;

/// <summary>
/// @Summary : Protocol package
/// </summary>
public class Packet 
{
    public short Serial { get; set; }
    public byte Version { get; private set; }
    public byte Encryption { get; private set; }
    public short MessageId { get; private set; }
    public IExtensible MessageBody { get; private set; }
    public Socket Socket { get; set; }
    public bool IsStart { get; set; }
    public Packet(byte version, byte encryption, short messageId, IExtensible messageBody = null) {
        Version = version;
        Encryption = encryption;
        MessageId = messageId;
        MessageBody = messageBody;
    }

    public byte[] GetBuffer() {
        int offset = 10;
        byte[] buffer = null;
        byte[] data = UnitySocket.Serialize(MessageBody);
        if (data != null) {
            int length = data.Length + offset;
            byte[] sizeByte = BitConverter.GetBytes(length);
            Array.Reverse(sizeByte);
            byte[] serialByte = BitConverter.GetBytes(Serial);
            Array.Reverse(serialByte);
            byte[] headByte = BitConverter.GetBytes(MessageId);
            Array.Reverse(headByte);
            buffer = new byte[length];

            Array.Copy(sizeByte, buffer, 4);
            Array.Copy(serialByte, 0, buffer, 4, 2);
            buffer[6] = Version;
            buffer[7] = Encryption;
            Array.Copy(headByte, 0, buffer, 8, 2);
            Array.Copy(data, 0, buffer, offset, data.Length);
        }
        return buffer;
    }
}
