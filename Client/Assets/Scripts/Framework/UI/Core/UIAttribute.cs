//----------------------------------------------
//            liuaf UnityFramework
// Copyright © 2015-2025 liuaf Entertainment
// Created by : Liu Aifei (329737941@qq.com)
//----------------------------------------------

using System;

[AttributeUsage(AttributeTargets.Class)]
public class UIAttribute : Attribute {

    public UILayer Layer { get; set; }
    public System.Type CommandType { get; set; }

}
