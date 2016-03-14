using UnityEngine;
using System.Collections;

public class LiuafEvent : IEvent {

    protected string name;
    protected string message;
    protected EventTypes eventType;
    protected ListenerCollection listeners;
    protected bool poolEvent;

    string IEvent.Name { get { return name; } set { name = value; } }
    string IEvent.Message { get { return message; } set { message = value; } }
    EventTypes IEvent.EventType { get { return eventType; } set { eventType = value; } }
    ListenerCollection IEvent.Listeners { get { return listeners; } }
    bool IEvent.PoolEvent { get { return poolEvent; } set { poolEvent = value; } }

    void IEvent.RaiseEvent()
    {
        
    }

    void IEvent.AbandonListener(int index)
    {
        
    }

    void IEvent.AbandonListener()
    {
        
    }
}
