using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.IO;
using System;
using System.Text;
using System.Security.Cryptography;
using System.Reflection;

/// <summary>
/// @Summary : 工具类包含通用函数中使用RPG框架。
/// @Author ： Liu Aifei
/// @Date : 2014.04.22
/// </summary>
public static class UnityTools
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


    /// <summary>
    /// 获取一个序列化脚本中是否存在某个属性
    /// </summary>
    /// <param name="info"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static bool ContainsValue(this SerializationInfo info, string name)
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

    #endregion 

    //========================================================================================================================

    #region Coroutine tools

    /// <summary>
    /// Starts the coroutine in a non MonoBehaviour class
    /// </summary>
    /// <returns>
    /// The coroutine.
    /// </returns>
    /// <param name='routine'>
    /// Routine.
    /// </param>
    public static Coroutine StartCoroutine(IEnumerator routine)
    {
        return StartCoroutine(routine, "Coroutine", false);
    }

    public static Coroutine StartCoroutine(IEnumerator routine, string routineName)
    {
        return StartCoroutine(routine, "Coroutine-" + routineName, false);
    }

    public static Coroutine StartCoroutine(IEnumerator routine, string routineName, bool dontDestroyOnLoad)
    {
        GameObject routineHandlerObject = new GameObject(routineName);
        CoroutineInstance routineHandler = routineHandlerObject.AddComponent<CoroutineInstance>();
        if (dontDestroyOnLoad)
        {
            GameObject.DontDestroyOnLoad(routineHandlerObject.gameObject);
        }
        return routineHandler.ProcessWork(routine);
    }

    #endregion 

    //========================================================================================================================

    #region path tools

    private static string _projectPath;
    /// <summary>
    /// 获取项目绝对路径
    /// </summary>
    public static string FullProjectPath
    {
        get
        {
            if (_projectPath == null || _projectPath.Equals(""))
            {
                _projectPath = Application.dataPath;
                _projectPath = _projectPath.Substring(0, _projectPath.LastIndexOf("Assets"));
            }
            return _projectPath;
        }
    }
    /// <summary>
    /// 检测制定文件路径是否存在
    /// </summary>
    /// <param name="filePath"> 相对路径("Assets" 目录) </param>
    /// <returns></returns>
    public static bool RelativeFileExist(string filePath)
    {
        filePath = FullProjectPath + "/" + filePath;
        bool exist = File.Exists(filePath);
        return exist;
    }
    /// <summary>
    /// 如果存在的路径(路径应该开始与“Assets”)
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static bool RelativePathExist(string path)
    {
        path = FullProjectPath + "/" + path;
        bool exist = Directory.Exists(path);
        return exist;
    }

    #endregion 

    //========================================================================================================================

    #region Formula Tools

    /// <summary>
    /// 将一个int只转换成英制字符串
    /// </summary>
    /// <param name="arg"></param>
    /// <returns></returns>
    public static string BritishSystem(int arg)
    {
        string british = arg.ToString();
        int i = british.Length;
        while (true)
        {
            i -= 3;
            if (i <= 0)
            {
                break;
            }
            british = british.Insert(i, ",");
        }
        return british;
    }

    #endregion

    //========================================================================================================================

    #region MD5

    /// <summary>
    /// MD5 String
    /// </summary>
    /// <returns>
    public static string MD5(string str)
    {
        byte[] b = Encoding.Default.GetBytes(str);
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] c = md5.ComputeHash(b);
        return System.BitConverter.ToString(c).Replace("-", "");
    }

    #endregion 

}

public class CoroutineInstance : MonoBehaviour
{
    public Coroutine ProcessWork(IEnumerator routine)
    {
        return StartCoroutine(DestroyWhenComplete(routine));
    }

    public IEnumerator DestroyWhenComplete(IEnumerator routine)
    {
        yield return StartCoroutine(routine);
        Destroy(gameObject);
    }
}