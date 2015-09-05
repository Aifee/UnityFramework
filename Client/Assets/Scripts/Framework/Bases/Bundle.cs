//----------------------------------------------
//            liuaf UnityFramework
// Copyright © 2015-2025 liuaf Entertainment
// Created by : Liu Aifei (329737941@qq.com)
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class Bundle {

    private Dictionary<string, System.Object> _Data = new Dictionary<string, System.Object>();

    public static Bundle Create<T>(string key, T val) {
        Bundle bundle = new Bundle();
        bundle.SetValue<T>(key, val);
        return bundle;
    }
    public void SetValue<T>(string key, T val) {
        if (_Data.ContainsKey(key))
            _Data.Remove(key);
        _Data.Add(key, val);
    }

    public T GetValue<T>(string key) {
        System.Object obj;
        return _Data.TryGetValue(key, out obj) && (obj is T) ? (T)obj : default(T);
    }

    public bool ContainsKey(string key) {
        return _Data.ContainsKey(key);
    }

    public bool ContainsKey<T>(string key) {
        System.Object obj;
        return _Data.TryGetValue(key, out obj) && (obj is T);
    }

    public void Remove(string key) {
        if (ContainsKey(key))
            _Data.Remove(key);
    }

    public override string ToString() {
        List<string> contents = new List<string>();
        foreach (KeyValuePair<string, System.Object> kv in _Data) {
            System.Object val = kv.Value;
            if (val is System.Array) {
                System.Array arr = val as System.Array;
                int len = arr.Length;
                string[] arrayContents = new string[len];
                for (int i = 0; i < len; i++) {
                    arrayContents[i] = string.Format("{0}", arr.GetValue(i));
                }
                val = string.Format("[{0}]", string.Join(", ", arrayContents));
            }

            contents.Add(string.Format("{0}:{1}", kv.Key, val));
        }
        return string.Concat("[Bundle]{", string.Join(", ", contents.ToArray()), "}");
    }

}
