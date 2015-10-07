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
    public ConfigManager.PropertiesType Type = ConfigManager.PropertiesType.Unknown;
    public FieldInfo Info;
    public FieldInfoItem() {

    }
    public FieldInfoItem(string name, FieldInfo info) {
        Name = name;
        Info = info;
        Type = GetPropertiesType();
    }
    public ConfigManager.PropertiesType GetPropertiesType() {
        if (typeof(int).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.INT;
        } else if (typeof(string).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.STR;
        } else if (typeof(float).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.FLOAT;
        } else if (typeof(List<int>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.LIST_INT;
        } else if (typeof(List<string>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.LIST_STR;
        } else if (typeof(List<float>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.LIST_FLOAT;
        } else if (typeof(List<Dictionary<int, int>>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.LIST_DICT_INT_INT;
        } else if (typeof(List<Dictionary<int, string>>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.LIST_DICT_INT_STR;
        } else if (typeof(List<Dictionary<int, float>>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.LIST_DICT_INT_FLOAT;
        } else if (typeof(List<Dictionary<string, string>>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.LIST_DICT_STR_STR;
        } else if (typeof(List<Dictionary<string, int>>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.LIST_DICT_STR_INT;
        } else if (typeof(List<Dictionary<string, float>>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.LIST_DICT_STR_FLOAT;
        } else if (typeof(List<Dictionary<float, float>>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.LIST_DICT_FLOAT_FLOAT;
        } else if (typeof(List<Dictionary<float, int>>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.LIST_DICT_FLOAT_INT;
        } else if (typeof(List<Dictionary<float, string>>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.LIST_DICT_FLOAT_STR;
        } else if (typeof(Dictionary<int, int>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.DICT_INT_INT;
        } else if (typeof(Dictionary<int, string>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.DICT_INT_STR;
        } else if (typeof(Dictionary<int, float>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.DICT_INT_FLOAT;
        } else if (typeof(Dictionary<int, List<int>>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.DICT_INT_LIST_INT;
        } else if (typeof(Dictionary<int, List<string>>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.DICT_INT_LIST_STR;
        } else if (typeof(Dictionary<int, List<float>>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.DICT_INT_LIST_FLOAT;
        } else if (typeof(Dictionary<string, string>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.DICT_STR_STR;
        } else if (typeof(Dictionary<string, int>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.DICT_STR_INT;
        } else if (typeof(Dictionary<string, float>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.DICT_STR_FLOAT;
        } else if (typeof(Dictionary<string, List<int>>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.DICT_STR_LIST_INT;
        } else if (typeof(Dictionary<string, List<string>>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.DICT_STR_LIST_STR;
        } else if (typeof(Dictionary<string, List<float>>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.DICT_STR_LIST_FLOAT;
        } else if (typeof(Dictionary<float, float>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.DICT_FLOAT_FLOAT;
        } else if (typeof(Dictionary<float, int>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.DICT_FLOAT_INT;
        } else if (typeof(Dictionary<float, string>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.DICT_FLOAT_STR;
        } else if (typeof(Dictionary<float, List<int>>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.DICT_FLOAT_LIST_INT;
        } else if (typeof(Dictionary<float, List<string>>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.DICT_FLOAT_LIST_STR;
        } else if (typeof(Dictionary<float, List<float>>).IsAssignableFrom(Info.FieldType)) {
            return ConfigManager.PropertiesType.DICT_FLOAT_LIST_FLOAT;
        } else {
            return ConfigManager.PropertiesType.Unknown;
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
        string[] dicArray;
        string split;
        switch (Type) {
            case ConfigManager.PropertiesType.Unknown:

                break;
            case ConfigManager.PropertiesType.INT:
                value = int.Parse(obj.ToString());
                break;
            case ConfigManager.PropertiesType.STR:
                value = obj.ToString();
                break;
            case ConfigManager.PropertiesType.FLOAT:
                value = float.Parse(obj.ToString());
                break;
            case ConfigManager.PropertiesType.LIST_INT:
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
            case ConfigManager.PropertiesType.LIST_STR:
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
            case ConfigManager.PropertiesType.LIST_FLOAT:
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
            case ConfigManager.PropertiesType.LIST_DICT_INT_INT:
                str = obj.ToString();
                str = str.Replace("[", "");
                str = str.Replace("]", "");
                if (str.Equals(""))
                    return;
                listArray = str.Split(new char[] { ',' });
                List<Dictionary<int, int>> liiList = new List<Dictionary<int, int>>();
                foreach (string s in listArray) {
                    split = s.Replace("{", "");
                    split = split.Replace("}", "");
                    dicArray = s.Split(new char[] { ',' });
                    if (dicArray.Length == 2) {
                        Dictionary<int, int> iiDic = new Dictionary<int, int>();
                        iiDic.Add(int.Parse(dicArray[0].ToString()), int.Parse(dicArray[1].ToString()));
                        liiList.Add(iiDic);
                    }
                }
                value = liiList;
                break;
            case ConfigManager.PropertiesType.LIST_DICT_INT_STR:
                str = obj.ToString();
                str = str.Replace("[", "");
                str = str.Replace("]", "");
                if (str.Equals(""))
                    return;
                listArray = str.Split(new char[] { ',' });
                List<Dictionary<int, string>> lisList = new List<Dictionary<int, string>>();
                foreach (string s in listArray) {
                    split = s.Replace("{", "");
                    split = split.Replace("}", "");
                    dicArray = s.Split(new char[] { ',' });
                    if (dicArray.Length == 2) {
                        Dictionary<int, string> isDic = new Dictionary<int, string>();
                        isDic.Add(int.Parse(dicArray[0].ToString()), dicArray[1].ToString());
                        lisList.Add(isDic);
                    }
                }
                value = lisList;
                break;
            case ConfigManager.PropertiesType.LIST_DICT_INT_FLOAT:
                str = obj.ToString();
                str = str.Replace("[", "");
                str = str.Replace("]", "");
                if (str.Equals(""))
                    return;
                listArray = str.Split(new char[] { ',' });
                List<Dictionary<int, float>> lifList = new List<Dictionary<int, float>>();
                foreach (string s in listArray) {
                    split = s.Replace("{", "");
                    split = split.Replace("}", "");
                    dicArray = s.Split(new char[] { ',' });
                    if (dicArray.Length == 2) {
                        Dictionary<int, float> ifDic = new Dictionary<int, float>();
                        ifDic.Add(int.Parse(dicArray[0].ToString()), float.Parse(dicArray[1].ToString()));
                        lifList.Add(ifDic);
                    }
                }
                value = lifList;
                break;
            case ConfigManager.PropertiesType.LIST_DICT_STR_STR:
                str = obj.ToString();
                str = str.Replace("[", "");
                str = str.Replace("]", "");
                if (str.Equals(""))
                    return;
                listArray = str.Split(new char[] { ',' });
                List<Dictionary<string, string>> lssList = new List<Dictionary<string, string>>();
                foreach (string s in listArray) {
                    split = s.Replace("{", "");
                    split = split.Replace("}", "");
                    dicArray = s.Split(new char[] { ',' });
                    if (dicArray.Length == 2) {
                        Dictionary<string, string> ssDic = new Dictionary<string, string>();
                        ssDic.Add(dicArray[0].ToString(), dicArray[1].ToString());
                        lssList.Add(ssDic);
                    }
                }
                value = lssList;
                break;
            case ConfigManager.PropertiesType.LIST_DICT_STR_INT:
                str = obj.ToString();
                str = str.Replace("[", "");
                str = str.Replace("]", "");
                if (str.Equals(""))
                    return;
                listArray = str.Split(new char[] { ',' });
                List<Dictionary<string, int>> lsiList = new List<Dictionary<string, int>>();
                foreach (string s in listArray) {
                    split = s.Replace("{", "");
                    split = split.Replace("}", "");
                    dicArray = s.Split(new char[] { ',' });
                    if (dicArray.Length == 2) {
                        Dictionary<string, int> siDic = new Dictionary<string, int>();
                        siDic.Add(dicArray[0].ToString(), int.Parse(dicArray[1].ToString()));
                        lsiList.Add(siDic);
                    }
                }
                value = lsiList;
                break;
            case ConfigManager.PropertiesType.LIST_DICT_STR_FLOAT:
                str = obj.ToString();
                str = str.Replace("[", "");
                str = str.Replace("]", "");
                if (str.Equals(""))
                    return;
                listArray = str.Split(new char[] { ',' });
                List<Dictionary<string, float>> lsfList = new List<Dictionary<string, float>>();
                foreach (string s in listArray) {
                    split = s.Replace("{", "");
                    split = split.Replace("}", "");
                    dicArray = s.Split(new char[] { ',' });
                    if (dicArray.Length == 2) {
                        Dictionary<string, float> sfDic = new Dictionary<string, float>();
                        sfDic.Add(dicArray[0].ToString(), float.Parse(dicArray[1].ToString()));
                        lsfList.Add(sfDic);
                    }
                }
                value = lsfList;
                break;
            case ConfigManager.PropertiesType.LIST_DICT_FLOAT_FLOAT:
                str = obj.ToString();
                str = str.Replace("[", "");
                str = str.Replace("]", "");
                if (str.Equals(""))
                    return;
                listArray = str.Split(new char[] { ',' });
                List<Dictionary<float, float>> lffList = new List<Dictionary<float, float>>();
                foreach (string s in listArray) {
                    split = s.Replace("{", "");
                    split = split.Replace("}", "");
                    dicArray = s.Split(new char[] { ',' });
                    if (dicArray.Length == 2) {
                        Dictionary<float, float> ffDic = new Dictionary<float, float>();
                        ffDic.Add(float.Parse(dicArray[0].ToString()), float.Parse(dicArray[1].ToString()));
                        lffList.Add(ffDic);
                    }
                }
                value = lffList;
                break;
            case ConfigManager.PropertiesType.LIST_DICT_FLOAT_INT:
                str = obj.ToString();
                str = str.Replace("[", "");
                str = str.Replace("]", "");
                if (str.Equals(""))
                    return;
                listArray = str.Split(new char[] { ',' });
                List<Dictionary<float, int>> lfiList = new List<Dictionary<float, int>>();
                foreach (string s in listArray) {
                    split = s.Replace("{", "");
                    split = split.Replace("}", "");
                    dicArray = s.Split(new char[] { ',' });
                    if (dicArray.Length == 2) {
                        Dictionary<float, int> fiDic = new Dictionary<float, int>();
                        fiDic.Add(float.Parse(dicArray[0].ToString()), int.Parse(dicArray[1].ToString()));
                        lfiList.Add(fiDic);
                    }
                }
                value = lfiList;
                break;
            case ConfigManager.PropertiesType.LIST_DICT_FLOAT_STR:
                str = obj.ToString();
                str = str.Replace("[", "");
                str = str.Replace("]", "");
                if (str.Equals(""))
                    return;
                listArray = str.Split(new char[] { ',' });
                List<Dictionary<float, string>> lfsList = new List<Dictionary<float, string>>();
                foreach (string s in listArray) {
                    split = s.Replace("{", "");
                    split = split.Replace("}", "");
                    dicArray = s.Split(new char[] { ',' });
                    if (dicArray.Length == 2) {
                        Dictionary<float, string> fsDic = new Dictionary<float, string>();
                        fsDic.Add(float.Parse(dicArray[0].ToString()), dicArray[1].ToString());
                        lfsList.Add(fsDic);
                    }
                }
                value = lfsList;
                break;
            case ConfigManager.PropertiesType.DICT_INT_INT:
                str = obj.ToString();
                str = str.Replace("{", "");
                str = str.Replace("}", "");
                if (str.Equals(""))
                    return;
                dicArray = str.Split(new char[] { ',' });
                Dictionary<int, int> diiDic = new Dictionary<int, int>();
                foreach (string s in dicArray) {
                    dicArray = s.Split(new char[] { ':' });
                    if (dicArray.Length == 2) {
                        diiDic.Add(int.Parse(dicArray[0].ToString()), int.Parse(dicArray[1].ToString()));
                    }
                }
                value = diiDic;
                break;
            case ConfigManager.PropertiesType.DICT_INT_STR:
                str = obj.ToString();
                str = str.Replace("{", "");
                str = str.Replace("}", "");
                if (str.Equals(""))
                    return;
                dicArray = str.Split(new char[] { ',' });
                Dictionary<int, string> disDic = new Dictionary<int, string>();
                foreach (string s in dicArray) {
                    dicArray = s.Split(new char[] { ':' });
                    if (dicArray.Length == 2) {
                        disDic.Add(int.Parse(dicArray[0].ToString()), dicArray[1].ToString());
                    }
                }
                value = disDic;
                break;
            case ConfigManager.PropertiesType.DICT_INT_FLOAT:
                str = obj.ToString();
                str = str.Replace("{", "");
                str = str.Replace("}", "");
                if (str.Equals(""))
                    return;
                dicArray = str.Split(new char[] { ',' });
                Dictionary<int, float> difDic = new Dictionary<int, float>();
                foreach (string s in dicArray) {
                    dicArray = s.Split(new char[] { ':' });
                    if (dicArray.Length == 2) {
                        difDic.Add(int.Parse(dicArray[0].ToString()), float.Parse(dicArray[1].ToString()));
                    }
                }
                value = difDic;
                break;
            case ConfigManager.PropertiesType.DICT_INT_LIST_INT:
                str = obj.ToString();
                str = str.Replace("{", "");
                str = str.Replace("}", "");
                if (str.Equals(""))
                    return;
                dicArray = str.Split(new char[] { ',' });
                Dictionary<int, List<int>> diliDic = new Dictionary<int, List<int>>();
                foreach (string s in dicArray) {
                    dicArray = s.Split(new char[] { ':' });
                    if (dicArray.Length == 2) {
                        split = dicArray[1];
                        split.Replace("[", "");
                        split.Replace("]", "");
                        listArray = split.Split(new char[] { ',' });
                        List<int> dliList = new List<int>();
                        foreach (string ls in listArray) {
                            dliList.Add(int.Parse(ls));
                        }
                        diliDic.Add(int.Parse(dicArray[0].ToString()), dliList);
                    }
                }
                value = diliDic;
                break;
            case ConfigManager.PropertiesType.DICT_INT_LIST_STR:
                str = obj.ToString();
                str = str.Replace("{", "");
                str = str.Replace("}", "");
                if (str.Equals(""))
                    return;
                dicArray = str.Split(new char[] { ',' });
                Dictionary<int, List<string>> dilsDic = new Dictionary<int, List<string>>();
                foreach (string s in dicArray) {
                    dicArray = s.Split(new char[] { ':' });
                    if (dicArray.Length == 2) {
                        split = dicArray[1];
                        split.Replace("[", "");
                        split.Replace("]", "");
                        listArray = split.Split(new char[] { ',' });
                        List<string> dliList = new List<string>();
                        foreach (string ls in listArray) {
                            dliList.Add(ls);
                        }
                        dilsDic.Add(int.Parse(dicArray[0].ToString()), dliList);
                    }
                }
                value = dilsDic;
                break;
            case ConfigManager.PropertiesType.DICT_INT_LIST_FLOAT:
                str = obj.ToString();
                str = str.Replace("{", "");
                str = str.Replace("}", "");
                if (str.Equals(""))
                    return;
                dicArray = str.Split(new char[] { ',' });
                Dictionary<int, List<float>> dilfDic = new Dictionary<int, List<float>>();
                foreach (string s in dicArray) {
                    dicArray = s.Split(new char[] { ':' });
                    if (dicArray.Length == 2) {
                        split = dicArray[1];
                        split.Replace("[", "");
                        split.Replace("]", "");
                        listArray = split.Split(new char[] { ',' });
                        List<float> dliList = new List<float>();
                        foreach (string ls in listArray) {
                            dliList.Add(float.Parse(ls));
                        }
                        dilfDic.Add(int.Parse(dicArray[0].ToString()), dliList);
                    }
                }
                value = dilfDic;
                break;
            case ConfigManager.PropertiesType.DICT_STR_STR:
                str = obj.ToString();
                str = str.Replace("{", "");
                str = str.Replace("}", "");
                if (str.Equals(""))
                    return;
                dicArray = str.Split(new char[] { ',' });
                Dictionary<string, string> dssDic = new Dictionary<string, string>();
                foreach (string s in dicArray) {
                    dicArray = s.Split(new char[] { ':' });
                    if (dicArray.Length == 2) {
                        dssDic.Add(dicArray[0].ToString(), dicArray[1].ToString());
                    }
                }
                value = dssDic;
                break;
            case ConfigManager.PropertiesType.DICT_STR_INT:
                str = obj.ToString();
                str = str.Replace("{", "");
                str = str.Replace("}", "");
                if (str.Equals(""))
                    return;
                dicArray = str.Split(new char[] { ',' });
                Dictionary<string, int> dsiDic = new Dictionary<string, int>();
                foreach (string s in dicArray) {
                    dicArray = s.Split(new char[] { ':' });
                    if (dicArray.Length == 2) {
                        dsiDic.Add(dicArray[0].ToString(), int.Parse(dicArray[1].ToString()));
                    }
                }
                value = dsiDic;
                break;
            case ConfigManager.PropertiesType.DICT_STR_FLOAT:
                str = obj.ToString();
                str = str.Replace("{", "");
                str = str.Replace("}", "");
                if (str.Equals(""))
                    return;
                dicArray = str.Split(new char[] { ',' });
                Dictionary<string, float> dsfDic = new Dictionary<string, float>();
                foreach (string s in dicArray) {
                    dicArray = s.Split(new char[] { ':' });
                    if (dicArray.Length == 2) {
                        dsfDic.Add(dicArray[0].ToString(), float.Parse(dicArray[1].ToString()));
                    }
                }
                value = dsfDic;
                break;
            case ConfigManager.PropertiesType.DICT_STR_LIST_INT:
                str = obj.ToString();
                str = str.Replace("{", "");
                str = str.Replace("}", "");
                if (str.Equals(""))
                    return;
                dicArray = str.Split(new char[] { ',' });
                Dictionary<string, List<int>> dsliDic = new Dictionary<string, List<int>>();
                foreach (string s in dicArray) {
                    dicArray = s.Split(new char[] { ':' });
                    if (dicArray.Length == 2) {
                        split = dicArray[1];
                        split.Replace("[", "");
                        split.Replace("]", "");
                        listArray = split.Split(new char[] { ',' });
                        List<int> dliList = new List<int>();
                        foreach (string ls in listArray) {
                            dliList.Add(int.Parse(ls));
                        }
                        dsliDic.Add(dicArray[0].ToString(), dliList);
                    }
                }
                value = dsliDic;
                break;
            case ConfigManager.PropertiesType.DICT_STR_LIST_STR:
                str = obj.ToString();
                str = str.Replace("{", "");
                str = str.Replace("}", "");
                if (str.Equals(""))
                    return;
                dicArray = str.Split(new char[] { ',' });
                Dictionary<string, List<string>> dslsDic = new Dictionary<string, List<string>>();
                foreach (string s in dicArray) {
                    dicArray = s.Split(new char[] { ':' });
                    if (dicArray.Length == 2) {
                        split = dicArray[1];
                        split.Replace("[", "");
                        split.Replace("]", "");
                        listArray = split.Split(new char[] { ',' });
                        List<string> dliList = new List<string>();
                        foreach (string ls in listArray) {
                            dliList.Add(ls);
                        }
                        dslsDic.Add(dicArray[0].ToString(), dliList);
                    }
                }
                value = dslsDic;
                break;
            case ConfigManager.PropertiesType.DICT_STR_LIST_FLOAT:
                str = obj.ToString();
                str = str.Replace("{", "");
                str = str.Replace("}", "");
                if (str.Equals(""))
                    return;
                dicArray = str.Split(new char[] { ',' });
                Dictionary<string, List<float>> dslfDic = new Dictionary<string, List<float>>();
                foreach (string s in dicArray) {
                    dicArray = s.Split(new char[] { ':' });
                    if (dicArray.Length == 2) {
                        split = dicArray[1];
                        split.Replace("[", "");
                        split.Replace("]", "");
                        listArray = split.Split(new char[] { ',' });
                        List<float> dliList = new List<float>();
                        foreach (string ls in listArray) {
                            dliList.Add(float.Parse(ls));
                        }
                        dslfDic.Add(dicArray[0].ToString(), dliList);
                    }
                }
                value = dslfDic;
                break;
            case ConfigManager.PropertiesType.DICT_FLOAT_FLOAT:
                str = obj.ToString();
                str = str.Replace("{", "");
                str = str.Replace("}", "");
                if (str.Equals(""))
                    return;
                dicArray = str.Split(new char[] { ',' });
                Dictionary<float, float> dffDic = new Dictionary<float, float>();
                foreach (string s in dicArray) {
                    dicArray = s.Split(new char[] { ':' });
                    if (dicArray.Length == 2) {
                        dffDic.Add(float.Parse(dicArray[0].ToString()), float.Parse(dicArray[1].ToString()));
                    }
                }
                value = dffDic;
                break;
            case ConfigManager.PropertiesType.DICT_FLOAT_INT:
                str = obj.ToString();
                str = str.Replace("{", "");
                str = str.Replace("}", "");
                if (str.Equals(""))
                    return;
                dicArray = str.Split(new char[] { ',' });
                Dictionary<float, int> dfiDic = new Dictionary<float, int>();
                foreach (string s in dicArray) {
                    dicArray = s.Split(new char[] { ':' });
                    if (dicArray.Length == 2) {
                        dfiDic.Add(float.Parse(dicArray[0].ToString()), int.Parse(dicArray[1].ToString()));
                    }
                }
                value = dfiDic;
                break;
            case ConfigManager.PropertiesType.DICT_FLOAT_STR:
                str = obj.ToString();
                str = str.Replace("{", "");
                str = str.Replace("}", "");
                if (str.Equals(""))
                    return;
                dicArray = str.Split(new char[] { ',' });
                Dictionary<float, string> dfsDic = new Dictionary<float, string>();
                foreach (string s in dicArray) {
                    dicArray = s.Split(new char[] { ':' });
                    if (dicArray.Length == 2) {
                        dfsDic.Add(float.Parse(dicArray[0].ToString()), dicArray[1].ToString());
                    }
                }
                value = dfsDic;
                break;
            case ConfigManager.PropertiesType.DICT_FLOAT_LIST_INT:
                str = obj.ToString();
                str = str.Replace("{", "");
                str = str.Replace("}", "");
                if (str.Equals(""))
                    return;
                dicArray = str.Split(new char[] { ',' });
                Dictionary<float, List<int>> dfliDic = new Dictionary<float, List<int>>();
                foreach (string s in dicArray) {
                    dicArray = s.Split(new char[] { ':' });
                    if (dicArray.Length == 2) {
                        split = dicArray[1];
                        split.Replace("[", "");
                        split.Replace("]", "");
                        listArray = split.Split(new char[] { ',' });
                        List<int> dliList = new List<int>();
                        foreach (string ls in listArray) {
                            dliList.Add(int.Parse(ls));
                        }
                        dfliDic.Add(float.Parse(dicArray[0].ToString()), dliList);
                    }
                }
                value = dfliDic;
                break;
            case ConfigManager.PropertiesType.DICT_FLOAT_LIST_STR:
                str = obj.ToString();
                str = str.Replace("{", "");
                str = str.Replace("}", "");
                if (str.Equals(""))
                    return;
                dicArray = str.Split(new char[] { ',' });
                Dictionary<float, List<string>> dflsDic = new Dictionary<float, List<string>>();
                foreach (string s in dicArray) {
                    dicArray = s.Split(new char[] { ':' });
                    if (dicArray.Length == 2) {
                        split = dicArray[1];
                        split.Replace("[", "");
                        split.Replace("]", "");
                        listArray = split.Split(new char[] { ',' });
                        List<string> dliList = new List<string>();
                        foreach (string ls in listArray) {
                            dliList.Add(ls);
                        }
                        dflsDic.Add(float.Parse(dicArray[0].ToString()), dliList);
                    }
                }
                break;
                value = dflsDic;
            case ConfigManager.PropertiesType.DICT_FLOAT_LIST_FLOAT:
                str = obj.ToString();
                str = str.Replace("{", "");
                str = str.Replace("}", "");
                if (str.Equals(""))
                    return;
                dicArray = str.Split(new char[] { ',' });
                Dictionary<float, List<float>> dflfDic = new Dictionary<float, List<float>>();
                foreach (string s in dicArray) {
                    dicArray = s.Split(new char[] { ':' });
                    if (dicArray.Length == 2) {
                        split = dicArray[1];
                        split.Replace("[", "");
                        split.Replace("]", "");
                        listArray = split.Split(new char[] { ',' });
                        List<float> dliList = new List<float>();
                        foreach (string ls in listArray) {
                            dliList.Add(float.Parse(ls));
                        }
                        dflfDic.Add(float.Parse(dicArray[0].ToString()), dliList);
                    }
                }
                value = dflfDic;
                break;
        }
        Info.SetValue(t, value);
    }

}
