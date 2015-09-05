//----------------------------------------------
//            liuaf UnityFramework
// Copyright © 2015-2025 liuaf Entertainment
// Created by : Liu Aifei (329737941@qq.com)
//----------------------------------------------

using UnityEngine;
using System.Collections;

public abstract class SingletonComponent<T> : MonoBehaviour where T : SingletonComponent<T> {

    private static T instance = null;

    protected void Awake() {
        if (instance != null) {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this as T;
    }

    protected void OnDestroy() {
        instance = null;
    }

    public static T Instance { get { return instance; } }

}