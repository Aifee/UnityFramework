using UnityEngine;
using System.Collections;

public interface IEventPool 
{

	 // 属性

     ArrayList Events { get; }// 获取池中所有的事件的集合

     ListenerCollection Listners { get; }// 获取池中所有的响应者的集合

 

     // 方法

     void AddEvent( Event obj ,bool copyListners ); // 添加一个事件并把它作为广播式事件

     void RemoveEventAt( int index ); // 将一个事件从列表中移除

     void RemoveEvent( Event listener ); // 将一个事件从列表中移除

     void Broadcast( Event e ); // 向列表中的所有响应者广播指定事件（可以是非池中的事件）

     void BroadcastItemAt( int index ); // 向列表中的所有响应者广播池中的指定事件
}
