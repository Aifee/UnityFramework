//----------------------------------------------
//            MobArts PiDan Project
// Copyright © 2010-2015 MobArts Entertainment
// Created by : Liu Aifei (329737941@qq.com)
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class FieldInfoItem {
    public string Name;
    public eConfigDataType Type = eConfigDataType.Unknown;
    public FieldInfo Info;
    public FieldInfoItem() {

    }
    public FieldInfoItem(string name, FieldInfo info) {
        Name = name;
        Info = info;
        Type = GetPropertiesType();
    }
    public eConfigDataType GetPropertiesType() {
        if (typeof(int).IsAssignableFrom(Info.FieldType)) {
            return eConfigDataType.INT;
        } else if (typeof(string).IsAssignableFrom(Info.FieldType)) {
            return eConfigDataType.STR;
        } else if (typeof(float).IsAssignableFrom(Info.FieldType)) {
            return eConfigDataType.FLOAT;
        }else if(typeof(bool).IsAssignableFrom(Info.FieldType)){
            return eConfigDataType.BOOL;
        }else if (typeof(List<int>).IsAssignableFrom(Info.FieldType)) {
            return eConfigDataType.LIST_INT;
        } else if (typeof(List<string>).IsAssignableFrom(Info.FieldType)) {
            return eConfigDataType.LIST_STR;
        } else if (typeof(List<float>).IsAssignableFrom(Info.FieldType)) {
            return eConfigDataType.LIST_FLOAT;
        } else if (typeof(List<bool>).IsAssignableFrom(Info.FieldType)){
            return eConfigDataType.LIST_BOOL;
        }else if (typeof(Dictionary<int, int>).IsAssignableFrom(Info.FieldType)) {
            return eConfigDataType.DIC_INT_INT;
        } else if (typeof(Dictionary<int, string>).IsAssignableFrom(Info.FieldType)) {
            return eConfigDataType.DIC_INT_STR;
        } else if (typeof(Dictionary<int, float>).IsAssignableFrom(Info.FieldType)) {
            return eConfigDataType.DIC_INT_FLOAT;
        } else if (typeof(Dictionary<int, bool>).IsAssignableFrom(Info.FieldType)) {
            return eConfigDataType.DIC_INT_BOOL;
        } else if (typeof(Dictionary<string, string>).IsAssignableFrom(Info.FieldType)) {
            return eConfigDataType.DIC_STR_STR;
        } else if (typeof(Dictionary<string, int>).IsAssignableFrom(Info.FieldType)) {
            return eConfigDataType.DIC_STR_INT;
        } else if (typeof(Dictionary<string, float>).IsAssignableFrom(Info.FieldType)) {
            return eConfigDataType.DIC_STR_FLOAT;
        } else if (typeof(Dictionary<string, bool>).IsAssignableFrom(Info.FieldType)) {
            return eConfigDataType.DIC_STR_BOOL;
        } else {
            return eConfigDataType.Unknown;
        }
    }
    public int GetValue(object obj) {
        object arg = Info.GetValue(obj);
        int id = int.Parse(arg.ToString());
        return id;
    }
    public void SetValue(object t, object obj) {
        object value = null;
        string str;
        string[] listArray;
        Dictionary<string, object> dic;
        switch (Type) {
            case eConfigDataType.Unknown:

                break;
            case eConfigDataType.INT:
                value = int.Parse(obj.ToString());
                break;
            case eConfigDataType.STR:
            case eConfigDataType.JSON:
                value = obj.ToString();
                break;
            case eConfigDataType.FLOAT:
                value = float.Parse(obj.ToString());
                break;
            case eConfigDataType.BOOL:
                value = obj.ToString() != "0";
                break;
            case eConfigDataType.LIST_INT:
                str = obj.ToString();
                str = str.Replace("[", "");
                str = str.Replace("]", "");
                if (str.Equals(""))
                    return;
                listArray = str.Split(new char[] { ',' });
                List<int> intList = new List<int>();
                foreach (string s in listArray) {
                    intList.Add(int.Parse(s));
                }
                value = intList;
                break;
            case eConfigDataType.LIST_STR:
                str = obj.ToString();
                str = str.Replace("[", "");
                str = str.Replace("]", "");
                if (str.Equals(""))
                    return;
                listArray = str.Split(new char[] { ',' });
                List<string> stringList = new List<string>();
                foreach (string s in listArray) {
                    stringList.Add(s);
                }
                value = stringList;
                break;
            case eConfigDataType.LIST_FLOAT:
                str = obj.ToString();
                str = str.Replace("[", "");
                str = str.Replace("]", "");
                if (str.Equals(""))
                    return;
                listArray = str.Split(new char[] { ',' });
                List<float> floatList = new List<float>();
                foreach (string s in listArray) {
                    floatList.Add(float.Parse(s));
                }
                value = floatList;
                break;
            case eConfigDataType.LIST_BOOL:
                str = obj.ToString();
                str = str.Replace("[", "");
                str = str.Replace("]", "");
                if (str.Equals(""))
                    return;
                listArray = str.Split(new char[] { ',' });
                List<bool> boolList = new List<bool>();
                foreach (string s in listArray) {
                    boolList.Add(s != "0");
                }
                value = boolList;
                break;
            case eConfigDataType.DIC_INT_INT:
                str = obj.ToString();
                dic = MiniJSON.Json.Deserialize(str) as Dictionary<string, object>;
                Dictionary<int, int> diiDic = new Dictionary<int, int>();
                foreach (KeyValuePair<string, object> kvp in dic) {
                    diiDic.Add(int.Parse(kvp.Key), int.Parse(kvp.Value.ToString()));
                }
                value = diiDic;
                break;
            case eConfigDataType.DIC_INT_STR:
                str = obj.ToString();
                dic = MiniJSON.Json.Deserialize(str) as Dictionary<string, object>;
                Dictionary<int, string> disDic = new Dictionary<int, string>();
                foreach (KeyValuePair<string, object> kvp in dic) {
                    disDic.Add(int.Parse(kvp.Key), kvp.Value.ToString());
                }
                value = disDic;
                break;
            case eConfigDataType.DIC_INT_FLOAT:
                str = obj.ToString();
                dic = MiniJSON.Json.Deserialize(str) as Dictionary<string, object>;
                Dictionary<int, float> difDic = new Dictionary<int, float>();
                foreach (KeyValuePair<string, object> kvp in dic) {
                    difDic.Add(int.Parse(kvp.Key), float.Parse(kvp.Value.ToString()));
                }
                value = difDic;
                break;
            case eConfigDataType.DIC_INT_BOOL:
                str = obj.ToString();
                dic = MiniJSON.Json.Deserialize(str) as Dictionary<string, object>;
                Dictionary<int, bool> dibDic = new Dictionary<int, bool>();
                foreach (KeyValuePair<string, object> kvp in dic) {
                    dibDic.Add(int.Parse(kvp.Key), kvp.Value.ToString() != "0");
                }
                value = dibDic;
                break;
            case eConfigDataType.DIC_STR_STR:
                str = obj.ToString();
                dic = MiniJSON.Json.Deserialize(str) as Dictionary<string, object>;
                Dictionary<string, string> dssDic = new Dictionary<string, string>();
                foreach (KeyValuePair<string, object> kvp in dic) {
                    dssDic.Add(kvp.Key, kvp.Value.ToString());
                }
                value = dssDic;
                break;
            case eConfigDataType.DIC_STR_INT:
                str = obj.ToString();
                dic = MiniJSON.Json.Deserialize(str) as Dictionary<string, object>;
                Dictionary<string, int> dsiDic = new Dictionary<string, int>();
                foreach (KeyValuePair<string, object> kvp in dic) {
                    dsiDic.Add(kvp.Key, int.Parse(kvp.Value.ToString()));
                }
                value = dsiDic;
                break;
            case eConfigDataType.DIC_STR_FLOAT:
                str = obj.ToString();
                dic = MiniJSON.Json.Deserialize(str) as Dictionary<string, object>;
                Dictionary<string, float> dsfDic = new Dictionary<string, float>();
                foreach (KeyValuePair<string, object> kvp in dic) {
                    dsfDic.Add(kvp.Key, float.Parse(kvp.Value.ToString()));
                }
                value = dsfDic;
                break;
            case eConfigDataType.DIC_STR_BOOL:
                str = obj.ToString();
                dic = MiniJSON.Json.Deserialize(str) as Dictionary<string, object>;
                Dictionary<string, bool> dsbDic = new Dictionary<string, bool>();
                foreach (KeyValuePair<string, object> kvp in dic) {
                    dsbDic.Add(kvp.Key, kvp.Value.ToString() != "0");
                }
                value = dsbDic;
                break;
        }
        //Debug.Log("value = " + value.ToString());
        Info.SetValue(t, value);
    }

}
