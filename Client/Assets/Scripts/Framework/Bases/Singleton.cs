//----------------------------------------------
//            liuaf UnityFramework
// Copyright © 2015-2025 liuaf Entertainment
// Created by : Liu Aifei (329737941@qq.com)
//----------------------------------------------

using UnityEngine;
using System.Collections;

public class Singleton<T> where T : Singleton<T>, new() {
    protected static readonly object staticSyncRoot = new object();
    private static T instance = null;

    public static T Instance {
        get {
            if (instance == null) {
                lock (staticSyncRoot) {
                    if (instance == null) 
                        instance = new T();
                }
            }
            return instance;
        }
    }

}
