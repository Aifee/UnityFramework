using System;
using UnityEngine;
using System.Collections.Generic;

public interface IPanel  {
    
    UILayer Layer { get; }
    string PanelName { get; }
    GameObject GameObject { get; }
    Transform Transform { get; }
    IList<string> ListNotificationInterests();
    void HandleNotification(INotification notification);
    void Activation(string panelName,GameObject go,UILayer layer);
    void Show();
    void Hide();
    void Destroy();
}

