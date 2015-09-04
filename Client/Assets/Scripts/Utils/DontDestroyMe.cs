using UnityEngine;
using System.Collections;

/// <summary>
/// @Summary : 切换场景不销毁对象脚本处理
/// @Auther : liuaifei
/// @Date : 2014.04.22
/// </summary>
/// 
public class DontDestroyMe : MonoBehaviour 
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
