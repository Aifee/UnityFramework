//----------------------------------------------
//            liuaf UnityFramework
// Copyright © 2015-2025 liuaf Entertainment
// Created by : Liu Aifei (329737941@qq.com)
//----------------------------------------------

using System.Collections.Generic;

public interface ICommand {
    void Execute(string notificationName, Bundle bundle = null);
    IList<string> ListNotificationInterests();
    void SetView(View view);
}
