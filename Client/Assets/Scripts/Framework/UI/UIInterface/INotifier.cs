//----------------------------------------------
//            liuaf UnityFramework
// Copyright © 2015-2025 liuaf Entertainment
// Created by : Liu Aifei (329737941@qq.com)
//----------------------------------------------

public interface INotifier {
    void SendNotification(string notificationName, Bundle bundle = null);
}
