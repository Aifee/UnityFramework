//----------------------------------------------
//            MobArts PiDan Project
// Copyright © 2010-2015 MobArts Entertainment
// Created by : Liu Aifei (329737941@qq.com)
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class PacketAttribute : Attribute {
    public short MessageId {
        get;
        set;
    }

    public PacketAttribute(short messageId) {
        MessageId = messageId;
    }
}