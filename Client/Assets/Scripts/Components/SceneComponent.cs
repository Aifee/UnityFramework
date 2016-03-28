using UnityEngine;
using System.Collections;

public enum eSceneComponent{
    Grid = 0,
    Building = 1,
}

public class SceneComponent : MonoBehaviour {

    public eSceneComponent Type;
    public object Data{get;private set;}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void Reset(eSceneComponent type,object obj){
        Type = type;
        Data = obj;
    }
}
