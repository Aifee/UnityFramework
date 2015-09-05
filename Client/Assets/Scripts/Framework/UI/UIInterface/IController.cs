//----------------------------------------------
//            liuaf UnityFramework
// Copyright © 2015-2025 liuaf Entertainment
// Created by : Liu Aifei (329737941@qq.com)
//----------------------------------------------

public interface IController {
    void RegisterCommand(string notificationName, ICommand commandType);
    void ExecuteCommand(string notificationName, Bundle bunble = null);
    void RemoveCommand(string notificationName);
    bool HasCommand(string notificationName);
}
