//----------------------------------------------
//            liuaf threeKingdoms Project
// Copyright © 2010-2015 threeKingdoms
// Created by : Liu Aifei (329737941@qq.com)
//--------------------------------------------
using UnityEngine;
using System.Collections;

public static class TransformUtil {
    /// <summary>
    /// 重置局部坐标为 Vector3.zero
    /// </summary>
    /// <param name="tran">Tran.</param>
    public static void ResetPosition(this Transform tran){
        if(tran == null) return;
        tran.localPosition = Vector3.zero;
    }
    /// <summary>
    /// 重置局部坐标为 v
    /// </summary>
    /// <param name="tran">Tran.</param>
    /// <param name="v">V.</param>
    public static void ResetPosition(this Transform tran,Vector3 v){
        if(tran == null) return;
        tran.localPosition = v;
    }
    /// <summary>
    /// 重置局部四元数旋转为 Quaternion.identity
    /// </summary>
    /// <param name="tran">Tran.</param>
    public static void ResetRotation(this Transform tran){
        if(tran == null) return;
        tran.localRotation = Quaternion.identity;
    }
    /// <summary>
    /// 重置局部四元数旋转为 q
    /// </summary>
    /// <param name="tran">Tran.</param>
    /// <param name="q">Q.</param>
    public static void ResetRotation(this Transform tran, Quaternion q){
        if(tran == null) return;
        tran.localRotation = q;
    }
    /// <summary>
    /// 重置局部欧拉角旋转为 v
    /// </summary>
    /// <param name="tran">Tran.</param>
    /// <param name="v">V.</param>
    public static void ResetEulerAngles(this Transform tran, Vector3 v){
        if(tran == null) return;
        tran.eulerAngles = v;
    }
    /// <summary>
    /// 重置局部缩放为 Vector3.one
    /// </summary>
    /// <param name="tran">Tran.</param>
    public static void ResetScale(this Transform tran){
        if(tran == null) return;
        tran.localScale = Vector3.one;
    }
    /// <summary>
    /// 重置局部缩放为 v
    /// </summary>
    /// <param name="tran">Tran.</param>
    /// <param name="v">V.</param>
    public static void ResetScale(this Transform tran,Vector3 v){
        if(tran == null) return;
        tran.localScale = v;
    }
    /// <summary>
    /// 重置
    /// --局部坐标为 Vector3.zero
    /// --局部四元数旋转为 Quaternion.identity
    /// --局部缩放为 Vector3.one
    /// </summary>
    /// <param name="tran">Tran.</param>
    public static void Reset(this Transform tran) {
        if (tran == null) { return; }
        tran.ResetScale();
        tran.ResetRotation();
        tran.ResetPosition();
    }
    /// <summary>
    /// 重置 parent
    /// </summary>
    /// <param name="trans">Trans.</param>
    /// <param name="parent">Parent.</param>
    public static void ResetParent(this Transform trans, Transform parent) {
        if (trans == null) { return; }
        trans.parent = parent;
        trans.Reset();
    }
    /// <summary>
    /// 重置局部坐标 x
    /// </summary>
    /// <param name="tran">Tran.</param>
    /// <param name="x">The x coordinate.</param>
    public static void ResetPositionX(this Transform tran, float x){
        if(tran == null) return;
        Vector3 v = tran.localPosition;
        v.x = x;
        tran.localPosition = v;
    }
    /// <summary>
    /// 重置局部坐标 y
    /// </summary>
    /// <param name="tran">Tran.</param>
    /// <param name="y">The y coordinate.</param>
    public static void ResetPositionY(this Transform tran, float y){
        if(tran == null) return;
        Vector3 v = tran.localPosition;
        v.y = y;
        tran.localPosition = v;
    }

    /// <summary>
    /// 重置局部坐标 x
    /// </summary>
    /// <param name="tran">Tran.</param>
    /// <param name="z">The z coordinate.</param>
    public static void ResetPositionZ(this Transform tran, float z){
        if(tran == null) return;
        Vector3 v = tran.localPosition;
        v.z = z;
        tran.localPosition = v;
    }
    /// <summary>
    /// 重置局部缩放 x
    /// </summary>
    /// <param name="tran">Tran.</param>
    /// <param name="x">The x coordinate.</param>
    public static void ResetScaleX(this Transform tran, float x){
        if(tran == null) return;
        Vector3 v = tran.localScale;
        v.x = x;
        tran.localScale = v;
    }
    /// <summary>
    /// 重置局部缩放 y
    /// </summary>
    /// <param name="tran">Tran.</param>
    /// <param name="y">The y coordinate.</param>
    public static void ResetScaleY(this Transform tran, float y){
        if(tran == null) return;
        Vector3 v = tran.localScale;
        v.y = y;
        tran.localScale = v;
    }
    /// <summary>
    /// 重置局部缩放 z
    /// </summary>
    /// <param name="tran">Tran.</param>
    /// <param name="z">The z coordinate.</param>
    public static void ResetScaleZ(this Transform tran, float z){
        if(tran == null) return;
        Vector3 v = tran.localScale;
        v.z = z;
        tran.localScale = v;
    }
    /// <summary>
    /// 重置局部欧拉角旋转 x
    /// </summary>
    /// <param name="tran">Tran.</param>
    /// <param name="x">The x coordinate.</param>
    public static void ResetEulerAngleX(this Transform tran,float x){
        if(tran == null) return;
        Vector3 v = tran.eulerAngles;
        v.x = x;
        tran.eulerAngles = v;
    }
    /// <summary>
    /// 重置局部欧拉角旋转 y
    /// </summary>
    /// <param name="tran">Tran.</param>
    /// <param name="y">The y coordinate.</param>
    public static void ResetEulerAngleY(this Transform tran,float y){
        if(tran == null) return;
        Vector3 v = tran.eulerAngles;
        v.y = y;
        tran.eulerAngles = v;
    }
    /// <summary>
    /// 重置局部欧拉角旋转 z
    /// </summary>
    /// <param name="tran">Tran.</param>
    /// <param name="z">The z coordinate.</param>
    public static void ResetEulerAngleZ(this Transform tran,float z){
        if(tran == null) return;
        Vector3 v = tran.eulerAngles;
        v.z = z;
        tran.eulerAngles = v;
    }
    /// <summary>
    /// 删除Transform的child节点
    /// </summary>
    /// <param name="tran">Tran.</param>
    /// <param name="child">Child.</param>
    public static void DeleteChild(this Transform tran,string child){
        if(tran == null) return;
        Transform childTran = tran.FindChild(child);
        if(childTran == null) return;
        GameObject.Destroy(childTran.gameObject);
    }
    /// <summary>
    /// 重置子节点 child 是否显示
    /// </summary>
    /// <param name="go">Go.</param>
    /// <param name="child">Child.</param>
    /// <param name="active">If set to <c>true</c> active.</param>
    public static void ActiveChild(this GameObject go,string child,bool active){
        if(go == null) return;
        Transform childTran = go.transform.FindChild(child);
        if(childTran == null) return;
        childTran.gameObject.SetActive(active);
    }
    /// <summary>
    /// 获取某个GameObject下的子节点脚本
    /// </summary>
    /// <returns>The child.</returns>
    /// <param name="go">Go.</param>
    /// <param name="childName">Child name.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static T GetChild<T>(this GameObject go,string childName) where T : MonoBehaviour{
        T[] childs = go.GetComponentsInChildren<T>();
        GameObject child = null;
        foreach (T t in childs) {
            if (childName.Equals(t.name))
                child = t.gameObject;
        }
        if (child == null) {
            Debug.LogError(childName + "is not child of" + go.name);
            return null;
        }
        T tempT = child.GetComponent<T>();
        if (tempT == null)
            Debug.LogError(childName + "is not has component");
        
        return tempT;
    }
}