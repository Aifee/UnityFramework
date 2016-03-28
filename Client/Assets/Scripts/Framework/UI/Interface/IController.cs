using System;

public interface IController  {
    void RegisterCommand(string notificationName, Type commandType);
    void ExecuteCommand(INotification notification);
    void RemoveCommand(string notificationName);
    bool HasCommand(string notificationName);
    
}

