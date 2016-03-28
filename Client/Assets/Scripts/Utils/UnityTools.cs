//----------------------------------------------
//            liuaf threeKingdoms Project
// Copyright © 2010-2015 threeKingdoms
// Created by : Liu Aifei (329737941@qq.com)
//--------------------------------------------
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// @Summary : 
/// @Author ： Liu Aifei
/// @Date : 2014.04.22
/// </summary>
public sealed class UnityTools
{

    #region 3d坐标 组件 对象脚本处理
    /// <summary>
    /// 在一个Vector3位置找到周围MaxDistance距离中的所有组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="position"></param>
    /// <param name="maxDistance"></param>
    /// <returns>Returns all components of type T in range.</returns>
    public static T[] FindObjectsOfType<T>(Vector3 position, float maxDistance) where T : Component
    {
        Transform[] all = (Transform[])GameObject.FindObjectsOfType(typeof(Transform));
        List<T> objects = new List<T>();
        foreach (Transform trans in all)
        {
            if (Vector3.Distance(position, trans.position) < maxDistance && trans.GetComponent<T>())
            {
                objects.Add(trans.GetComponent<T>());
            }
        }
        return objects.ToArray();
    }

    /// <summary>
    /// 在一个顶点上找到周围所有的组件（场景中存在的）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="position"></param>
    /// <returns>Returns the nearest component of type T.</returns>
    public static T FindObjectOfType<T>(Vector3 position) where T : Component
    {
        Transform[] all = (Transform[])GameObject.FindObjectsOfType(typeof(Transform));
        T closest = null;
        float distance = Mathf.Infinity;

        foreach (Transform trans in all)
        {
            Vector3 diff = trans.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = trans.GetComponent<T>();
                distance = curDistance;
            }
        }
        return closest;
    }
    /// <summary>
    /// 在一个顶点上找到周围maxDistance距离中的某一个组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="position"></param>
    /// <param name="maxDistance"></param>
    /// <returns></returns>
    public static T FindObjectOfType<T>(Vector3 position, float maxDistance) where T : Component
    {
        Transform[] all = (Transform[])GameObject.FindObjectsOfType(typeof(Transform));
        T closest = null;
        float distance = Mathf.Infinity;

        foreach (Transform trans in all)
        {
            Vector3 diff = trans.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < maxDistance && curDistance < distance)
            {
                closest = trans.GetComponent<T>();
                distance = curDistance;
            }
        }
        return closest;
    }
    /// <summary>
    /// 查找摸一个顶点范围内的所有同时包含T组件和K组件的对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="position"></param>
    /// <param name="maxDistance"></param>
    /// <returns>Return all components of type T in range, that have also component K.</returns>
    public static T[] FindObjectsOfType<T, K>(Vector3 position, float maxDistance) where T : Component where K : Component
    {
        Transform[] all = (Transform[])GameObject.FindObjectsOfType(typeof(Transform));
        List<T> objects = new List<T>();
        foreach (Transform trans in all)
        {
            if (Vector3.Distance(position, trans.position) < maxDistance && trans.GetComponent<T>() && trans.GetComponent<K>())
            {
                objects.Add(trans.GetComponent<T>());
            }
        }
        return objects.ToArray();
    }

    /// <summary>
    /// Finds Objects of type T in view.
    /// The GameObject needs a collider.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="trans"></param>
    /// <param name="maxDistance"></param>
    /// <param name="viewAngle"></param>
    /// <param name="height"></param>
    /// <returns>Returns all components of type T in view</returns>
    public static T[] FindObjectsOfType<T>(Transform trans, float maxDistance, float viewAngle, float height) where T : Component
    {
        Collider[] gos = Physics.OverlapSphere(trans.position, maxDistance);
        List<T> objects = new List<T>();
        foreach (Collider col in gos)
        {
            T comp = col.GetComponent<T>();
            if (comp != null)
            {
                if (CheckLineOfSight(trans, col.transform, viewAngle, height))
                {
                    objects.Add(comp);
                }
            }
        }
        return objects.ToArray();
    }

    /// <summary>
    /// Finds the nearest component of type T in range and view.
    /// </summary>
    /// <returns>
    /// The nearest component of type T.
    /// </returns>
    /// <param name='trans'>
    /// Transform of the agent.
    /// </param>
    /// <param name='maxDistance'>
    /// Max distance to the transform.
    /// </param>
    /// <param name='viewAngle'>
    /// View angle.
    /// </param>
    /// <param name='height'>
    /// Height of the agent.
    /// </param>
    public static T FindObjectOfType<T>(Transform trans, float maxDistance, float viewAngle, float height) where T : Component
    {
        Collider[] gos = Physics.OverlapSphere(trans.position, maxDistance);
        T closest = null;
        float distance = Mathf.Infinity;

        foreach (Collider col in gos)
        {
            T comp = col.GetComponent<T>();
            Vector3 diff = col.transform.position - trans.position;
            float curDistance = diff.sqrMagnitude;

            if (comp != null && curDistance < distance)
            {
                if (CheckLineOfSight(trans, col.transform, viewAngle, height))
                {
                    closest = comp;
                    distance = curDistance;
                }
            }
        }
        return closest;
    }

    /// <summary>
    /// Finds the components of type T with tag in range
    /// </summary>
    /// <returns>
    /// Components with tag
    /// </returns>
    /// <param name='position'>
    /// Reference Vector3 position
    /// </param>
    /// <param name='range'>
    /// Maximum distance from position
    /// </param>
    /// <param name='tag'>
    /// Tag of the gameObject
    /// </param>
    /// <typeparam name='T'>
    /// Component of type T to search for
    /// </typeparam>
    public static T[] FindObjectsWithTag<T>(Vector3 position, float range, string tag) where T : Component
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);
        List<T> components = new List<T>();

        for (int i = 0; i < gos.Length; i++)
        {
            T comp = gos[i].GetComponent<T>();
            if (comp && Vector3.Distance(position, gos[i].transform.position) < range)
            {
                components.Add(comp);
            }
        }
        return components.ToArray();
    }

    /// <summary>
    /// Finds the gameObjects with tag in range
    /// </summary>
    /// <returns>
    /// The gameObjects with tag in range
    /// </returns>
    /// <param name='position'>
    /// Reference Vector3 position.
    /// </param>
    /// <param name='range'>
    /// Maximum distance to position.
    /// </param>
    /// <param name='tag'>
    /// The tag of the gameObject.
    /// </param>
    public static GameObject[] FindGameObjectsWithTag(Vector3 position, float range, string tag)
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);
        List<GameObject> list = new List<GameObject>();

        for (int i = 0; i < gos.Length; i++)
        {
            if (Vector3.Distance(position, gos[i].transform.position) < range)
            {
                list.Add(gos[i]);
            }
        }
        return list.ToArray();
    }

    /// <summary>
    /// Get a random Vector3 point inside an area
    /// </summary>
    /// <returns>
    /// Returns a Vector3 point in range
    /// </returns>
    /// <param name='position'>
    /// Vector3 as a reference position.
    /// </param>
    /// <param name='range'>
    /// Area range.
    /// </param>
    public static Vector3 RandomPointInArea(Vector3 position, float range)
    {

        Vector3 random = new Vector3(position.x + UnityEngine.Random.Range(-range, range), position.y, position.z + UnityEngine.Random.Range(-range, range));
        RaycastHit hit;
        if (Physics.Raycast(random + Vector3.up * 500, Vector3.down, out hit))
        {
            random.y = (hit.point.y + 0.2f);
        }
        return random;
    }


    /// <summary>
    /// Get a random Vector3 point inside an area
    /// </summary>
    /// <returns>
    /// The point in area.
    /// </returns>
    /// <param name='position'>
    /// Position.
    /// </param>
    /// <param name='range'>
    /// Range.
    /// </param>
    /// <param name='mask'>
    /// Mask.
    /// </param>
    public static Vector3 RandomPointInArea(Vector3 position, float range, LayerMask mask)
    {
        Vector3 random = new Vector3(position.x + UnityEngine.Random.Range(-range, range), position.y, position.z + UnityEngine.Random.Range(-range, range));
        RaycastHit hit;
        if (Physics.Raycast(random + Vector3.up * 500, Vector3.down, out hit, Mathf.Infinity, mask))
        {
            random.y = (hit.point.y + 0.2f);
        }
        return random;
    }

    /// <summary>
    /// Get a random Quaternion
    /// </summary>
    /// <returns>
    /// Quaternion
    /// </returns>
    /// <param name='axis'>
    /// Scale vector
    /// </param>
    /// <param name='min'>
    /// Minimum euler angle
    /// </param>
    /// <param name='max'>
    /// Maximum euler angle
    /// </param>
    public static Quaternion RandomQuaternion(Vector3 axis, float min, float max)
    {
        Vector3 euler = new Vector3(UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max));
        euler.Scale(axis);
        return Quaternion.Euler(euler);
    }

    /// <summary>
    /// Instantiates a prefab at random Vector3 point in area.
    /// </summary>
    /// <returns>
    /// Instantiated GameObject
    /// </returns>
    /// <param name='prefab'>
    /// Prefab to instantiate
    /// </param>
    /// <param name='position'>
    /// Vector3 point as a reference point
    /// </param>
    /// <param name='rotation'>
    /// Rotation of the GameObject
    /// </param>
    /// <param name='range'>
    /// Maximum range from the position
    /// </param>
    public static GameObject InstantiateAtRandomPointInArea(GameObject prefab, Vector3 position, Quaternion rotation, float range)
    {
        return (GameObject)GameObject.Instantiate(prefab, RandomPointInArea(position, range), rotation);
    }

    /// <summary>
    /// Instantiates a prefab at random Vector3 point in area.
    /// </summary>
    /// <returns>
    /// Instantiated GameObject
    /// </returns>
    /// <param name='prefab'>
    /// Prefab to instantiate
    /// </param>
    /// <param name='position'>
    /// Vector3 point as a reference point
    /// </param>
    /// <param name='range'>
    /// Maximum range from the position
    /// </param>
    public static GameObject InstantiateAtRandomPointInArea(GameObject prefab, Vector3 position, float range)
    {
        return (GameObject)GameObject.Instantiate(prefab, RandomPointInArea(position, range), Quaternion.identity);
    }

    /// <summary>
    /// Instantiates a prefab at random Vector3 point in area.
    /// </summary>
    /// <returns>
    /// Instantiated GameObject
    /// </returns>
    /// <param name='prefab'>
    /// Prefab to instantiate
    /// </param>
    /// <param name='position'>
    /// Vector3 point as a reference point
    /// </param>
    /// <param name='range'>
    /// Maximum range from the position
    /// </param>
    /// <param name='lookAt'>
    /// Transform to look at.
    /// </param>
    public static GameObject InstantiateAtRandomPointInArea(GameObject prefab, Vector3 position, float range, Transform lookAt)
    {
        GameObject go = (GameObject)GameObject.Instantiate(prefab, RandomPointInArea(position, range), Quaternion.identity);
        go.transform.LookAt(lookAt);
        return go;
    }

    /// <summary>
    /// Adds the pefab as a child to the root transform
    /// </summary>
    /// <returns>
    /// The child gameObject
    /// </returns>
    /// <param name='root'>
    /// Root gameObject
    /// </param>
    /// <param name='prefab'>
    /// Prefab to add as child
    /// </param>
    public static GameObject AddChild(GameObject root, GameObject prefab)
    {
        GameObject go = (GameObject)GameObject.Instantiate(prefab);
        go.transform.parent = root.transform;
        go.transform.localPosition = Vector3.zero;
        return go;
    }

    /// <summary>
    /// Destroys the children of type T in the root gameObject
    /// </summary>
    /// <param name='root'>
    /// Root gameObject
    /// </param>
    /// <typeparam name='T'>
    /// Type of the component that should be destroyed.
    /// </typeparam>
    public static void DestroyChildren<T>(GameObject root) where T : Component
    {
        foreach (Component component in root.GetComponentsInChildren<T>())
        {
            GameObject.Destroy(component.gameObject);
        }
    }

    /// <summary>
    /// Checks if the target is in line of sight without view angle
    /// </summary>
    /// <returns>
    /// Returns true if there is no collider between agent and target
    /// </returns>
    /// <param name='agent'>
    /// Transform of the agent
    /// </param>
    /// <param name='target'>
    /// Transform of the target
    /// </param>
    public static bool CheckLineOfSight(Transform agent, Transform target)
    {
        RaycastHit hit;
        if (Physics.Linecast(agent.position, target.position, out hit))
        {
            if (hit.transform == target.transform)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if the target is in line of sight, ignores height
    /// </summary>
    /// <returns>
    /// Returns true if there is no collider between agent and target and target is in view angle
    /// </returns>
    /// <param name='agent'>
    /// Transform of the agent
    /// </param>
    /// <param name='target'>
    /// Transform of the target
    /// </param>
    /// <param name='viewAngle'>
    /// Maximum view angle
    /// </param>
    public static bool CheckLineOfSight(Transform agent, Transform target, float viewAngle)
    {
        float targetAngle = Vector3.Angle(target.position - agent.position, agent.forward);
        if (Mathf.Abs(targetAngle) < (viewAngle / 2))
        {
            RaycastHit hit;
            if (Physics.Linecast(agent.position, target.position, out hit))
            {
                if (hit.transform == target.transform)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if the target is in line of sight
    /// </summary>
    /// <returns>
    /// Returns true if there is no collider between agent and target and target is in view angle
    /// </returns>
    /// <param name='agent'>
    /// Transform of the agent
    /// </param>
    /// <param name='target'>
    /// Transform of the target
    /// </param>
    /// <param name='viewAngle'>
    /// Maximum view angle
    /// </param>
    /// <param name='height'>
    /// The height of the target
    /// </param>
    public static bool CheckLineOfSight(Transform agent, Transform target, float viewAngle, float height)
    {
        float targetAngle = Vector3.Angle(target.position - agent.position, agent.forward);
        if (Mathf.Abs(targetAngle) < (viewAngle / 2))
        {
            RaycastHit hit;
            if (Physics.Linecast(agent.position + Vector3.up * height, target.position + Vector3.up * height, out hit))
            {
                if (hit.transform == target.transform)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Checks the line of sight with max distance to target
    /// </summary>
    /// <returns>
    /// Returns true if there is no collider between agent and target, target is in view angle and inside distance
    /// </returns>
    /// <param name='agent'>
    /// Transform of the agent
    /// </param>
    /// <param name='target'>
    /// Transform of the target
    /// </param>
    /// <param name='viewAngle'>
    /// Maximum view angle
    /// </param>
    /// <param name='height'>
    /// The height of the target
    /// </param>
    /// <param name='maxDistance'>
    /// Maximum distance between agent and target
    /// </param>
    public static bool CheckLineOfSight(Transform agent, Transform target, float viewAngle, float height, float maxDistance)
    {
        if (Vector3.Distance(agent.position, target.position) > maxDistance)
        {
            return false;
        }

        float targetAngle = Vector3.Angle(target.position - agent.position, agent.forward);
        if (Mathf.Abs(targetAngle) < (viewAngle / 2))
        {
            RaycastHit hit;
            if (Physics.Linecast(agent.position + Vector3.up * height, target.position + Vector3.up * height, out hit))
            {
                if (hit.transform == target.transform)
                {
                    return true;
                }
            }
        }
        return false;
    }



    #endregion 





}

