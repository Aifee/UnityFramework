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
        //gameObject.AddComponent<Client>();
        gameObject.AddComponent<DontDestroyMe>();
        //gameObject.AddComponent<ResourcesManager>();
        //UIManager.Instance.ShowView<LoginView>("LoginView");
	}
	
	// Update is called once per frame
	void Update () {
        GameStateControl.Instance.Update(Time.deltaTime);
	}
}
