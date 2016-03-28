using UnityEngine;
using System.Collections;

public class Notifier : INotifier {
    
    protected IUIManager Manager {
        get { return manager; }
    }
    private IUIManager manager = UIManager.Instance;
    public void SendNotification(string notificationName) {
        manager.SendNotification(notificationName);
    }
    
    public void SendNotification(string notificationName, object body) {
        manager.SendNotification(notificationName, body);
    }
    
    public void SendNotification(string notificationName, object body, string type) {
        manager.SendNotification(notificationName, body, type);
    }
}

