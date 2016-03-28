using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class UIManager : IUIManager {
    protected static IUIManager m_instance;
    protected static readonly object m_StaticSyncRoot = new object();
    protected static Camera m_UICamera;
    protected static UIRoot m_UIRoot;
    
    protected IController m_Controller;
    protected IView m_View;
    protected IDictionary<UILayer, Transform> m_LayerMap = new Dictionary<UILayer, Transform>();
    
    #region Instance
    
    public static IUIManager Instance {
        get {
            if (m_instance == null) {
                lock (m_StaticSyncRoot) {
                    if (m_instance == null)
                        m_instance = new UIManager();
                }
            }
            return m_instance;
        }
    }
    
    #endregion
    
    public static Camera UICamera {
        get {
            if (m_UICamera == null) {
                m_UICamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
            }
            return m_UICamera;
        }
    }
    public static UIRoot UIRoot {
        get {
            if (m_UIRoot == null) {
                m_UIRoot = GameObject.FindGameObjectWithTag("UIRoot").GetComponent<UIRoot>();
            }
            return m_UIRoot;
        }
    }
    
    protected UIManager() {
        if(UIManager.UIRoot.GetComponent<DontDestroyMe>() == null){
            UIManager.UIRoot.gameObject.AddComponent<DontDestroyMe>();
        }
        InitializeUIManager();
        IntiLayer();
    }
    
    protected virtual void InitializeUIManager() {
        InitializeController();
        InitializeView();
    }
    
    protected virtual void InitializeView() {
        if (m_View != null) return;
        m_View = View.Instance;
    }
    protected virtual void InitializeController() {
        if (m_Controller != null) return;
        m_Controller = Controller.Instance;
    }
    
    private void IntiLayer() {
        UIPanel[] panels = UIRoot.GetComponentsInChildren<UIPanel>();
        if (panels.Length > 0) {
            for (int i = 0; i < panels.Length; i++) {
                if (panels[i].transform == UIManager.UIRoot.transform)
                    continue;
                GameObject.Destroy(panels[i].gameObject);
            }
        }
        string[] values = System.Enum.GetNames(typeof(UILayer));
        for (int i = 0, iMax = values.Length; i < iMax; i++) {
            GameObject panelGO = new GameObject();
            panelGO.transform.parent = UIRoot.transform;
            panelGO.transform.localRotation = Quaternion.identity;
            panelGO.transform.localScale = Vector3.one;
            panelGO.transform.localPosition = Vector3.zero;
            UIPanel uiPanel = panelGO.AddComponent<UIPanel>();
            UILayer uilayer = (UILayer)System.Enum.Parse(typeof(UILayer), values[i]);
            panelGO.name = uilayer.ToString();
            uiPanel.depth = (int)uilayer;
            uiPanel.renderQueue = UIPanel.RenderQueue.StartAt;
            uiPanel.startingRenderQueue = 2000 + uiPanel.depth + 500 * i;
            uiPanel.gameObject.layer = LayerMask.NameToLayer("UI");
            m_LayerMap.Add(uilayer, uiPanel.transform);
        }
    }
    
    #region Command
    
    public void RegisterCommand(string notificationName, System.Type commandType) {
        m_Controller.RegisterCommand(notificationName, commandType);
    }
    
    public void RemoveCommand(string notificationName) {
        m_Controller.RemoveCommand(notificationName);
    }
    
    public bool HasCommand(string notificationName) {
        return m_Controller.HasCommand(notificationName);
    }
    
    #endregion
    
    #region Observers
    
    public void NotifyObservers(INotification note) {
        m_View.NotifyObservers(note);
    }
    
    #endregion
    
    #region INotifier
    
    public void SendNotification(string notificationName) {
        NotifyObservers(new Notification(notificationName));
    }
    
    public void SendNotification(string notificationName, object body) {
        NotifyObservers(new Notification(notificationName, body));
    }
    
    public void SendNotification(string notificationName, object body, string type) {
        NotifyObservers(new Notification(notificationName, body, type));
    }
    
    #endregion 
    
    #region Panel
    
    public IPanel Show<T>() where T : IPanel {
        IPanel panel = m_View.RetrievePanel(typeof(T));
        if (panel == null)
            panel = CreatePanel<T>();
        if (panel.Layer == UILayer.Default) {
            m_View.HideOtherLayer(panel, new List<UILayer>(new UILayer[] { UILayer.Default, UILayer.Secondary,UILayer.Pupop,UILayer.Guide }));
        } else if (panel.Layer == UILayer.Secondary) {
            m_View.HideOtherLayer(panel, new List<UILayer>(new UILayer[] { UILayer.Secondary, UILayer.Pupop,UILayer.Guide }));
        } else if (panel.Layer == UILayer.Pupop) {
            m_View.HideOtherLayer(panel, new List<UILayer>(new UILayer[] { UILayer.Pupop, UILayer.Guide }));
        } else if (panel.Layer == UILayer.Guide) {
            m_View.HideOtherLayer(panel, new List<UILayer>(new UILayer[] { UILayer.Guide }));
        } else if (panel.Layer == UILayer.TipAndWraning) {
            m_View.HideOtherLayer(panel, new List<UILayer>(new UILayer[] { UILayer.TipAndWraning }));
        }
        panel.Show();
        return panel;
    }
    
    public IPanel Show(System.Type type) {
        IPanel panel = m_View.RetrievePanel(type);
        if (panel == null)
            panel = CreatePanel(type);
        if (panel.Layer == UILayer.Default) {
            m_View.HideOtherLayer(panel, new List<UILayer>(new UILayer[] { UILayer.Default, UILayer.Secondary, UILayer.Pupop, UILayer.Guide }));
        } else if (panel.Layer == UILayer.Secondary) {
            m_View.HideOtherLayer(panel, new List<UILayer>(new UILayer[] { UILayer.Secondary, UILayer.Pupop, UILayer.Guide }));
        } else if (panel.Layer == UILayer.Pupop) {
            m_View.HideOtherLayer(panel, new List<UILayer>(new UILayer[] { UILayer.Pupop, UILayer.Guide }));
        } else if (panel.Layer == UILayer.Guide) {
            m_View.HideOtherLayer(panel, new List<UILayer>(new UILayer[] { UILayer.Guide }));
        } else if (panel.Layer == UILayer.TipAndWraning) {
            m_View.HideOtherLayer(panel, new List<UILayer>(new UILayer[] { UILayer.TipAndWraning }));
        }
        panel.Show();
        return panel;
    }
    public void Hide<T>() where T : IPanel {
        IPanel panel = RetrievePanel<T>();
        if (panel != null)
            panel.Hide();
    }
    public void Hide(Type type) {
        IPanel panel = RetrievePanel(type);
        if (panel != null)
            panel.Hide();
    }
    public IPanel RetrievePanel<T>() where T : IPanel {
        return RetrievePanel(typeof(T));
    }
    public IPanel RetrievePanel(Type type) {
        return m_View.RetrievePanel(type);
    }
    public bool HasPanel<T>() where T : IPanel {
        return m_View.HasPanel(typeof(T));
    }
    public bool HasPanel(Type type) {
        return m_View.HasPanel(type);
    }
    public void DestroyPanel<T>() where T : IPanel {
        m_View.RemovePanel(typeof(T));
    }
    public void DestroyPanel(Type type) {
        m_View.RemovePanel(type);
    }
    public void DestroyAll() {
        m_View.RemoveAllPanel();
    }
    
    protected IPanel CreatePanel<T>() where T : IPanel{
        PanelAttribute classAttribute = (PanelAttribute)System.Attribute.GetCustomAttribute(typeof(T), typeof(PanelAttribute));
        IPanel panel = null;
        GameObject prefab = InstantiatePrefab(classAttribute.PanelName, classAttribute.Layer);
        if (prefab != null) {
            GetLabelContent(prefab.transform);
            panel = System.Activator.CreateInstance(typeof(T)) as IPanel;
            panel.Activation(classAttribute.PanelName, prefab, classAttribute.Layer);
            m_View.RegisterPanel(panel);
        } else {
            Debug.Log("The load UIPanle is error,error panelName :" + classAttribute.PanelName);
            return null;
        }
        return panel;
    }
    protected IPanel CreatePanel(Type type) {
        PanelAttribute classAttribute = (PanelAttribute)System.Attribute.GetCustomAttribute(type, typeof(PanelAttribute));
        IPanel panel = null;
        GameObject prefab = InstantiatePrefab(classAttribute.PanelName, panel.Layer);
        if (prefab != null) {
            GetLabelContent(prefab.transform);
            panel = System.Activator.CreateInstance(type) as IPanel;
            panel.Activation(classAttribute.PanelName, prefab, classAttribute.Layer);
            m_View.RegisterPanel(panel);
        } else {
            Debug.Log("The load UIPanle is error,error panelName :" + classAttribute.PanelName);
            return null;
        }
        return panel;
    }
    private GameObject InstantiatePrefab(string prefabID, UILayer layer) {
        GameObject prefab = null;
        string prefabPath = "UI/" + prefabID;
        GameObject go = Resources.Load(prefabPath) as GameObject;
        if (go != null) {
            prefab = GameObject.Instantiate(go) as GameObject;
            prefab.name = prefabID;
            Camera uiCamera = UICamera;
            if (uiCamera == null) {
                Debug.LogError("UICamera is not found");
                return null;
            }
            prefab.transform.parent = m_LayerMap.ContainsKey(layer) ? m_LayerMap[layer] : UIManager.UIRoot.transform;
            prefab.transform.localRotation = Quaternion.identity;
            prefab.transform.localScale = Vector3.one;
            prefab.transform.localPosition = new Vector3(0, 0, Mathf.Clamp(prefab.transform.localPosition.z, -2, 2));
        }
        return prefab;
    }
    private void GetLabelContent(Transform transform) {
        UILabel[] labelArray = transform.GetComponentsInChildren<UILabel>(true);
        foreach (UILabel label in labelArray) {
            if (label != null)
                label.transform.localPosition = new Vector3(label.transform.localPosition.x, label.transform.localPosition.y, label.transform.localPosition.z - 1);
            string labelContent = label.text;
            if (labelContent != null && labelContent != "") {
                try {
                    string textContent = "";// LanguageManager.Instance.GetLanguage(labelContent.Trim());
                    if (textContent != null && textContent != "")
                        label.text = textContent;
                } catch { }
            }
        }
    }
    #endregion

    #region static function

    private static IPanel optionPopup = null;
    public static void OptionPopup(string info = "",List<ePopupType> list = null,Action<ePopupType> action = null){
        if(optionPopup == null){
            optionPopup = UIManager.Instance.Show<OptionPopupPanel>();
            Bundle bundle = new Bundle();
            bundle.SetValue<string>("info",info);
            bundle.SetValue<List<ePopupType>>("list",list);
            bundle.SetValue<Action<ePopupType>>("action",action);
            UIManager.Instance.SendNotification(OptionPopupPanel.OPTIONPOPUP_SHOWINFO,bundle);
        }else{
            UIManager.Instance.Hide<OptionPopupPanel>();
            optionPopup = null;
        }
    }


    #endregion
    
}

