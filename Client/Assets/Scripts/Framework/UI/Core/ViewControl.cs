//----------------------------------------------
//            liuaf UnityFramework
// Copyright © 2015-2025 liuaf Entertainment
// Created by : Liu Aifei (329737941@qq.com)
//----------------------------------------------

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public sealed class ViewControl : Singleton<ViewControl>, IController
{
    protected readonly object syncRoot = new object();
    protected IDictionary<string, ICommand> commandMap;

    public ViewControl() {
        commandMap = new Dictionary<string, ICommand>();
    }

    public void RegisterCommand(string notificationName, ICommand commandType) {
        lock (syncRoot) {
            if (!commandMap.ContainsKey(notificationName)) {
                commandMap.Add(notificationName, commandType);
            } else {
                Debug.LogError(string.Format("Warning! Registered by {0} already exists, please check whether the change", notificationName));
                commandMap[notificationName] = commandType;
            }
        }
    }

    public void ExecuteCommand(string notificationName, Bundle bunble = null) {
        if (commandMap.ContainsKey(notificationName)) {
            ICommand command = commandMap[notificationName];
            command.Execute(notificationName, bunble);
        } else {
            Debug.LogWarning(string.Format("The implementation of the {0} is not registered, please check after the implementation of", notificationName));
        }
    }

    public void RemoveCommand(string notificationName) {
        if (commandMap.ContainsKey(notificationName)) {
            commandMap.Remove(notificationName);
        }
    }

    public bool HasCommand(string notificationName) {
        return commandMap.ContainsKey(notificationName);
    }




    
}