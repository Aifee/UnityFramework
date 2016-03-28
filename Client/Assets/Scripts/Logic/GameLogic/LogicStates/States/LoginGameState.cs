//----------------------------------------------
//            liuaf threeKingdoms Project
// Copyright © 2015-2017 threeKingdoms
// Created by : Liu Aifei (329737941@qq.com)
//--------------------------------------------
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// @Summary : 登陆状态
/// @Author : LiuAifei
/// @Date : 2014.02.23
/// </summary>
public class LoginGameState : GameState
{
    public LoginGameState()
    {
        //m_Control = new LoginGameStateControl(this);
    }
    /// <summary>
    /// 状态开始
    /// </summary>
    public override void Start()
    {
        Debug.Log("login state!!");
        UIManager.Instance.Show<LoginPanel>();
        isLoaded = true;
    }



    public override void Stop()
    {
        UIManager.Instance.DestroyAll();
    }
    
}
