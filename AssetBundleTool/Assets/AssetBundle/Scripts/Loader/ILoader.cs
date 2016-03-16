using UnityEngine;
using System.Collections;

public interface ILoader
{
    T RecourcesLoad<T>(string assetName) where T : UnityEngine.Object;
    void AsynLoad<T>(string assetName, LoaderComplete complete) where T : UnityEngine.Object;
    void UnloadAssetBundle(string assetbundleName);
}
