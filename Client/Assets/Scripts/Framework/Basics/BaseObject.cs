using UnityEngine;
using System;
using System.Collections;
using System.Reflection;

[Serializable]
public class BaseObject : object,ICloneable {

    public object Clone() {
        using (System.IO.Stream objectStream = new System.IO.MemoryStream()) {
            System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            formatter.Serialize(objectStream, this);
            objectStream.Seek(0, System.IO.SeekOrigin.Begin);
            return formatter.Deserialize(objectStream);
        }
    }

    public override string ToString() {
        string str = this.GetType() + "\n";
        FieldInfo[] fields = this.GetType().GetFields();
        foreach (FieldInfo info in fields) {
            str += string.Format("({0})   {1} : {2}\n", info.FieldType, info.Name, info.GetValue(this));
        }
        return str;

    }
}
