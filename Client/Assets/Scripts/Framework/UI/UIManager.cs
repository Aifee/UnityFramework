//----------------------------------------------
//            liuaf UnityFramework
// Copyright © 2015-2025 liuaf Entertainment
// Created by : Liu Aifei (329737941@qq.com)
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum UILayer {
    Default = 0,
    Secondary = 1,
    Pupop = 2,
    Guide = 3,
    TipAndWraning = 4,
}
public delegate void CallbackHandler();
/// <summary>
/// @Summary : UI view management, mainly responsible for the display, hide, and inform the other panel message processing
/// @Date : 2015.8.3
/// </summary>
public class UIManager : Singleton<UIManager>, INotifier
{
    #region vars
    private static readonly int UILAYER_SPACING = 50;
    public Dictionary<GameObject, View> m_UIViewDic = new Dictionary<GameObject, View>();
    private Dictionary<UILayer, Transform> m_LayerPanelDic = new Dictionary<UILayer, Transform>();
    public static Camera UICamera { get; private set; }

    public static UIRoot UiRoot { get; private set; }

    #endregion 

    public UIManager() {
        UICamera = GameObject.FindWithTag("UICamera").GetComponent<Camera>();
        UiRoot = GameObject.FindWithTag("UIRoot").GetComponent<UIRoot>();
        AutoWidth();
        
        InitChildPanel();
    }
    private void AutoWidth() {
        UiRoot.scalingStyle = global::UIRoot.Scaling.ConstrainedOnMobiles;
        UiRoot.manualWidth = Screen.width;
        UiRoot.fitWidth = true;
        UiRoot.fitHeight = false;

        UICamera.orthographicSize = 640f / (float)Screen.width;
    }
    private void InitChildPanel() {
        if (m_LayerPanelDic == null)
            m_LayerPanelDic = new Dictionary<UILayer, Transform>();
        UIPanel[] panels = UiRoot.GetComponentsInChildren<UIPanel>();
        if (panels.Length > 0) {
            for (int i = 0; i < panels.Length; i++) {
                if (panels[i].transform == UIManager.UiRoot.transform)
                    continue;
                GameObject.Destroy(panels[i].gameObject);
            }
        }
        for (int i = 0; i < 5; i++) {
            GameObject panelGo = new GameObject();
            TransformUtil.Reset(panelGo.transform, UiRoot.transform);
            UIPanel panel = panelGo.AddComponent<UIPanel>();
            UILayer uiLayer = (UILayer)System.Enum.Parse(typeof(UILayer), i.ToString());
            panelGo.name = uiLayer.ToString();
            panel.depth = i * UILAYER_SPACING;
            panel.gameObject.layer = LayerMask.NameToLayer("UI");
            m_LayerPanelDic.Add(uiLayer, panel.transform);
        }
        
    }
    public View ShowView<T>(string name) where T : View {
        T view = null;
        foreach (GameObject prefab in m_UIViewDic.Keys) {
            if (prefab.name != name)
                continue;

            if (m_UIViewDic[prefab] is T) {
                view = m_UIViewDic[prefab] as T;
            }
        }

        if (view == null) {
            view = CreateView<T>(name);
        }

        if (view.Layer == UILayer.Guide) {
            HideLayerView(view);
        } else if (view.Layer == UILayer.Default) {
            HideOtherView(view);
        }
        view.Showing();
        return view;
    }
    public void HideView<T>(string name) where T : View {
        foreach (GameObject prefab in m_UIViewDic.Keys) {
            if (prefab.name != name)
                continue;

            if (m_UIViewDic[prefab] is T) {
                m_UIViewDic[prefab].Hiding();
            }
        }
    }

    private T CreateView<T>(string prefabId) where T : View {
        UIAttribute classAttribute = (UIAttribute)System.Attribute.GetCustomAttribute(typeof(T), typeof(UIAttribute));
        GameObject prefab = InstantiatePrefab(prefabId, classAttribute.Layer);
        View view = null;
        if (prefab != null) {
            view = System.Activator.CreateInstance(typeof(T)) as View;
            if (classAttribute.CommandType != null) {
                ICommand command = System.Activator.CreateInstance(classAttribute.CommandType) as ICommand;
                command.SetView(view);
                IList<string> notifications = command.ListNotificationInterests();
                if (notifications.Count > 0) {
                    foreach (string notification in notifications) {
                        ViewControl.Instance.RegisterCommand(notification, command);
                    }
                }
            }
            view.Activation(prefab);
            view.Layer = classAttribute.Layer;
            m_UIViewDic.Add(prefab, view);
        } else {
            Debug.LogError("Error loading UIPrefab");
        }
        return view as T;
    }

    private GameObject InstantiatePrefab(string prefabId,UILayer layer) {
        GameObject prefab = null;
        string prefabFullPath = "Prefabs/UI/" + prefabId;
        prefab = Resources.Load(prefabFullPath) as GameObject;//ResourcesManager.Instance.LoadUIPrefab(m_PrefabId);
        if (prefab != null) {
            prefab = GameObject.Instantiate(prefab) as GameObject;
            prefab.name = prefabId;

            Camera uiCamera = UIManager.UICamera;
            if (uiCamera == null) {
                Debug.LogError("UICamera is not find");
                return null;
            }
            
            prefab.transform.parent = m_LayerPanelDic.ContainsKey(layer) ? m_LayerPanelDic[layer] : UIManager.UiRoot.transform;
            prefab.transform.localScale = new Vector3(1, 1, 1);
            prefab.transform.localPosition = new Vector3(0, 0, Mathf.Clamp(prefab.transform.localPosition.z, -2f, 2f));
            if (prefab.GetComponent<ViewRoot>() == null) {
                prefab.AddComponent<ViewRoot>();
            }
        }
        return prefab;
    }

    public View GetView(GameObject prefab) {
        return m_UIViewDic.ContainsKey(prefab) ? m_UIViewDic[prefab] : null;
    }
    public View GetView(string prefabName) {
        foreach (KeyValuePair<GameObject, View> kvp in m_UIViewDic) {
            if (kvp.Key.name == prefabName) {
                return kvp.Value;
            }
        }
        return null;
    }
    protected void HideLayerView(View view){
        if (m_UIViewDic.Count == 0)
            return;
        foreach (View v in m_UIViewDic.Values) {
            if (v == view || v.Layer != view.Layer)
                continue;
            v.Hiding();
        }
    }
    public void HideOtherView(View view) {
        if (m_UIViewDic.Count == 0)
            return;

        foreach (View v in m_UIViewDic.Values) {
            if (v == view || v.Layer == UILayer.Guide || v.Layer == UILayer.TipAndWraning)
                continue;
            v.Hiding();
        }
    }

    public void HideAllView() {
        if (m_UIViewDic.Count == 0)
            return;

        foreach (View view in m_UIViewDic.Values) {
            if (!view.IsDefalut)
                view.Hiding();
        }
    }

    public void DestroyView<T>(string name) where T : View {
        GameObject viewPrefab = null;
        foreach (GameObject prefab in m_UIViewDic.Keys) {
            if (prefab != null && prefab.name != name)
                continue;

            if (m_UIViewDic[prefab] is T) {
                viewPrefab = prefab;
            }
        }

        if (m_UIViewDic.ContainsKey(viewPrefab)) {
            View view = m_UIViewDic[viewPrefab];
            view.Destroy();
            m_UIViewDic.Remove(viewPrefab);
        }
    }

    public GameObject GetViewRoot(GameObject gameobject) {
        Transform parent = gameobject.transform.parent;
        ViewRoot parentPanel = null;
        while (parent != null) {
            if (parent.GetComponent<ViewRoot>()) {
                parentPanel = parent.gameObject.GetComponent<ViewRoot>();
            }
            parent = parent.parent;
        }
        return parentPanel != null ? parentPanel.gameObject : null;
    }

    public void DestroyAllView() {
        foreach (GameObject prefab in m_UIViewDic.Keys) {
            GameObject.Destroy(prefab);
        }
        m_UIViewDic.Clear();
        Resources.UnloadUnusedAssets();
    }
    public void SendNotification(string notificationName, Bundle bundle = null) {
        ViewControl.Instance.ExecuteCommand(notificationName,bundle);
    }

}
