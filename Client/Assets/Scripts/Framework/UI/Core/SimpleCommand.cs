//----------------------------------------------
//            liuaf UnityFramework
// Copyright © 2015-2025 liuaf Entertainment
// Created by : Liu Aifei (329737941@qq.com)
//----------------------------------------------

using System.Collections.Generic;

public class SimpleCommand : Notifier, ICommand {
    protected View View { get; private set; }
    public virtual void Execute(string notificationName, Bundle bundle = null) {
        throw new System.NotImplementedException();
    }

    public void SetView(View view) {
        View = view;
    }


    public virtual IList<string> ListNotificationInterests() {
        throw new System.NotImplementedException();
    }
}
