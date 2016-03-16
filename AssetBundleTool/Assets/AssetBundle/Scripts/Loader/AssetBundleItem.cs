using UnityEngine;
using System.Collections;

public class AssetBundleItem {

    public AssetBundle Assetbundle { get; private set; }
    public string AssetbundleName { get; private set; }
    private int referencedCount;

    public AssetBundleItem(AssetBundle ab,string abName)
    {
        this.Assetbundle = ab;
        this.AssetbundleName = abName;
        this.referencedCount = 1;
    }
    public void Retain()
    {
        this.referencedCount++;
    }

    public void Release()
    {
        this.referencedCount--;
        if (this.referencedCount == 0)
        {
            this.Assetbundle.Unload(false);
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
