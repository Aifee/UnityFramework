using UnityEngine;
using System.Collections;


public interface IEvent 
{
    string Name { get; set; }// 获取或设置事件的名称

    string Message { get; set; }// 获取或设置事件的简单描述

    EventTypes EventType { get; set; }// 获取或设置事件类型（枚举EventTypes）

    ListenerCollection Listeners { get; } // 获取响应者的集合

    bool PoolEvent { get; set; }// 获取或设置事件的简单描述

    // 方法

    void RaiseEvent(); // 通知响应者事件的发生

    void AbandonListener(int index); // 抛弃一个事件响应者，并把它从 Listeners 中移除。

    void AbandonListener(); // 抛弃所有的事件响应者
}
