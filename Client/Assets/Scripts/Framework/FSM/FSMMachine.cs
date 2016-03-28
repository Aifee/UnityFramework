using System;
using UnityEngine;
using System.Collections.Generic;

public class FSMMachine : IFSMMachine
{
    protected StateID stateId;
    protected IFSMState currentState;
    protected IFSMState defaultState;
    protected IFSMState goalState;
    protected List<IFSMState> m_lsStates = new List<IFSMState>();
    protected Queue<StateID> switchQueue = new Queue<StateID>();

    public virtual void AddState(IFSMState kState)
    {
        this.m_lsStates.Add(kState);
    }

    public void SwitchState(StateID id) {
        switchQueue.Enqueue(id);
    }
    public virtual IFSMState GetCurrentState()
    {
        return currentState;
    }

    public StateID GetCurrentStateID()
    {
        return currentState.GetID();
    }

    public virtual IFSMState GetState(StateID eID)
    {
        foreach (IFSMState state in this.m_lsStates)
        {
            if (state.GetID() == eID)
            {
                return state;
            }
        }
        return null;
    }

    public virtual void SetDefaultState(IFSMState kState)
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
    public virtual void Update(float fDelta)
    {
    }


}
