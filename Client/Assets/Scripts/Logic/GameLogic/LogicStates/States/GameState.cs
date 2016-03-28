using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SceneName{
    Login,
    Manager,
    Fight,
}
/// <summary>
/// @Summary : ????
/// @Author : Liu Aifei
/// @Date : 2014.04.23
/// </summary>
public class GameState
{
    public bool isLoaded{get;protected set;}
    public GameState()
    {

    }

    /// <summary>
    /// GameState初始话函数.
    /// </summary>
    public virtual void Start() { }

    public virtual void LoadScene(){}
    /// <summary>
    /// 帧间隔中调用.
    /// </summary>
    /// <param name="dt">帧间隔时间</param>
    public virtual void Update(float dt) { }

    /// <summary>
    /// GameState终止函数
    /// </summary>
    public virtual void Stop() { }

    public virtual void OnClick(Gesture gesture){}
    public virtual void OnLongTap(Gesture gesture){}
    public virtual void OnDragStart(Gesture gesture){}
    public virtual void OnDrag(Gesture gesture){}
    public virtual void OnPinch(Gesture gesture){}
    public virtual void OnDragEnd(Gesture gesture){}
}
