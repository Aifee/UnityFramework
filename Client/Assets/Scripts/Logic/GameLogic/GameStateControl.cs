//----------------------------------------------
//            liuaf UnityFramework
// Copyright © 2015-2025 liuaf Entertainment
// Created by : Liu Aifei (329737941@qq.com)
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameStateControl : FSMControl {

    #region Intance

    private static GameStateControl _instance;
    public static GameStateControl Instance {
        get {
            if (_instance == null)
                _instance = new GameStateControl();
            return _instance;
        }
    }

    #endregion


    public GameStateControl() {
        GameLoginState currentState = new GameLoginState(StateID.GAME_LOGIN, this);
        base.machines.AddState(currentState);
        base.machines.AddState(new GameStartState(StateID.GAME_ENTER, this));
        base.machines.AddState(new GameFightState(StateID.GAME_FIGHT, this));

        base.machines.SetDefaultState(currentState);
    }
    public void Switch(StateID id) {
        base.machines.SwitchState(id);
    }
    public void Update(float fDelta) {
        base.machines.UpdateMachine(fDelta);
    }
    
}
