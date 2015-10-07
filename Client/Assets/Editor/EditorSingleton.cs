using UnityEngine;
using UnityEditor;
using System.Collections;

public class EditorSingleton<T> : EditorWindow where T : EditorSingleton<T> {

	static private T instance = null;
	static public T Instance{get{return instance;}}
	protected void OnEnable(){
		instance = this as T;
		Init ();
	}
	protected void OnDisable(){
		Disable ();
	}
	protected virtual void Init(){

	}
	protected virtual void Disable(){

	}
}
