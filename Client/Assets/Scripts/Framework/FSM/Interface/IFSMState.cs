using UnityEngine;
using System.Collections;

public interface IFSMState {

    StateID CheckTransitions();
    void Enter();
    void Exit();
    StateID GetID();
    void Update(float fDelta);


}
