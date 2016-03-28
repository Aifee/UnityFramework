#define LOADER_DEBUG
//#undef LOADER_DEBUG
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 加载回调函数，先检查是否有错误码，如果错误码为null或者“”的情况下加载才算成功
/// </summary>
/// <param name="error"> 错误码 </param>
/// <param name="item"> 所加载的assetbundle信息 </param>
public delegate void LoaderComplete(string error,AssetBundleItem item);

public class Loader : MonoBehaviour,ILoader
{

    public const string PREFABS_PATH = "Prefabs/";
    public const string MATERIALS_PATH = "Materials/";
    public const string TEXTURES_PATH = "Textures/";
    public const string ANIMATORS_PATH = "Animators/";

    #region Instance

    private static object m_asynObject = new object();
    private static GameObject instanceGo;
    private static Loader instance;
    public static Loader Instance
    {
        get
        {
            lock (m_asynObject)
            {
                if (instanceGo == null)
                {
                    instanceGo = new GameObject("Loader");
                }
                if (instance == null)
                {
                    instance = instanceGo.GetComponent<Loader>() == null ? instanceGo.AddComponent<Loader>() : instanceGo.GetComponent<Loader>();
                }
                return instance;
            }
        }
    }

    #endregion

    /// <summary>
    /// AssetBundleName AssetBundleItem
    /// </summary>
    private Dictionary<string, AssetBundleItem> assetbundles = new Dictionary<string, AssetBundleItem>();
    /// <summary>
    /// 加载完成回调集合，
    /// </summary>
    private Dictionary<string, LoaderComplete> completes = new Dictionary<string, LoaderComplete>();

    private Queue<string> dependenciesQuest = new Queue<string>();

    private AssetBundleManifest manifest;
    public Loader()
    {

    }
    public T RecourcesLoad<T>(string assetName) where T : UnityEngine.Object
    {
        string path = "";
        Type type = typeof(T);
        if (type == typeof(GameObject))
        {
            path += PREFABS_PATH;
        }
        else if (type == typeof(Material))
        {
            path += MATERIALS_PATH;
        }
        else if (type == typeof(Texture))
        {
            path += TEXTURES_PATH;
        }
        else if (type == typeof(Animator))
        {
            path += MATERIALS_PATH;
        }
        path += assetName;
        return Resources.Load<T>(assetName);
    }
    public void AssetBundleLoad<T>(string assetName, LoaderComplete complete) where T : UnityEngine.Object
    {
        if (completes.ContainsKey(assetName))
        {
            Debug.LogError("Load to load the file already exists in the queue, please check it! :" + assetName);
            return;
        }
        AssetBundleItem item = IsLoadedAssetBundle(assetName);
        if (item != null)
        {
            complete("", item);
            return;
        }
        completes.Add(assetName, complete);
        StartCoroutine(AsynLoadSingle<T>(assetName));
    }
    
    private AssetBundleItem IsLoadedAssetBundle(string assetName)
    {
        AssetBundleItem item = null;
        assetbundles.TryGetValue(assetName, out item);
        if (item != null)
        {
            item.Retain();
            return item;
        }
        return null;
    }
    /// <summary>
    /// 加载单个资源成功后回调
    /// </summary>
    /// <param name="url"></param>
    /// <param name="complete"></param>
    /// <param name="assetName"></param>
    /// <returns></returns>
    private IEnumerator AsynLoadSingle<T>(string assetName) where T : UnityEngine.Object
    {
        string url = assetName;
        Debug.Log("url :" + url);
        WWW www = new WWW(url);
        yield return www;
        LoaderComplete complete = null;
        completes.TryGetValue(assetName, out complete);
        if (www.error != null)
        {
            Debug.LogError(www.error);
            complete(www.error, null);
        }
        else
        {
            object obj = null;
            if (typeof(T) == typeof(AssetBundle))
            {
                obj = www.assetBundle;
            }
            else if (typeof(T) == typeof(TextAsset))
            {
                obj = www.text;
            }
            else if (typeof(T) == typeof(Texture))
            {
                obj = www.texture;
            }
            else if (typeof(T) == typeof(AudioClip))
            {
                obj = www.audioClip;
            }
            else if (typeof(T) == typeof(byte[]))
            {
                obj = www.bytes;
            }
            AssetBundleItem item = new AssetBundleItem(obj, assetName);
            assetbundles.Add(assetName, item);
            if (manifest == null)
            {
                complete("", item);
            }
            else
            {
                string[] dependencies = manifest.GetDirectDependencies(assetName);
                if (dependencies != null && dependencies.Length >= 0)
                {
                    for(int i = 0, iMax = dependencies.Length; i < iMax; i ++){
                        dependenciesQuest.Enqueue(dependencies[i]);
                    }
                    StartCoroutine(AsynLoadDependQuest(assetName));
                }
                else
                {
                    complete("", item);
                }
            }
        }
        www.Dispose();
    }
    private IEnumerator AsynLoadDependQuest(string assetName)
    {
        LoaderComplete complete = null;
        completes.TryGetValue(assetName, out complete);
        AssetBundleItem item = null;
        assetbundles.TryGetValue(assetName, out item);

        if (dependenciesQuest.Count > 0)
        {
            string url = dependenciesQuest.Dequeue();
            WWW www = new WWW(url);
            yield return www;
            if (www.error != null)
            {
                complete(www.error, null);
                www.Dispose();
            }
            else
            {
                StartCoroutine(AsynLoadDependQuest(assetName));
            }
        }
        else
        {
            complete("", item);
        }
    }
    
    public void UnloadAssetBundle(string assetbundleName)
    {
        AssetBundleItem item = null;
        assetbundles.TryGetValue(assetbundleName, out item);
        if (item == null)
        {
            return;
        }
        if (manifest != null)
        {
            string[] dependencies = manifest.GetDirectDependencies(assetbundleName);
            if (dependencies.Length > 0)
            {
                for (int i = 0, iMax = dependencies.Length; i < iMax; i++)
                {
                    UnloadAssetBundle(dependencies[i]);
                }
            }
        }
        item.Release();
    }

}
