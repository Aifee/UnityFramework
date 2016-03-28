//----------------------------------------------
//            liuaf threeKingdoms Project
// Copyright © 2010-2015 threeKingdoms
// Created by : Liu Aifei (329737941@qq.com)
//----------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// @Summary : The object processing is not destroyed when the scene is switched
/// @Auther : liuaifei
/// @Date : 2014.10.26
/// </summary>
/// 
public class DontDestroyMe : MonoBehaviour 
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
