using UnityEngine;
using System.Collections;

public class Notification : INotification {
    
    private string m_Name;
    private object m_Body;
    private string m_Type;
    public Notification(string name)
    : this(name, null, null) { }
    public Notification(string name, object body)
    : this(name, body, null) { }
    public Notification(string name, object body, string type) {
        m_Name = name;
        m_Body = body;
        m_Type = type;
    }
    public override string ToString() {
        string msg = "Notification Name: " + Name;
        msg += "\nBody:" + ((Body == null) ? "null" : Body.ToString());
        msg += "\nType:" + ((Type == null) ? "null" : Type);
        return msg;
    }
    public string Name {
        get { return m_Name; }
    }
    
    public object Body {
        get {
            return m_Body;
        }
        set {
            m_Body = value;
        }
    }
    
    public string Type {
        get {
            return m_Type;
        }
        set {
            m_Type = value;
        }
    }
}

