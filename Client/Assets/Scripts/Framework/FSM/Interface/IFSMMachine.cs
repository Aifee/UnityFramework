using UnityEngine;
using System.Collections;

public interface IFSMMachine {

    void AddState(IFSMState kState);
    void SwitchState(StateID id);
    IFSMState GetCurrentState();
    StateID GetCurrentStateID();
    IFSMState GetState(StateID eID);
    void SetDefaultState(IFSMState kState);
    void UpdateMachine(float fDelta);
    void Update(float fDelta);
}
