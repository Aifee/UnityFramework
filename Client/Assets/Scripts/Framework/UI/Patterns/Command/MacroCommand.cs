using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MacroCommand : Notifier, ICommand, INotifier {
    private IList<Type> m_subCommands;
    public MacroCommand() {
        m_subCommands = new List<Type>();
        InitializeMacroCommand();
    }
    protected virtual void InitializeMacroCommand() {
    }
    protected void AddSubCommand(Type commandType) {
        m_subCommands.Add(commandType);
    }
    public virtual void Execute(INotification notification) {
        while (m_subCommands.Count > 0) {
            Type commandType = m_subCommands[0];
            object commandInstance = Activator.CreateInstance(commandType);
            
            if (commandInstance is ICommand) {
                ((ICommand)commandInstance).Execute(notification);
            }
            
            m_subCommands.RemoveAt(0);
        }
    }
}

