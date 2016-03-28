using UnityEngine;
using System;
using System.Collections;

public class ByteRead {
    private int pointer = 0;
    private byte[] Data;
    public ByteRead(byte[] data) {
        Data = data;
        pointer = 0;
    }
    public bool ReadBool() {
        if (pointer > Data.Length - 1) {
            Debug.LogError("Endian has read to the end");
            return false;
        }
        return Data[pointer++] != 0;
    }
    public byte ReadByte() {
        if (pointer > Data.Length - 1) {
            Debug.LogError("Endian has read to the end");
            return 0;
        }
        return Data[pointer++];
    }
    public short ReadShort() {
        if (pointer > Data.Length - 2) {
            Debug.LogError("Endian has read to the end");
            return 0;
        }
        short value = BitConverter.ToInt16(Data,pointer);
        pointer += 2;
        return value;
    }
    public int ReadInt() {
        if (pointer > Data.Length - 3) {
            Debug.LogError("Endian has read to the end");
            return 0;
        }
        int value = BitConverter.ToInt32(Data, pointer);
        pointer += 4;
        return value;
    }
    public long ReadLong() {
        if (pointer > Data.Length - 7) {
            Debug.LogError("Endian has read to the end");
            return 0;
        }
        long value = BitConverter.ToInt64(Data, pointer);
        pointer += 8;
        return value;
    }
    public float ReadFloat() {
        if (pointer > Data.Length - 3) {
            Debug.LogError("Endian has read to the end");
            return 0;
        }
        float value = BitConverter.ToSingle(Data, pointer);
        pointer += 4;
        return value;
    }
    public double ReadDouble() {
        if (pointer > Data.Length - 7) {
            Debug.LogError("Endian has read to the end");
            return 0;
        }
        double value = BitConverter.ToDouble(Data, pointer);
        pointer += 8;
        return value;
    }
    public string ReadString() {
        if (pointer > Data.Length - 7) {
            Debug.LogError("Endian has read to the end");
            return "";
        }
        byte[] byteValue = new byte[8];
        Array.Copy(Data, pointer, byteValue, 0, byteValue.Length);
        string value = System.Text.Encoding.Default.GetString(byteValue);
        pointer += 8;
        return value;
    }
    public Vector2 ReadVector2() {
        return new Vector2(ReadFloat(), ReadFloat());
    }
    public Vector3 ReadVector3() {
        return new Vector3(ReadFloat(), ReadFloat(), ReadFloat());
    }
    public Vector4 ReadVector4() {
        return new Vector4(ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat());
    }

    public Quaternion ReadQuaternion() {
        return new Quaternion(ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat());
    }
}
