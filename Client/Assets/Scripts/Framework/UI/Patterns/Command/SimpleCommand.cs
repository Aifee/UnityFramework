using UnityEngine;
using System.Collections;

public class SimpleCommand : Notifier, ICommand, INotifier {
    
    public virtual void Execute(INotification notification) {
    }
}

