using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 
/// </summary>
public class TimersManager :SingletonComponent<TimersManager>
{
    /// <summary>
    /// 全局实例
    /// </summary>
    private static TimersManager _Instance = null;

    public static bool IsPlaying { get; set; }

    /// <summary>
    /// 定时器字典
    /// </summary>
    private Dictionary<string, Timer> m_TimerList = new Dictionary<string, Timer>();

    /// <summary>
    ///   增加队列
    /// </summary>
    private Dictionary<string, Timer> m_AddTimerList = new Dictionary<string, Timer>();

    /// <summary>
    ///   销毁队列
    /// </summary>
    private List<string> m_DestroyTimerList = new List<string>();


    public delegate void TimerManagerHandler( string key , float time );

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// 全局实例
    /// </summary>
    /// -----------------------------------------------------------------------------
    public static TimersManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                if (_Instance == null)
                {
                    _Instance = FindObjectOfType(typeof(TimersManager)) as TimersManager;
                    TimersManager.IsPlaying = true;
                }
            }
            return _Instance;
        }
    }

    // Use this for initialization
    void Awake()
    {
        if (TimersManager.Instance != null && TimersManager.Instance != this)
        {
            UnityEngine.Object.Destroy(this);
            return;
        }
		
        //DontDestroyOnLoad(this.gameObject);
        _Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsPlaying)
            return;
        if (m_DestroyTimerList.Count > 0)
        {
            ///>从销毁队列中销毁指定内容
            foreach (string i in m_DestroyTimerList)
            {
                m_TimerList.Remove(i);
            }

            //清空
            m_DestroyTimerList.Clear();
        }

        if (m_AddTimerList.Count > 0)
        {
            ///>从增加队列中增加定时器
            foreach (KeyValuePair<string, Timer> i in m_AddTimerList)
            {
                if (i.Value == null)
                    continue;

                if (m_TimerList.ContainsKey(i.Key))
                {
                    m_TimerList[i.Key] = i.Value;
                }
                else
                {
                    m_TimerList.Add(i.Key, i.Value);
                }
            }

            //清空
            m_AddTimerList.Clear();
        }

        if (m_TimerList.Count > 0)
        {
            //响应定时器
            foreach (Timer timer in m_TimerList.Values)
            {
                if (timer == null)
                    return;

                timer.Run();
            }
        }
    }

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// 增加定时器
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    /// -----------------------------------------------------------------------------
    public bool AddTimer(string key, float duration, TimerManagerHandler handler)
    {
        return Internal_AddTimer(key, TIMER_MODE.NORMAL, duration, handler);
    }

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// 增加持续定时器
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    /// -----------------------------------------------------------------------------
    public bool AddTimerRepeat(string key, float duration, TimerManagerHandler handler)
    {
        return Internal_AddTimer(key, TIMER_MODE.REPEAT, duration, handler);
    }
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// 销毁指定定时器
    /// </summary>
    /// <param name="key">标识符</param>
    /// <returns></returns>
    /// -----------------------------------------------------------------------------
    public bool Destroy(string key)
    {
        if (!m_TimerList.ContainsKey(key))
            return false;

        if (!m_DestroyTimerList.Contains(key))
        {
            m_DestroyTimerList.Add(key);
        }

        return true;
    }

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// 增加定时器
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    /// -----------------------------------------------------------------------------
    private bool Internal_AddTimer(string key, TIMER_MODE mode, float duration, TimerManagerHandler handler)
    {
        if (string.IsNullOrEmpty(key))
            return false;

        if (duration < 0.0f)
            return false;

        Timer timer = new Timer(key, mode, Time.time, duration, handler, this);

        if (m_AddTimerList.ContainsKey(key))
        {
            m_AddTimerList[key] = timer;
        }
        else
        {
            m_AddTimerList.Add(key, timer);
        }

        return true;
    }

    public bool IsRunning(string key)
    {
        return m_TimerList.ContainsKey(key);
    }

    /// -----------------------------------------------------------------------------
    /// <summary>
    ///  定时器模式
    /// </summary>
    /// -----------------------------------------------------------------------------
    private enum TIMER_MODE
    {
        NORMAL,
        REPEAT,
    }

    private class Timer
    {
        /// <summary>
        ///   名称
        /// </summary>
        private string m_Name;

        /// <summary>
        ///   模式
        /// </summary>
        private TIMER_MODE m_Mode;

        /// <summary>
        ///   开始时间
        /// </summary>
        private float m_StartTime;

        /// <summary>
        ///   时长
        /// </summary>
        private float m_duration;

        /// <summary>
        ///   定时器委托
        /// </summary>
        private TimerManagerHandler m_TimerEvent;

        private TimersManager m_Manger;

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// 开始时间
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// -----------------------------------------------------------------------------
        public float StartTime
        {
            get
            {
                return m_StartTime;
            }
            set
            {
                m_StartTime = value;
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// 剩余时间
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// -----------------------------------------------------------------------------
        public float TimeLeft
        {
            get
            {
                return Mathf.Max(0.0f, m_duration - (Time.time - m_StartTime));
            }
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// -----------------------------------------------------------------------------
        public Timer(string name, TIMER_MODE mode, float startTime, float duration, TimerManagerHandler handler, TimersManager manager)
        {
            m_Name = name;
            m_Mode = mode;
            m_StartTime = startTime;
            m_duration = duration;
            m_TimerEvent = handler;
            m_Manger = manager;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// 运行事件
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// -----------------------------------------------------------------------------
        public void Run()
        {
            if (this.TimeLeft > 0.0f)
                return;

            if (this.m_TimerEvent != null)
            {
                this.m_TimerEvent(m_Name, m_duration);
            }

            if (m_Mode == TIMER_MODE.NORMAL)
            {
                m_Manger.Destroy(this.m_Name);
            }
            else
            {
                m_StartTime = Time.time;
            }
            return;
        }
    }

    public void Disponse()
    {
        m_AddTimerList.Clear();
        m_DestroyTimerList.Clear();
        m_TimerList.Clear();
    }

//    private static float loginTime;
//    /// <summary>
//    /// 游戏登录时间，在登录时要设置，从服务器获取时间
//    /// </summary>
//    /// <value>The login time.</value>
//    public static float LoginTime{
//        get{return loginTime;}
//        set{loginTime = value;}
//    }
//    /// <summary>
//    /// 游戏运行时间，区别于系统自带的运行时间，登录游戏时间+运行时间
//    /// </summary>
//    /// <value>The realtime since startup.</value>
//    public static long realtimeSinceStartup{
//        get{ return UnityEngine.Time.realtimeSinceStartup + loginTime; }
//    }

}

