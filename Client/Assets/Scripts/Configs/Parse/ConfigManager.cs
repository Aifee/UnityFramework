//----------------------------------------------
//            liuaf threeKingdoms Project
// Copyright © 2015-2017 threeKingdoms
// Created by : Liu Aifei (329737941@qq.com)
//--------------------------------------------
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

public enum eConfigDataType{
    Unknown = 0,
    /// <summary> int </summary>
    INT = 1,
    /// <summary> float </summary>
    FLOAT = 2,
    /// <summary> string </summary>
    STR = 3,
    /// <summary> bool </summary>
    BOOL = 4,
    /// <summary> List [ int ] </summary>
    LIST_INT = 5,
    /// <summary> List [ string ] </summary>
    LIST_STR = 6,
    /// <summary> List [ float ] </summary>
    LIST_FLOAT = 7,
    /// <summary> bool </summary>
    LIST_BOOL = 8,
    /// <summary> Dictionary [ int,int ] </summary>
    DIC_INT_INT = 9,
    /// <summary> Dictionary [ int,string ] </summary>
    DIC_INT_STR = 10,
    /// <summary> Dictionary [ int,float ] </summary>
    DIC_INT_FLOAT = 11,
    /// <summary> Dictionary [ string,bool ] </summary>
    DIC_INT_BOOL = 12,
    /// <summary> Dictionary [ string,string ] </summary>
    DIC_STR_STR = 13,
    /// <summary> Dictionary [ string,int ] </summary>
    DIC_STR_INT = 14,
    /// <summary> Dictionary [ string,float ] </summary>
    DIC_STR_FLOAT = 15,
    /// <summary> Dictionary [ string,bool ] </summary>
    DIC_STR_BOOL = 16,
    JSON = 17,
}
public enum eConfigType {
    Xml,
    Json,
    Csv,
}

public class ConfigManager : Singleton<ConfigManager> {
    private static string CONFIG_PATH = "Configs/";
    static public eConfigDataType StringToValueType(string str) {
        eConfigDataType type = eConfigDataType.Unknown;
        switch (str) {
            case "INT": type = eConfigDataType.INT; break;
            case "FLOAT": type = eConfigDataType.FLOAT; break;
            case "STR": type = eConfigDataType.STR; break;
            case "BOOL": type = eConfigDataType.BOOL; break;
            case "LIST[INT]": type = eConfigDataType.LIST_INT; break;
            case "LIST[STR]": type = eConfigDataType.LIST_STR; break;
            case "LIST[FLOAT]": type = eConfigDataType.LIST_FLOAT; break;
            case "LIST[BOOL]": type = eConfigDataType.LIST_BOOL; break;
            case "DIC[INT,INT]": type = eConfigDataType.DIC_INT_INT; break;
            case "DIC[INT,FLOAT]": type = eConfigDataType.DIC_INT_FLOAT; break;
            case "DIC[INT,STR]": type = eConfigDataType.DIC_INT_STR; break;
            case "DIC[INT,BOOL]": type = eConfigDataType.DIC_INT_BOOL; break;
            case "DIC[STR,INT]": type = eConfigDataType.DIC_STR_INT; break;
            case "DIC[STR,FLOAT]": type = eConfigDataType.DIC_STR_FLOAT; break;
            case "DIC[STR,STR]": type = eConfigDataType.DIC_STR_STR; break;
            case "DIC[STR,BOOL]": type = eConfigDataType.DIC_STR_BOOL; break;
            case "JSON": type = eConfigDataType.JSON ;break;
        }
        return type;
    }


    private Dictionary<System.Type, Dictionary<int, Config>> configs = new Dictionary<System.Type, Dictionary<int, Config>>();
    public T GetConfig<T>(eConfigType type,int id) where T : Config, new() {
        T t = null;
        Dictionary<int, T> dic = null;
        if (configs.ContainsKey(typeof(T))) {
            dic = configs[typeof(T)] as Dictionary<int, T>;
        } else {
            dic = Analytical<T>(type);
            configs.Add(typeof(T), dic as Dictionary<int, Config>);
        }
        if (dic.ContainsKey(id)) {
            t = dic[id];
        } else {
            Debug.LogError(string.Format("No field data found {1} {0} the corresponding configuration table in", typeof(T),id));
        }
        return t;
    }

    private Dictionary<int, T> Analytical<T>(eConfigType type) where T : Config, new() {
        TextAsset asset = Load<T>();
        Debug.Log("Asset == " + asset.text);
        if (type == eConfigType.Xml) {
            return AnalyticalXML<T>(asset.text);
        } else if (type == eConfigType.Csv) {
            return AnalyticalCSV<T>(asset.text);
        } else if (type == eConfigType.Json) {
            return AnalyticalJSON<T>(asset.text);
        }
        return null;
        //string[][] configArray = AssetToArray(asset.text);
        //if (configArray == null) {
        //    Debug.LogError("Data format error, data line number must be at least 2 lines");
        //    return null;
        //}
        //Dictionary<int, T> dic = new Dictionary<int, T>();

        //string[] titleNames = configArray[0];
        //string[] typeName = configArray[1];
        //int rowCount = configArray.Length;
        //for (int i = 2; i < rowCount; i++) {
        //    T t = new T();
        //    Dictionary<string, FieldInfoItem> items = GetFieldInfoItems(t.GetType());
        //    string[] column = configArray[i];
        //    int columnCount = column.Length;
        //    string[] values = configArray[i];
        //    for (int j = 0; j < columnCount; j++) {
        //        if (items.ContainsKey(titleNames[j])) {
        //            items[titleNames[j]].SetValue(t, values[j]);
        //        }
        //    }
        //    if (items.ContainsKey("id")) {
        //        int key = items["id"].GetValue(t);
        //        dic.Add(key, t);
        //    }
        //}
        //return dic;
    }
    private Dictionary<int, T> AnalyticalXML<T>(string text) where T : Config, new() {
        T t = new T();
        Dictionary<int, T> dic = new Dictionary<int, T>();
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(text);
        XmlNode equipXN = xmlDoc.SelectSingleNode(t.tableName);
        XmlAttributeCollection collection = equipXN.Attributes;
        string[] titleNames = new string[collection.Count];
        string[] typeName = new string[collection.Count];
        for (int i = 0; i < collection.Count; i++) {
            titleNames[i] = collection.Item(i).Name;
            typeName[i] = collection.Item(i).Value;
        }
        XmlNodeList list = equipXN.ChildNodes;
        if (list != null && list.Count > 0) {
            foreach (XmlNode node in list) {
                XmlElement element = node as XmlElement;
                for (int i = 0; i < titleNames.Length; i++) {
                    string value = element.GetAttribute(titleNames[i]);
                    GetXMLType(StringToValueType(typeName[i]), value);
                }
            }
        }
        return dic;
    }
    private void GetXMLType(eConfigDataType type,string value) {
        switch (type) {
            case eConfigDataType.INT:
                break;
            case eConfigDataType.FLOAT:
                break;
            case eConfigDataType.STR:
                break;
            case eConfigDataType.LIST_INT:
                break;
            case eConfigDataType. LIST_STR:
                break;
            case eConfigDataType. LIST_FLOAT:
                break;
            case eConfigDataType.LIST_BOOL: break;
            case eConfigDataType.DIC_INT_INT: break;
            case eConfigDataType.DIC_INT_STR : break;
            case eConfigDataType.DIC_INT_FLOAT: break;
            case eConfigDataType.DIC_INT_BOOL: break;
            case eConfigDataType.DIC_STR_STR: break;
            case eConfigDataType.DIC_STR_INT: break;
            case eConfigDataType.DIC_STR_FLOAT: break;
            case eConfigDataType.DIC_STR_BOOL: break;
        }
    }
    private Dictionary<int, T> AnalyticalCSV<T>(string text) where T : Config, new()  {
        Dictionary<int, T> dic = new Dictionary<int, T>();


        return dic;
    }
    private Dictionary<int, T> AnalyticalJSON<T>(string text) where T : Config, new()  {
        Dictionary<int, T> dic = new Dictionary<int, T>();


        return dic;
    }
    private string[][] AssetToArray(string assetText) {
        string[] rows = assetText.Split(new char[] { '\n' });
        int rowCount = rows.Length;
        if (rowCount < 2) {
            return null;
        }
        string[][] array =  new string[rowCount][];
        for (int i = 0; i < rowCount; i++) {
            string[] column = rows[i].Split(new char[]{'|'});
            int columnCount = column.Length;
            array[i] = new string[columnCount];
            for (int j = 0; j < columnCount; j++) {
                array[i][j] = column[j];
            }
        }
        return array;
    }
    
    public Dictionary<string, FieldInfoItem> GetFieldInfoItems(Type classType) {
        Dictionary<string, FieldInfoItem> items = new Dictionary<string, FieldInfoItem>();
        FieldInfo[] infos = classType.GetFields();
        foreach (FieldInfo info in infos) {
            FieldInfoItem item = new FieldInfoItem(info.Name, info);
            items.Add(item.Name, item);
        }
        return items;
    }
    public TextAsset Load<T>() where T : Config, new() {
        T t = new T();
        string path = CONFIG_PATH + t.tableName;
        TextAsset asset = Resources.Load<TextAsset>(path);
        return asset;
    }
    //public void InitGameConfig(){
    //    string assets = ResourcesManager.Instance.LoadConfig("GameConfig");
    //    Debug.Log("assets" + assets);
    //    XmlDocument xml = new XmlDocument();
    //    xml.LoadXml(assets);
    //    XmlNode versionXM = xml.SelectSingleNode("GameConfig");
    //    XmlElement versionElement = versionXM.SelectSingleNode("version") as XmlElement;
    //    VersionManager.Instance.MajorVersion = int.Parse(versionElement.GetAttribute("major"));
    //    VersionManager.Instance.MinorVersion = int.Parse(versionElement.GetAttribute("minor"));
    //    VersionManager.Instance.Revision = int.Parse(versionElement.GetAttribute("revision"));
    //    VersionManager.Instance.MonthVersion = int.Parse(versionElement.GetAttribute("month"));
    //    VersionManager.Instance.DailyVersion = int.Parse(versionElement.GetAttribute("daily"));


    //}
    
}
