//----------------------------------------------
//            liuaf UnityFramework
// Copyright © 2015-2025 liuaf Entertainment
// Created by : Liu Aifei (329737941@qq.com)
//----------------------------------------------

public class Notifier : INotifier {
    
    public void SendNotification(string notificationName, Bundle bundle = null) {
        uiManager.SendNotification(notificationName, bundle);
    }
    private UIManager uiManager {
        get {
            if (m_Manager == null)
                m_Manager = UIManager.Instance;
            return m_Manager;
        }
    }
    private UIManager m_Manager;
}