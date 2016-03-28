using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Net;

public class ThreadEvent{
    public string Key;
    public List<object> evParams = new List<object>();
}
public class NotiData{
    public string evName;
    public object evParam;

    public NotiData(string name,object param){
        this.evName = name;
        this.evParam = param;
    }
}


public class ThreadMananger : SingletonComponent<ThreadMananger> {

    private Thread thread;
    private Action<NotiData> func;
    private Stopwatch sw = new Stopwatch();
    private string currDownFile = string.Empty;

    static readonly object m_synObject = new object();
    static Queue<ThreadEvent> events = new Queue<ThreadEvent>();

    delegate void ThreadSyncEvent(NotiData data);
    private ThreadSyncEvent m_SyncEvent;

    void Start(){
        m_SyncEvent = OnSyncEvent;
        thread = new Thread(OnUpdate);
        thread.Start();
    }
    public void AddEvent(ThreadEvent ev, Action<NotiData> func){
        lock(m_synObject){
            this.func = func;
            events.Enqueue(ev);
        }
    }
    private void OnSyncEvent(NotiData data){
        if(this.func != null) func(data);
        ///通知给UI底层，说明事件
    }
    void OnUpdate(){
        while(true){
            lock(m_synObject){
                if(events.Count > 0){
                    ThreadEvent e = events.Dequeue();
                    try{
                        switch(e.Key){
                            default:

                                break;
                        }
                    }catch(System.Exception ex){
                        UnityEngine.Debug.LogError(ex.Message);
                    }
                }
            }
        }
    }
}
