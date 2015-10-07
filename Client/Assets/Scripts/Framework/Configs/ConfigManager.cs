//----------------------------------------------
//            MobArts PiDan Project
// Copyright © 2010-2015 MobArts Entertainment
// Created by : Liu Aifei (329737941@qq.com)
//----------------------------------------------

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

public class ConfigManager : Singleton<ConfigManager> {
    private static string CONFIG_PATH = "Configs/";
    static public PropertiesType StringToValueType(string str) {
        PropertiesType type = PropertiesType.Unknown;
        switch (str) {
            case "int": type = PropertiesType.INT; break;
            case "float": type = PropertiesType.FLOAT; break;
            case "string": type = PropertiesType.STR; break;
            case "list[int]": type = PropertiesType.LIST_INT; break;
            case "list[string]": type = PropertiesType.LIST_STR; break;
            case "list[float]": type = PropertiesType.LIST_FLOAT; break;
            case "list[dic[int,int]]": type = PropertiesType.LIST_DICT_INT_INT; break;
            case "list[dic[int,string]]": type = PropertiesType.LIST_DICT_INT_STR; break;
            case "list[dic[int,float]]": type = PropertiesType.LIST_DICT_INT_FLOAT; break;
            case "list[dic[string,string]]": type = PropertiesType.LIST_DICT_STR_STR; break;
            case "list[dic[string,int]]": type = PropertiesType.LIST_DICT_STR_INT; break;
            case "list[dic[string,float]]": type = PropertiesType.LIST_DICT_STR_FLOAT; break;
            case "list[dic[float,float]]": type = PropertiesType.LIST_DICT_FLOAT_FLOAT; break;
            case "list[dic[float,int]]": type = PropertiesType.LIST_DICT_FLOAT_INT; break;
            case "list[dic[float,string]]": type = PropertiesType.LIST_DICT_FLOAT_STR; break;
            case "dic[int,int]": type = PropertiesType.DICT_INT_INT; break;
            case "dic[int,string]": type = PropertiesType.DICT_INT_STR; break;
            case "dic[int,float]": type = PropertiesType.DICT_INT_FLOAT; break;
            case "dic[int,list[int]]": type = PropertiesType.DICT_INT_LIST_INT; break;
            case "dic[int,list[string]]": type = PropertiesType.DICT_INT_LIST_STR; break;
            case "dic[int,list[float]]": type = PropertiesType.DICT_INT_LIST_FLOAT; break;
            case "dic[string,string]": type = PropertiesType.DICT_STR_STR; break;
            case "dic[string,int]": type = PropertiesType.DICT_STR_INT; break;
            case "dic[string,float]": type = PropertiesType.DICT_STR_FLOAT; break;
            case "dic[string,list[int]]": type = PropertiesType.DICT_STR_LIST_INT; break;
            case "dic[string,list[string]]": type = PropertiesType.DICT_STR_LIST_STR; break;
            case "dic[string,list[float]]": type = PropertiesType.DICT_STR_LIST_FLOAT; break;
            case "dic[float,float]": type = PropertiesType.DICT_FLOAT_FLOAT; break;
            case "dic[float,int]": type = PropertiesType.DICT_FLOAT_INT; break;
            case "dic[float,string]": type = PropertiesType.DICT_FLOAT_STR; break;
            case "dic[float,list[int]]": type = PropertiesType.DICT_FLOAT_LIST_INT; break;
            case "dic[float,list[string]]": type = PropertiesType.DICT_FLOAT_LIST_STR; break;
            case "dic[float,list[float]]": type = PropertiesType.DICT_FLOAT_LIST_FLOAT; break;
        }
        return type;
    }

    public enum PropertiesType {
        Unknown,
        /// <summary> int </summary>
        INT,
        /// <summary> float </summary>
        FLOAT,
        /// <summary> string </summary>
        STR,
        /// <summary> List [ int ] </summary>
        LIST_INT,
        /// <summary> List [ string ] </summary>
        LIST_STR,
        /// <summary> List [ float ] </summary>
        LIST_FLOAT,
        /// <summary> List [ Dictionary [ int,int ] ] </summary>
        LIST_DICT_INT_INT,
        /// <summary> List [ Dictionary [ int,string ] ] </summary>
        LIST_DICT_INT_STR,
        /// <summary> List [ Dictionary [ int,float ] ] </summary>
        LIST_DICT_INT_FLOAT,
        /// <summary> List [ Dictionary [ string,string ] ] </summary>
        LIST_DICT_STR_STR,
        /// <summary> List [ Dictionary [ string,int ] ] </summary>
        LIST_DICT_STR_INT,
        /// <summary> List [ Dictionary [ string,float ] ] </summary>
        LIST_DICT_STR_FLOAT,
        /// <summary> List [ Dictionary [ float,float ] ] </summary>
        LIST_DICT_FLOAT_FLOAT,
        /// <summary> List [ Dictionary [ float,int ] ] </summary>
        LIST_DICT_FLOAT_INT,
        /// <summary> List [ Dictionary [ float,string ] ] </summary>
        LIST_DICT_FLOAT_STR,
        /// <summary> Dictionary [ int,int ] </summary>
        DICT_INT_INT,
        /// <summary> Dictionary [ int,string ] </summary>
        DICT_INT_STR,
        /// <summary> Dictionary [ int,float ] </summary>
        DICT_INT_FLOAT,
        /// <summary> Dictionary [ int,List [ int ] ] </summary>
        DICT_INT_LIST_INT,
        /// <summary> Dictionary [ int,List [ string ] ] </summary>
        DICT_INT_LIST_STR,
        /// <summary> Dictionary [ int,List [ float ] ] </summary>
        DICT_INT_LIST_FLOAT,
        /// <summary> Dictionary [ string,string ] </summary>
        DICT_STR_STR,
        /// <summary> Dictionary [ string,int ] </summary>
        DICT_STR_INT,
        /// <summary> Dictionary [ string,float ] </summary>
        DICT_STR_FLOAT,
        /// <summary> Dictionary [ string,List [ int ] ] </summary>
        DICT_STR_LIST_INT,
        /// <summary> Dictionary [ string,List [ string ] ] </summary>
        DICT_STR_LIST_STR,
        /// <summary> Dictionary [ string,List [ float ] ] </summary>
        DICT_STR_LIST_FLOAT,
        /// <summary> Dictionary [ float,float ] </summary>
        DICT_FLOAT_FLOAT,
        /// <summary> Dictionary [ float,int ] </summary>
        DICT_FLOAT_INT,
        /// <summary> Dictionary [ float,string ] </summary>
        DICT_FLOAT_STR,
        /// <summary> Dictionary [ float,List [ int ] ] </summary>
        DICT_FLOAT_LIST_INT,
        /// <summary> Dictionary [ float,List [ string ] ] </summary>
        DICT_FLOAT_LIST_STR,
        /// <summary> Dictionary [ float,List [ float ] ] </summary>
        DICT_FLOAT_LIST_FLOAT,
    }
    public enum ConfigType {
        Xml,
        Json,
        Csv,
    }

    private Dictionary<System.Type, Dictionary<int, Config>> configs = new Dictionary<System.Type, Dictionary<int, Config>>();
    public T GetConfig<T>(ConfigType type,int id) where T : Config, new() {
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

    private Dictionary<int, T> Analytical<T>(ConfigType type) where T : Config, new() {
        TextAsset asset = Load<T>();
        Debug.Log("Asset == " + asset.text);
        if (type == ConfigType.Xml) {
            return AnalyticalXML<T>(asset.text);
        } else if (type == ConfigType.Csv) {
            return AnalyticalCSV<T>(asset.text);
        } else if (type == ConfigType.Json) {
            return AnalyticalJSON<T>(asset.text);
        }
        return null;
        string[][] configArray = AssetToArray(asset.text);
        if (configArray == null) {
            Debug.LogError("Data format error, data line number must be at least 2 lines");
            return null;
        }
        Dictionary<int, T> dic = new Dictionary<int, T>();

        string[] titleNames = configArray[0];
        string[] typeName = configArray[1];
        int rowCount = configArray.Length;
        for (int i = 2; i < rowCount; i++) {
            T t = new T();
            Dictionary<string, FieldInfoItem> items = GetFieldInfoItems(t.GetType());
            string[] column = configArray[i];
            int columnCount = column.Length;
            string[] values = configArray[i];
            for (int j = 0; j < columnCount; j++) {
                if (items.ContainsKey(titleNames[j])) {
                    items[titleNames[j]].SetValue(t, values[j]);
                }
            }
            if (items.ContainsKey("id")) {
                int key = items["id"].GetValue(t);
                dic.Add(key, t);
            }
        }
        return dic;
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
    private void GetXMLType(PropertiesType type,string value) {
        switch (type) {
            case PropertiesType.INT:
                break;
            case PropertiesType.FLOAT:
                break;
            case PropertiesType.STR:
                break;
            case PropertiesType.LIST_INT:
                break;
            case PropertiesType. LIST_STR:
                break;
            case PropertiesType. LIST_FLOAT:
                break;
            case PropertiesType.LIST_DICT_INT_INT:
                break;
            case PropertiesType.LIST_DICT_INT_STR:
                break;
            case PropertiesType.LIST_DICT_INT_FLOAT:
                break;
            case PropertiesType.LIST_DICT_STR_STR:
                break;
            case PropertiesType.LIST_DICT_STR_INT:
                break;
            case PropertiesType.LIST_DICT_STR_FLOAT:
                break;
            case PropertiesType.LIST_DICT_FLOAT_FLOAT:break;
            case PropertiesType.LIST_DICT_FLOAT_INT:break;
            case PropertiesType.LIST_DICT_FLOAT_STR:break;
            case PropertiesType.DICT_INT_INT:break;
            case PropertiesType.DICT_INT_STR:break;
            case PropertiesType.DICT_INT_FLOAT:break;
            case PropertiesType.DICT_INT_LIST_INT:break;
            case PropertiesType.DICT_INT_LIST_STR:break;
            case PropertiesType.DICT_INT_LIST_FLOAT:break;
            case PropertiesType.DICT_STR_STR:break;
            case PropertiesType.DICT_STR_INT:break;
            case PropertiesType.DICT_STR_FLOAT:break;
            case PropertiesType.DICT_STR_LIST_INT:break;
            case PropertiesType.DICT_STR_LIST_STR:break;
            case PropertiesType.DICT_STR_LIST_FLOAT:break;
            case PropertiesType.DICT_FLOAT_FLOAT:break;
            case PropertiesType. DICT_FLOAT_INT:break;
            case PropertiesType.DICT_FLOAT_STR:break;
            case PropertiesType.DICT_FLOAT_LIST_INT:break;
            case PropertiesType.DICT_FLOAT_LIST_STR:break;
            case PropertiesType.DICT_FLOAT_LIST_FLOAT:break;
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
    
}
