using System;
using UnityEngine;
using System.Collections.Generic;

public interface IUIManager : INotifier {
    
    #region Command
    void RegisterCommand(string notificationName, Type commandType);
    void RemoveCommand(string notificationName);
    bool HasCommand(string notificationName);
    #endregion
    
    #region Observer
    
    void NotifyObservers(INotification note);
    
    #endregion
    
    #region Panel
    
    IPanel Show<T>() where T : IPanel;
    IPanel Show(Type type);
    void Hide<T>() where T : IPanel;
    void Hide(Type type);
    IPanel RetrievePanel<T>() where T : IPanel;
    IPanel RetrievePanel(Type type);
    bool HasPanel<T>() where T : IPanel;
    bool HasPanel(Type type);
    void DestroyPanel<T>() where T : IPanel;
    void DestroyPanel(Type type);
    void DestroyAll();
    
    #endregion
    
}
