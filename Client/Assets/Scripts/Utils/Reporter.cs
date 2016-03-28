using UnityEngine;
using System.Collections;
/// <summary>
/// 一个自动获取设备信息的脚本，可以把收集到的信息发送到指定的服务器中
/// </summary>
public class Reporter : MonoBehaviour {
    
    private float TS = 0;
    public  GameObject fps;
    public  float countdown = 5;
    public  float thankYouTextTimeOut = 60;
    public  string thankYouText ="Thanks!";
    public  string URL = "";//"http://your-domain-here.com/Reporter.php";
    
    // Use this for initialization
    void Start () 
    {
        TS = Time.time + countdown;
    }
    
    // Update is called once per frame
    void Update () 
    {
        if(Time.time > TS && TS != -1)
        {
            WWWForm form = new WWWForm();
            TS = -1; // oneshot only
            
            try { form.AddField("FPS", fps.GetComponent<GUIText>().text.ToString()); } finally {} {  }
            try { form.AddField("Machine Name",System.Environment.MachineName); } finally {} {  }
            try { form.AddField("Operating System",System.Environment.OSVersion.ToString()); } finally {} {  }
            try { form.AddField("User Name",System.Environment.UserName); } finally {} {  }
            try { form.AddField(".NET Version",System.Environment.Version.ToString()); } finally {} {  }
            try { form.AddField("CPUCount",System.Environment.ProcessorCount); } finally {} {  }
            try { form.AddField("Command",System.Environment.CommandLine); } finally {} {  }
            try { form.AddField("CWD",System.Environment.CurrentDirectory); } finally {} {  }
            try { form.AddField("SYS",System.Environment.SystemDirectory); } finally {} {  }
            try { form.AddField("Runtime",Application.platform.ToString()); } finally {} {  }
            if(Application.isEditor)
            {
                try { form.AddField("Running In","editor"); } finally {} {  }
            }
            else
            {
                try { form.AddField("Running In","runtime"); } finally {} {  }
            }
            try { 
                form.AddField("URL",Application.absoluteURL);
                form.AddField("Src:",Application.absoluteURL);
                form.AddField("Unity:",Application.unityVersion);
            } finally {} {  }
            //try { data+= ""+System.Environment.+"\n";
            //Debug.Log(data);
            try {
                // This next section is Unity2.0 only, just comment/delete if you're still using unity 1.x
                form.AddField("graphicsMemorySize",SystemInfo.graphicsMemorySize);
                form.AddField("graphicsDeviceName",SystemInfo.graphicsDeviceName);
                form.AddField("graphicsDeviceVendor",SystemInfo.graphicsDeviceVendor);
                form.AddField("graphicsDeviceVersion",SystemInfo.graphicsDeviceVersion);
                //
                form.AddField("supportsShadows", SystemInfo.supportsShadows ? "supported" : "unsupported");
                form.AddField("supportsRenderTextures",SystemInfo.supportsRenderTextures ? "supported" : "unsupported");
                form.AddField("supportsImageEffects",SystemInfo.supportsImageEffects ? "supported" : "unsupported");
            } finally {} {  }
            WWW www = new WWW(URL, form);
            //yield www;
            this.GetComponent<GUIText>().text = thankYouText;
            Destroy(this.gameObject,thankYouTextTimeOut);
        }
        
    }
}