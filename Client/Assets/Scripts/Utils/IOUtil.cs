using UnityEngine;
using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;

public sealed class IOUtil
{
	/// <summary>
	/// 获取一个序列化脚本中是否存在某个属性
	/// </summary>
	/// <param name="info"></param>
	/// <param name="name"></param>
	/// <returns></returns>
	public static bool ContainsValue(SerializationInfo info, string name)
	{
		SerializationInfoEnumerator e = info.GetEnumerator();
		while (e.MoveNext())
		{
			if (e.Name == name)
			{
				return true;
			}
		}
		return false;
	}
	
	/// <summary>
	/// 克隆一个脚本，并设置其值和被克隆脚本一样
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="t"></param>
	/// <returns></returns>
	public static T GetReflection<T>(T t) where T : new()
	{
		Type type = typeof(T); //获取MyClass的类型信息
		T temp = new T();
		
		FieldInfo[] fieldArray = type.GetFields();
		foreach (FieldInfo file in fieldArray)
		{
			object obj = t.GetType().GetField(file.Name).GetValue(t);
			temp.GetType().GetField(file.Name).SetValue(temp, obj);
		}
		return temp;
	}

}
