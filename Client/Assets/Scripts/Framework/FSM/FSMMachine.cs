//----------------------------------------------
//            liuaf UnityFramework
// Copyright © 2015-2025 liuaf Entertainment
// Created by : Liu Aifei (329737941@qq.com)
//----------------------------------------------

using System;
using UnityEngine;
using System.Collections.Generic;

public class FSMMachine
{
    private StateID stateId;
    private FSMState currentState;
    private FSMState defaultState;
    private FSMState goalState;
    private List<FSMState> m_lsStates = new List<FSMState>();
    private Queue<StateID> switchQueue = new Queue<StateID>();

    public virtual void AddState(FSMState kState)
    {
        this.m_lsStates.Add(kState);
    }

    public void SwitchState(StateID id) {
        switchQueue.Enqueue(id);
    }
    public FSMState GetCurrentState()
    {
        return currentState;
    }

    public StateID GetCurrentStateID()
    {
        return currentState.GetID();
    }

    public virtual FSMState GetState(StateID eID)
    {
        foreach (FSMState state in this.m_lsStates)
        {
            if (state.GetID() == eID)
            {
                return state;
            }
        }
        return null;
    }

    public virtual void SetDefaultState(FSMState kState)
    {
        defaultState = kState;
    }

    public virtual void UpdateMachine(float fDelta)
    {
        if (this.m_lsStates.Count != 0)
        {
            if (currentState == null)
            {
                currentState = defaultState;
                currentState.Enter();
            }
            if (currentState != null)
            {
                StateID iD = currentState.GetID();
                stateId = currentState.CheckTransitions();
                if (switchQueue.Count > 0) {
                    stateId = switchQueue.Dequeue();
                }
                if (stateId != iD) {
                    goalState = this.GetState(stateId);
                    if (goalState != null) {
                        currentState.Exit();
                        currentState = goalState;
                        currentState.Enter();
                    }
                }
                currentState.Update(fDelta);
            }
        }
    }
}
