using System;
using UnityEngine;

public class Fps : MonoBehaviour
{
    public string ex_string;
    private int frame_count;
    private int frame_rate;
    private long time_start;
    private Rect windowRect = new Rect((float) (Screen.width - 140), 20f, 120f, 100f);

    private void OnGUI()
    {
        this.windowRect = GUI.Window(0, this.windowRect, new GUI.WindowFunction(this.windowProc), "FPS");
    }

    private void Start()
    {
        Application.targetFrameRate = 1000;
    }
    private void Update()
    {
        this.frame_count++;
        if ((DateTime.Now.Ticks - this.time_start) > 10000000.0)
        {
            this.frame_rate = this.frame_count;
            this.frame_count = 0;
            this.time_start = DateTime.Now.Ticks;
        }
    }

    private void windowProc(int id)
    {
        GUI.Label(new Rect(10f, 20f, 100f, 30f), string.Concat(new object[] { "FPS=", this.frame_rate, " ", this.ex_string }));
        GUI.Label(new Rect(10f, 50f, 100f, 30f), Screen.width + ":" + Screen.height);
        GUI.DragWindow();
    }
}

