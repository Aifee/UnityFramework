//----------------------------------------------
//            liuaf UnityFramework
// Copyright © 2015-2025 liuaf Entertainment
// Created by : Liu Aifei (329737941@qq.com)
//----------------------------------------------

using UnityEngine;
using System.Collections;

public class MainLooper : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.AddComponent<FPS>();
        gameObject.AddComponent<Client>();
        gameObject.AddComponent<DontDestroyMe>();
        gameObject.AddComponent<ResourcesManager>();
        gameObject.AddComponent<InGameLog>();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 45;

	}
	
	// Update is called once per frame
	void Update () {
        GameStatesManager.Instance.UpdateState(Time.deltaTime);
	}
    bool FingerGesturesGlobalFilter( int fingerIndex, Vector2 position )
    { 
        Ray ray = UIManager.UICamera.ScreenPointToRay(new Vector3(position.x , position.y, 0));
        bool touchUI = Physics.Raycast( ray, float.PositiveInfinity);  
        //Debug.Log("touchUI:" + touchUI);
        return !touchUI;
    }
}
