using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;

public class VersionConfig  {
    public string version;
    private Dictionary<string, VersionConfigItem> items = new Dictionary<string, VersionConfigItem>();
    public VersionConfig(string text)
    {
        ParseConfig(text);
    }
    public int TotalSize
    {
        get
        {
            int size = 0;
            foreach (VersionConfigItem item in items.Values)
            {
                size += item.Size;
            }
            return size;
        }
    }
    public Dictionary<string, VersionConfigItem> Items
    {
        get
        {
            if (items == null)
                items = new Dictionary<string, VersionConfigItem>();
            return items;
        }
    }
    private void ParseConfig(string text)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(text);
        XmlNode root = xmlDoc.SelectSingleNode("files");
        XmlElement rootElement = root as XmlElement;
        version = rootElement.GetAttribute("version");
        XmlNodeList nodeList = root.ChildNodes;
        foreach (XmlNode xmlNode in nodeList)
        {
            VersionConfigItem item = new VersionConfigItem();
            XmlElement element = xmlNode as XmlElement;
            item.Path = element.GetAttribute("path");
            item.Md5 = element.GetAttribute("md5");
            item.Size = int.Parse(element.GetAttribute("size"));
            if (items.ContainsKey(item.Path))
            {
                items[item.Path] = item;
            }
            else
            {
                items.Add(item.Md5, item);
            }
        }
    }
}
public class VersionConfigItem
{
    public string Path;
    public string Md5;
    public int Size;
}
