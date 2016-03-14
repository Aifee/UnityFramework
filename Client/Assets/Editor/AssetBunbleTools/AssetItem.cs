using UnityEngine;
using UnityEditor;
using System.Collections;
[System.Serializable]
public class AssetItem  {

    public string MD5;
    public string FullName;
    public string Name;
    public string Path;
    public string Type;
    [System.NonSerialized]
    public Object Obj;

    public void DeserializeObj() {
        Obj = AssetDatabase.LoadAssetAtPath(Path, typeof(Object));
    }
}
