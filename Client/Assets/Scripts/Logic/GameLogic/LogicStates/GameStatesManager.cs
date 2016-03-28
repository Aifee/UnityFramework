//----------------------------------------------
//            liuaf threeKingdoms Project
// Copyright © 2015-2017 threeKingdoms
// Created by : Liu Aifei (329737941@qq.com)
//--------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Reflection;
using System;

/// <summary>
/// @Summary : 游戏进度管理
/// @Author : Liu Aifei
/// @Date : 2014.04.23
/// </summary>
public class GameStatesManager
{
    #region Instance
    
    private static GameStatesManager m_Instance;
    public static GameStatesManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new GameStatesManager();
            }
            return m_Instance;
        }
    }
    
    #endregion
    
    
    //保存所有的GameState实例.
    private Dictionary<string, GameState> m_GameStateMap = null;
    
    //当前状态引用.
    private GameState m_CurrentState = null;
    private bool isPlaying = true;

    /// <summary>
    /// 构造函数.根据配置表进行初始化.
    /// </summary>
    public GameStatesManager()
    {
        m_GameStateMap = new Dictionary<string, GameState>();
        m_CurrentState = new LoginGameState();
        m_GameStateMap.Add(SceneName.Login.ToString(), m_CurrentState);
        m_GameStateMap.Add(SceneName.Manager.ToString(), new ManagerGameState());
        m_GameStateMap.Add(SceneName.Fight.ToString(), new FightGameState());
        SetState(m_CurrentState);
    }

    public GameState CurrentState{
        get{
            return m_CurrentState;
        }
    }
    
    /// <summary>
    /// 每帧间的更新.被主循环驱动.
    /// </summary>
    /// <param name="dt">帧间隔时间</param>
    public void UpdateState(float dt)
    {
        if (null != m_CurrentState && m_CurrentState.isLoaded)
        {
            m_CurrentState.Update(dt);
        }
    }
    
    private void SetState(GameState state)
    {
        if (null != m_CurrentState)
        {
            m_CurrentState.Stop();
        }
        
        m_CurrentState = state;
        if (null != m_CurrentState)
        {
            m_CurrentState.Start();
            m_CurrentState.LoadScene();
        }
    }
    
    /// <summary>
    /// 收到状态切换请求时的处理(注：如果目标GameState对象id为null则为退出应用程序).
    /// </summary>
    /// <param name="requiredState"></param>
    public void SwitchState(string targetGameStateId)
    {
        if (m_GameStateMap.ContainsKey(targetGameStateId))
        {
            //得到目标GameState对象;
            GameState targetGameState = m_GameStateMap[targetGameStateId];
            SetState(targetGameState);
        }
        else
        {
            Debug.LogError("Not found GameState obj:" + targetGameStateId);
        }
    }
}