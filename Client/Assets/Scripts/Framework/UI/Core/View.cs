//----------------------------------------------
//            liuaf UnityFramework
// Copyright © 2015-2025 liuaf Entertainment
// Created by : Liu Aifei (329737941@qq.com)
//----------------------------------------------

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class View : Notifier,IView 
{
    protected GameObject UIPrefab;
    public bool IsDefalut { get; private set; }
    public UILayer Layer { get; set; }
    protected SimpleCommand Command { get; private set; }
    public View() {
    }

    public void Activation(GameObject go) {
        UIPrefab = go;
        Start();
    }

    protected virtual void Start() {

    }
    protected virtual void StartRegisterEvents() {

    }
    protected virtual void Enable() {

    }
    protected virtual void Dormancy() {

    }


    public void Showing() {
        if (UIPrefab == null) {
            Debug.LogError("Controlled by UIPrefab is null,Please check the loading process is correct..");
        } else {
            UIPrefab.SetActive(true);
            Enable();
        }
    }

    public void Hiding() {
        if (UIPrefab == null) {
            Debug.LogError("Controlled by UIPrefab is null,Please check the loading process is correct..");
        } else {
            UIPrefab.SetActive(false);
            Dormancy();
        }
    }
    public void SetCommand(SimpleCommand command) {
        Command = command;
    }
    public virtual void Destroy() {
        GameObject.Destroy(UIPrefab);
    }
    protected T GetChild<T>(string childName) where T : MonoBehaviour {
        T[] childs = UIPrefab.GetComponentsInChildren<T>();
        GameObject child = null;
        foreach (T t in childs) {
            if (childName == t.name) {
                child = t.gameObject;
            }
        }
        if (child == null) {
            Debug.LogError(childName + " is not child of " + UIPrefab.name);
            return null;
        }

        T temp = child.GetComponent<T>();
        if (temp == null) {
            Debug.LogError(childName + " is not has component ");
        }

        return temp;
    }

    protected GameObject GetChild(string childName) {
        GameObject child = UIPrefab.transform.FindChild(childName).gameObject;
        if (child == null) {
            Debug.Log(UIPrefab + "is not find child of " + childName);
        }
        return child;
    }

    private void GetLabelContent(GameObject UIPrefab) {
        UILabel[] LabelArray = UIPrefab.transform.GetComponentsInChildren<UILabel>();
        foreach (UILabel label in LabelArray) {
            if (label != null) {
                // 设置层次
                label.transform.localPosition = new Vector3(label.transform.localPosition.x, label.transform.localPosition.y, label.transform.localPosition.z - 1);

                // 获取文本配置的内容
                string Lab_Content = label.text;

                if (Lab_Content != null && Lab_Content != "") {
                    // 根据配置内容获取text.xml里面配置的内容
                    string Txt_Content = "";// DataManager.Instance.GetText(Lab_Content);
                    if (Txt_Content != null && Txt_Content != "") {
                        label.text = Txt_Content;
                    }
                }
            }
        }
    }


}
