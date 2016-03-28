using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// @Summary : 资源加载管理
/// </summary>
public class ResourcesManager : SingletonComponent<ResourcesManager>
{
    public delegate void LoadSceneHandler();

    private const string CONFIG_PATH = "Configs/";
    private const string UI_PATH = "UI/";
    private const string OTHER_PATH = "Other/";
    private const string MATERIAL_PATH = "Materials/";
    private const string BUILDING_PATH = "Buildings/";

    /// <summary>
    /// 主动回收垃圾
    /// </summary>
    public void GCClearCache()
    {
        System.GC.Collect();
    }
    /// <summary>
    /// 加载UI预制件
    /// </summary>
    /// <param name="_name"></param>
    /// <returns></returns>
    public GameObject LoadUIPrefab(string uiName)
    {
        GameObject Obj = Resources.Load(UI_PATH + uiName, typeof(GameObject)) as GameObject;
        if (Obj == null)
        {
            Debug.LogError("Load UIPanel: " + uiName + " is not find..");
        }
        return Obj;
    }
    /// <summary>
    /// 加载XML数据文件
    /// </summary>
    /// <param name="name">文件名</param>
    public string LoadConfig(string xmlName)
    {
        TextAsset textAsset = Resources.Load(CONFIG_PATH + xmlName, typeof(TextAsset)) as TextAsset;

        if (textAsset == null)
        {
            Debug.LogWarning("Load TextAsset: " + xmlName + " is not find..");
            return null;
        }

        return textAsset.text;
    }
    public GameObject LoadBuilding(string buildingName){
        GameObject Obj = Resources.Load(BUILDING_PATH + buildingName, typeof(GameObject)) as GameObject;
        if (Obj == null)
        {
            Debug.LogError("Load UIPanel: " + buildingName + " is not find..");
        }
        return Obj;
    }

    public void LoadScene(string name, LoadSceneHandler handler)
    {
        isUpdate = true;
        StartCoroutine(LoadScene_Internal(name, handler));
    }
    private IEnumerator LoadScene_Internal(string name, LoadSceneHandler handler)
    {
        AsyncOperation asyn = Application.LoadLevelAsync(name);
        _loadingProgress = asyn.progress;
        yield return asyn;
        //UIManager.Instance.HideAllView();

        if (handler != null)
        {
            isUpdate = false;
            handler();
        }
    }
    private float _loadingProgress;
    private bool isUpdate;
    protected void Update()
    {
        if (isUpdate)
        {
            //UIManager.Instance.executeCommand(LoadingViewControl.SETLOADINGVALLUECOMMAND, _loadingProgress);
        }
    }
    /// <summary>
    /// 加载制定路径的模型
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject LoadPerfab(string name)
    {
        GameObject go = Resources.Load(name) as GameObject;
        if (go == null)
        {
            Debug.LogError(name);
        }
        return go == null ? null : go;
    }
    /// <summary>
    /// 加载其他预制件
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject LoadOtherPrefab(string name)
    {
        GameObject go = Resources.Load(OTHER_PATH + name) as GameObject;
        return go;
    }
    public Material LoadMaterial(string materialName){
        Material material = Resources.Load<Material>(MATERIAL_PATH + materialName);
        if(material == null)
            Debug.LogErrorFormat("the load materila :{0} error!",materialName);
        return material;
    }
}
