using UnityEngine;
using System.Net;
using System.Collections;

public class WebClient : MonoBehaviour {
    static public WebClient Instance;
    private float outputTest = 0;
    WebAsync webAsync = new WebAsync();
    void Awake () {

        Instance = GetComponent<WebClient>();
    }
    static private IEnumerator CheckURL () {
        bool foundURL;
        string checkThisURL = "http://www.example.com/index.html";
        WebAsync webAsync = new WebAsync();
        
        yield return Instance.StartCoroutine( webAsync.CheckForMissingURL(checkThisURL) );
        Debug.Log("Does "+ checkThisURL  +" exist? "+ webAsync.isURLmissing);

    }
	// Use this for initialization
	void Start () {
        Profile.StartProfile("Start");
        StartCoroutine( AreWeConnectedToInternet() );
	}
	
	// Update is called once per frame
	void Update () {
        Profile.StartProfile("Update");
        for (int i = 0; i < 100; ++i)
            outputTest += Mathf.Sin(i * Time.time);
        Profile.EndProfile("Update");
	}
    private IEnumerator AreWeConnectedToInternet () {
        bool areWe;
        WebRequest requestAnyURL = HttpWebRequest.Create("http://www.example.com");
        requestAnyURL.Method = "HEAD";
        
        IEnumerator e = webAsync.GetResponse(requestAnyURL);
        while ( e.MoveNext() ) { yield return e.Current; }
        
        areWe = (webAsync.requestState.errorMessage == null);

        Debug.Log("Are we connected to the inter webs? "+ areWe);
    }

    void OnApplicationQuit()
    {
        Profile.EndProfile("Start");
        Debug.Log("outputTest is " + outputTest.ToString("F3"));
        Profile.PrintResults();
    }

}
