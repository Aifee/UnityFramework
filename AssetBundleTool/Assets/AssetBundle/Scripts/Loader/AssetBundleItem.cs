using UnityEngine;
using System;
using System.Collections;

public class AssetBundleItem {

    public object Data { get; private set; }
    public string AssetbundleName { get; private set; }
    private int referencedCount;
    public System.Type Type { get; private set; }
    public AssetBundleItem(object obj,string abName)
    {
        Type = obj.GetType();
        this.Data = obj;
        this.AssetbundleName = abName;
        this.referencedCount = 1;
    }
    public void Retain()
    {
        this.referencedCount++;
    }

    public void Release()
    {
        if (Type != typeof(AssetBundle))
            return;
        this.referencedCount--;
        if (this.referencedCount == 0)
        {
            ((AssetBundle)Data).Unload(false);
        }
    }
    public int RetainCount
    {
        get
        {
            return this.referencedCount;
        }
    }
}
