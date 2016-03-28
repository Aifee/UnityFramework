using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Panel : Notifier, IPanel, INotifier {
    public const string NAME = "Panel";
    public const UILayer LAYER = UILayer.Default;
    
    private string m_PanelName;
    protected GameObject gameObject;
    protected Transform transform;
    private UILayer m_Layer;
    protected Dictionary<System.Type, MonoBehaviour[]> m_Behaviours = new Dictionary<System.Type, MonoBehaviour[]>();
    
    //public Panel() : this(NAME, null, LAYER) { }
    //public Panel(string panelName) : this(panelName, null, LAYER) { }
    //public Panel(string panelName, GameObject go) : this(panelName, go, LAYER) { }
    //public Panel(string panelName,GameObject go,UILayer layer){
    //    Debug.LogFormat("panelName:{0},layer:{1}", panelName, layer);
    //    m_PanelName = (panelName == null || panelName.Equals("")) ? NAME : panelName;
    //    GameObject = go;
    //    m_Layer = layer;
    //}
    public void Activation(string panelName, GameObject go, UILayer layer) {
        m_PanelName = (panelName == null || panelName.Equals("")) ? NAME : panelName;
        GameObject = go;
        InitAnchor();
        m_Layer = layer;
        Start();
    }
    private void InitAnchor(){
        UIAnchor[] anchors = gameObject.GetComponentsInChildren<UIAnchor>();
        if(anchors.Length > 0){
            for(int i = 0, iMax = anchors.Length; i < iMax; i ++){
                anchors[i].uiCamera = UIManager.UICamera;
            }
        }
    }
    
    public UILayer Layer {
        get { return m_Layer; }
    }
    
    public string PanelName {
        get { return m_PanelName; }
    }
    
    public GameObject GameObject {
        get { return gameObject; }
        private set {
            gameObject = value;
            if (gameObject != null) {
                transform = gameObject.transform;
            }
        }
    }
    
    public Transform Transform {
        get { return transform; }
    }
    
    public virtual IList<string> ListNotificationInterests() {
        return new List<string>();
    }
    
    public virtual void HandleNotification(INotification notification) {
        throw new System.NotImplementedException();
    }
    
    public void Show() {
        if (gameObject == null) {
            Debug.LogErrorFormat("Controlled by {0} is null,Please check the loading process is correct...", PanelName);
        } else {
            gameObject.SetActive(true);
            Enable();
        }
    }
    
    public void Hide() {
        if (gameObject == null)
            Debug.LogErrorFormat("Controlled by {0} is null,Please check the loading process is correct...", PanelName);
        else {
            gameObject.SetActive(false);
            Dormancy();
        }
    }
    
    public void Destroy() {
        Dormancy();
        GameObject.Destroy(gameObject);
    }
    protected virtual void Start() {
        
    }
    protected virtual void Enable() {
        
    }
    protected virtual void Dormancy() {
        
    }
    protected T GetChild<T>(string childName) where T : MonoBehaviour {
        if (gameObject == null) {
            Debug.LogErrorFormat("Controlled by {0} is null,Please check the loading process is correct...", PanelName);
            return null;
        }
        T[] childs = null;
        if (m_Behaviours.ContainsKey(typeof(T)))
            childs = m_Behaviours[typeof(T)] as T[];
        else {
            childs = gameObject.GetComponentsInChildren<T>();
            m_Behaviours.Add(typeof(T), childs);
        }
        GameObject child = null;
        foreach (T t in childs) {
            if (childName.Equals(t.name))
                child = t.gameObject;
        }
        if (child == null) {
            Debug.LogError(childName + "is not child of" + gameObject.name);
            return null;
        }
        T tempT = child.GetComponent<T>();
        if (tempT == null)
            Debug.LogError(childName + "is not has component");
        
        return tempT;
    }
    protected GameObject GetChild(string childName) {
        if (gameObject == null) {
            Debug.LogErrorFormat("Controlled by {0} is null,Please check the loading process is correct...", PanelName);
            return null;
        }
        GameObject child = transform.FindChild(childName).gameObject;
        if (child == null) {
            Debug.LogError(gameObject.name + "is not found child of" + childName);
            return null;
        }
        return child;
    }
}

