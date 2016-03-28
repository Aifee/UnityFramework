using UnityEngine;
using System.Collections;

public class VersionManager 
{
    public const string RESOURCESERVER_CONFIG = "ResourceServer";
    public const string VERSIONT_CONFIG = "AssetConfig";

    private LoaderVersionOperation version;
    public VersionManager()
    {
        version = new LoaderVersionOperation();
    }

    public void CheckVersion()
    {
        TextAsset textAsset = Loader.Instance.RecourcesLoad<TextAsset>(RESOURCESERVER_CONFIG);
        Debug.Log(textAsset.text);
        string url = string.Format("{0}{1}.xml", textAsset.text, VERSIONT_CONFIG);
        Debug.Log("url ＝ " + url);
        //Loader.Instance.AsynLoad<TextAsset>(url, LoadServerConfig);
    }
    private void LoadServerConfig(string error,AssetBundleItem item)
    {
        if (error != null && !error.Equals(""))
        {
            Debug.Log(error);
        }
        else
        {
            Debug.Log(item.AssetbundleName + "  :  " + item.Data);
        }
    }
}
