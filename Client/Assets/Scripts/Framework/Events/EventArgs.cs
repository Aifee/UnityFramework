using UnityEngine;
using System.Collections;

public class EventArgs  {

    public EventArgs()
    {

    }
    public EventArgs(Event e)
    {
        ev = e;
        sender = null;
    }
    public EventArgs(Event e, object value)
    {
        ev = e;
        sender = value;
    }
    private Event ev;
    public Event Event { get { return ev; } } // 获取传递这个参数的事件

    private object sender;
    public object Sender { get { return sender; } } // 获取事件源
}
