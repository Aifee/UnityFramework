using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {
    private VersionManager versionManager;
	// Use this for initialization
	void Start () {
        versionManager = new VersionManager();
        versionManager.CheckVersion();
	}
	
	// Update is called once per frame
	void Update () {
	
	}



}
