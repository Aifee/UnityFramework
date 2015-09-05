//----------------------------------------------
//            liuaf UnityFramework
// Copyright © 2015-2025 liuaf Entertainment
// Created by : Liu Aifei (329737941@qq.com)
//----------------------------------------------

using UnityEngine;
public interface IView {
    void Activation(GameObject go);
    void Showing();
    void Hiding();
    void Destroy();
    void SetCommand(SimpleCommand command);
}
