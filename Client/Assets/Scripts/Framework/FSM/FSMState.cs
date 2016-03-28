using System;

public class FSMState : IFSMState
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
