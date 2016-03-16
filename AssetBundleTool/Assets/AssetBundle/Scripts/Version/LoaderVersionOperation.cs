using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class LoaderVersionOperation
{
    private VersionConfig serverConfig;
    private VersionConfig localConfig;
    private List<VersionConfigItem> removeList = new List<VersionConfigItem>();
    private List<VersionConfigItem> downLoadList = new List<VersionConfigItem>();

    public void ParseServerConfig(string text)
    {
        serverConfig = new VersionConfig(text);
    }
    public void ParseLocalConfig(string text)
    {
        localConfig = new VersionConfig(text);
    }
    public List<VersionConfigItem> RemoveList
    {
        get
        {
            return removeList;
        }
    }
    public List<VersionConfigItem> DownLoadList
    {
        get
        {
            return downLoadList;
        }
    }
    public void ComparisonConfig()
    {
        Dictionary<string, VersionConfigItem> serverItems = serverConfig.Items;
        Dictionary<string, VersionConfigItem> localItems = localConfig.Items;
        foreach (KeyValuePair<string, VersionConfigItem> kvp in serverItems)
        {
            if (localItems.ContainsKey(kvp.Key))
            {
                if (localItems[kvp.Key].Md5 != kvp.Value.Md5)
                {
                    downLoadList.Add(kvp.Value);
                }
            }
            else
            {
                downLoadList.Add(kvp.Value);
            }
        }
        foreach (KeyValuePair<string, VersionConfigItem> kvp in localItems)
        {
            if (serverItems.ContainsKey(kvp.Key))
            {
                if (serverItems[kvp.Key].Md5 != kvp.Value.Md5)
                {
                    removeList.Add(kvp.Value);
                }
            }
            else
            {
                removeList.Add(kvp.Value);
            }
        }
    }
}
