using System;
using System.Collections.Generic;

public class Controller : IController  {
    protected IView m_View;
    protected IDictionary<string, Type> m_CommandMap;
    protected static volatile IController m_instance;
    protected readonly object m_SyncRoot = new object();
    protected static readonly object m_StaticSyncRoot = new object();
    
    #region Instance
    
    public static IController Instance {
        get {
            if (m_instance == null) {
                lock (m_StaticSyncRoot) {
                    if (m_instance == null) m_instance = new Controller();
                }
            }
            
            return m_instance;
        }
    }
    
    #endregion
    
    protected Controller() {
        m_CommandMap = new Dictionary<string, Type>();
        InitializeController();
    }
    protected virtual void InitializeController() {
        m_View = View.Instance;
    }
    
    public void RegisterCommand(string notificationName, System.Type commandType) {
        lock (m_SyncRoot) {
            if (!m_CommandMap.ContainsKey(notificationName)) {
                m_View.RegisterObserver(notificationName, new Observer("executeCommand", this));
            }
            
            m_CommandMap[notificationName] = commandType;
        }
    }
    
    public void ExecuteCommand(INotification notification) {
        Type commandType = null;
        lock (m_SyncRoot) {
            if (!m_CommandMap.ContainsKey(notification.Name)) return;
            commandType = m_CommandMap[notification.Name];
        }
        
        object commandInstance = Activator.CreateInstance(commandType);
        
        if (commandInstance is ICommand) {
            ((ICommand)commandInstance).Execute(notification);
        }
    }
    
    public void RemoveCommand(string notificationName) {
        lock (m_SyncRoot) {
            if (m_CommandMap.ContainsKey(notificationName)) {
                m_View.RemoveObserver(notificationName, this);
                m_CommandMap.Remove(notificationName);
            }
        }
    }
    
    public bool HasCommand(string notificationName) {
        lock (m_SyncRoot) {
            return m_CommandMap.ContainsKey(notificationName);
        }
    }
}
