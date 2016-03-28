using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// @Summary : 游戏管理类静态数据
/// @Author : liuaf
/// @Date : 2014.04.21
/// </summary>
public class GameManager
{
    #region CameraRoot

    static private Transform cameraRoot;
    static public Transform CameraRoot{
        get{
            if(cameraRoot == null){
                cameraRoot = GameObject.FindGameObjectWithTag("CameraRoot").transform;
                Debug.Log(cameraRoot);
                if(cameraRoot == null){
                    Debug.Log(cameraRoot);
                    GameObject go = ResourcesManager.Instance.LoadOtherPrefab(COMMDEF.MAINCAMERROOT);
                    cameraRoot = GameObject.Instantiate(go).transform;
                    Debug.Log(cameraRoot);
                }
            }
            return cameraRoot;
        }
    }

    #endregion

    //====================================================================

    #region MainCamera
    static private Camera camera;
    static public Camera MainCamera{
        get{
            if(camera == null){
                camera = CameraRoot.FindChild("Main Camera").GetComponent<Camera>();// GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            }
            return camera;
        }
    }
    #endregion

    //====================================================================

    #region Player

    static private Player player;
    static public Player Player{
        get{
            if(player == null)
            {
                player = new Player();
            }
            return player;
        }
    }

    #endregion

}