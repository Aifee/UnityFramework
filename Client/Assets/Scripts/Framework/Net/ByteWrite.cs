using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ByteWrite
{
    private List<byte> byteList = new List<byte>();
    public byte[] Data {
        get
        {
            return byteList.ToArray();
        }

    }
    public void WriteBool(bool value) {
        byteList.Add(value ? (byte)1 : (byte)0);
    }
    public void WriteByte(byte b) {
        byteList.Add(b);
    }
    public void WriteShort(short value) {
        byte[] bytes = BitConverter.GetBytes(value);
        foreach (byte b in bytes) {
            byteList.Add(b);
        }
    }
    public void WriteInt(int value) {
        byte[] bytes = BitConverter.GetBytes(value);
        foreach (byte b in bytes) {
            byteList.Add(b);
        }
    }
    public void WriteLong(long value) {
        byte[] bytes = BitConverter.GetBytes(value);
        foreach (byte b in bytes) {
            byteList.Add(b);
        }
    }
    public void WriteFloat(float value) {
        byte[] bytes = BitConverter.GetBytes(value);
        foreach (byte b in bytes) {
            byteList.Add(b);
        }
    }
    public void WriteDouble(double value) {
        byte[] bytes = BitConverter.GetBytes(value);
        foreach (byte b in bytes) {
            byteList.Add(b);
        }
    }
    public void WriteString(string value) {
        byte[] bytes = System.Text.Encoding.Default.GetBytes(value);
        //string str = System.Text.Encoding.Default.GetString(byte);
        foreach (byte b in bytes) {
            byteList.Add(b);
        }
    }
    public void WriteVector2(Vector2 value) {
        WriteFloat(value.x);
        WriteFloat(value.y);
    }
    public void WriteVector3(Vector3 value) {
        WriteFloat(value.x);
        WriteFloat(value.y);
        WriteFloat(value.z);
    }
    public void WriteVector4(Vector4 value) {
        WriteFloat(value.x);
        WriteFloat(value.y);
        WriteFloat(value.z);
        WriteFloat(value.w);
    }
    public void WriteQuaternion(Quaternion value) {
        WriteFloat(value.x);
        WriteFloat(value.y);
        WriteFloat(value.z);
        WriteFloat(value.w);
    }
    
}
