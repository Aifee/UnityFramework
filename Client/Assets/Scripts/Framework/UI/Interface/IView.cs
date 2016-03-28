using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public interface IView  {
    
    void RegisterObserver(string notificationName, IObserver observer);
    void RemoveObserver(string notificationName, object notifyContext);
    void NotifyObservers(INotification note);
    void RegisterPanel(IPanel panel);
    IPanel RetrievePanel(Type type);
    IPanel RemovePanel(Type type);
    void RemoveAllPanel();
    bool HasPanel(Type type);
    void HideOtherLayer(IPanel panel, IList<UILayer> relevantLayer);
}
