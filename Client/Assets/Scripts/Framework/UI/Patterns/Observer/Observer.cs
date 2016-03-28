using System;
using System.Reflection;

public class Observer : IObserver {
    private string m_NotifyMethod;
    private object m_NotifyContext;
    protected readonly object m_SyncRoot = new object();
    
    public Observer(string notifyMethod, object notifyContext) {
        m_NotifyMethod = notifyMethod;
        m_NotifyContext = notifyContext;
    }
    public virtual void NotifyObserver(INotification notification) {
        object context;
        string method;
        
        lock (m_SyncRoot) {
            context = NotifyContext;
            method = NotifyMethod;
        }
        
        Type t = context.GetType();
        BindingFlags f = BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase;
        MethodInfo mi = t.GetMethod(method, f);
        mi.Invoke(context, new object[] { notification });
    }
    public virtual bool CompareNotifyContext(object obj) {
        lock (m_SyncRoot) {
            return NotifyContext.Equals(obj);
        }
    }
    public virtual string NotifyMethod {
        private get {
            return m_NotifyMethod;
        }
        set {
            m_NotifyMethod = value;
        }
    }
    public virtual object NotifyContext {
        private get {
            return m_NotifyContext;
        }
        set {
            m_NotifyContext = value;
        }
    }
}

