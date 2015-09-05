//----------------------------------------------
//            liuaf UnityFramework
// Copyright © 2015-2025 liuaf Entertainment
// Created by : Liu Aifei (329737941@qq.com)
//----------------------------------------------

using UnityEngine;
using System.Collections;

public static class TransformUtil {

    public static void Reset(Transform trans) {
        if (trans == null) { return; }
        trans.localPosition = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = Vector3.one;
    }

    public static void Reset(Transform trans, Transform parent) {
        if (trans == null) { return; }
        trans.parent = parent;
        Reset(trans);
    }

}