//----------------------------------------------
//            liuaf UnityFramework
// Copyright © 2015-2025 liuaf Entertainment
// Created by : Liu Aifei (329737941@qq.com)
//----------------------------------------------

using System;

public class FSMState
{
    protected StateID stateId;
    protected FSMControl control;

    public FSMState(StateID eID, FSMControl kControl)
    {
        stateId = eID;
        control = kControl;
    }


    public virtual StateID CheckTransitions()
    {
        return stateId;
    }

    public virtual void Enter()
    {
    }

    public virtual void Exit()
    {
    }

    public StateID GetID()
    {
        return stateId;
    }

    public virtual void Update(float fDelta)
    {
    }
}
