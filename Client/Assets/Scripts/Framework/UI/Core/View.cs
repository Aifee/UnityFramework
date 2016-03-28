using System;
using System.Collections.Generic;
public class View : IView {
    protected IDictionary<Type, IPanel> m_PanelMap;
    protected IDictionary<string, IList<IObserver>> m_ObserverMap;
    protected static volatile IView m_instance;
    protected readonly object m_SyncRoot = new object();
    protected static readonly object m_StaticSyncRoot = new object();
    
    #region Instance
    
    public static IView Instance {
        get {
            if (m_instance == null) {
                lock (m_StaticSyncRoot) {
                    if (m_instance == null) m_instance = new View();
                }
            }
            return m_instance;
        }
    }
    
    #endregion
    
    protected View() {
        m_PanelMap = new Dictionary<Type, IPanel>();
        m_ObserverMap = new Dictionary<string, IList<IObserver>>();
        InitializeView();
    }
    protected virtual void InitializeView() {
    }
    
    public void RegisterObserver(string notificationName, IObserver observer) {
        lock (m_SyncRoot) {
            if (!m_ObserverMap.ContainsKey(notificationName)) {
                m_ObserverMap[notificationName] = new List<IObserver>();
            }
            m_ObserverMap[notificationName].Add(observer);
        }
    }
    
    public void RemoveObserver(string notificationName, object notifyContext) {
        lock (m_SyncRoot) {
            if (m_ObserverMap.ContainsKey(notificationName)) {
                IList<IObserver> observers = m_ObserverMap[notificationName];
                for (int i = 0; i < observers.Count; i++) {
                    if (observers[i].CompareNotifyContext(notifyContext)) {
                        observers.RemoveAt(i);
                        break;
                    }
                }
                if (observers.Count == 0) {
                    m_ObserverMap.Remove(notificationName);
                }
            }
        }
    }
    
    public void NotifyObservers(INotification note) {
        IList<IObserver> observers = null;
        
        lock (m_SyncRoot) {
            if (m_ObserverMap.ContainsKey(note.Name)) {
                IList<IObserver> observers_ref = m_ObserverMap[note.Name];
                observers = new List<IObserver>(observers_ref);
            }
        }
        if (observers != null) {        
            for (int i = 0; i < observers.Count; i++) {
                IObserver observer = observers[i];
                observer.NotifyObserver(note);
            }
        }
    }
    
    public void RegisterPanel(IPanel panel) {
        lock (m_SyncRoot) {
            if (m_PanelMap.ContainsKey(panel.GetType())) return;
            m_PanelMap[panel.GetType()] = panel;
            IList<string> interests = panel.ListNotificationInterests();
            if (interests.Count > 0) {
                IObserver observer = new Observer("handleNotification", panel);
                for (int i = 0; i < interests.Count; i++) {
                    RegisterObserver(interests[i].ToString(), observer);
                }
            }
        }
    }
    
    public IPanel RetrievePanel(Type type) {
        lock (m_SyncRoot) {
            if (!m_PanelMap.ContainsKey(type)) return null;
            return m_PanelMap[type];
        }
    }
    
    public IPanel RemovePanel(Type type) {
        IPanel panel = null;
        lock (m_SyncRoot) {
            if (!m_PanelMap.ContainsKey(type)) return null;
            panel = (IPanel)m_PanelMap[type];
            IList<string> interests = panel.ListNotificationInterests();
            
            for (int i = 0; i < interests.Count; i++) {
                RemoveObserver(interests[i], panel);
                m_PanelMap.Remove(type);
            }
            if (panel != null) {
                panel.Destroy();
            }
            return panel;
        }
    }
    public void RemoveAllPanel() {
        lock (m_SyncRoot) {
            if (m_PanelMap.Count <= 0) return;
            foreach (IPanel panel in m_PanelMap.Values) {
                IList<string> interests = panel.ListNotificationInterests();
                if (interests.Count > 0) {
                    for (int i = 0, iMax = interests.Count; i < iMax; i++) {
                        RemoveObserver(interests[i], panel);
                    }
                }
                panel.Destroy();
            }
            m_PanelMap.Clear();
        }
    }
    public bool HasPanel(Type type) {
        lock (m_SyncRoot) {
            return m_PanelMap.ContainsKey(type);
        }
    }
    
    public void HideOtherLayer(IPanel panel, IList<UILayer> relevantLayer) {
        lock (m_SyncRoot) {
            foreach (IPanel p in m_PanelMap.Values) {
                if (p == panel)
                    continue;
                if (!relevantLayer.Contains(p.Layer))
                    continue;
                p.Hide();
            }
        }
    }
}
