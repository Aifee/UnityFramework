using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[PanelAttribute(PanelName = "SelectTowerPanel",Layer = UILayer.Default)]
public class SelectTowerPanel : Panel {
    public const string SELECTTOWER_INITDATA = "SelectTower_InitData";


    private UIButton m_Btn_Colse;
    private UIGrid m_Grid;
    private GameObject m_TowerItem;
    private Dictionary<GameObject,SelectTowerItem> items = new Dictionary<GameObject, SelectTowerItem>();

	public override IList<string> ListNotificationInterests()
    {
        return new List<string>(new string[]{SELECTTOWER_INITDATA});
    }
    public override void HandleNotification(INotification notification)
    {
        switch(notification.Name){
            case SELECTTOWER_INITDATA:
                int gridId = notification.Body.ToString().ToInt();
                InitData(gridId);
                break;
            default:

                break;
        }
    }
    protected override void Start()
    {
        m_Btn_Colse = GetChild<UIButton>("Btn_Colse");
        m_Grid = GetChild<UIGrid>("Grid");
        m_TowerItem = GetChild("Center/TowerItem");
        m_TowerItem.SetActive(false);

        UIEventListener.Get(m_Btn_Colse.gameObject).onClick += OnClickBack;
    }
    protected override void Enable()
    {

    }
    protected override void Dormancy()
    {
        if(items.Count > 0){
            foreach(GameObject go in items.Keys){
                GameObject.Destroy(go);
            }
            items.Clear();
        }

    }

    private void OnClickBack(GameObject go){
        UIManager.Instance.Show<MainPanel>();
    }

    private void InitData(int gridId){
        for(int i = 0; i < 10; i ++){
            GameObject prefab = GameObject.Instantiate(m_TowerItem);
            prefab.transform.ResetParent(m_Grid.transform);
            prefab.SetActive(true);
            SelectTowerItem item = new SelectTowerItem(prefab);
            item.ResetData(gridId,"tower"+ i,10* i,10*i);
            items.Add(prefab,item);
        }
        m_Grid.Reposition();
    }





    class SelectTowerItem{
        private GameObject gameObject;
        private Transform transform;
        private UILabel m_Label_Name;
        private UIButton m_Btn_Select;
        private UILabel m_Label_Hurt;
        private UILabel m_Label_Hp;

        private int index;

        public SelectTowerItem(GameObject go){
            gameObject = go;
            transform = go.transform;
            m_Label_Name = transform.FindChild("Label_Name").GetComponent<UILabel>();
            m_Label_Hurt = transform.FindChild("Label_Hurt").GetComponent<UILabel>();
            m_Label_Hp = transform.FindChild("Label_Hp").GetComponent<UILabel>();
            m_Btn_Select = transform.FindChild("Btn_Select").GetComponent<UIButton>();
            UIEventListener.Get(m_Btn_Select.gameObject).onClick += OnClickSelect;
        }
        public void ResetData(int id,string name,int hurt,int hp){
            index = id;
            m_Label_Name.text = name;
            m_Label_Hurt.text = hurt.ToString();
            m_Label_Hp.text = hp.ToString();
        }
        private void OnClickSelect(GameObject go){
            Building building = new Building();
            building.ID = index;
            building.Art = "Tow_Tesla1";
            MapManager.Instance.AddBuilding(index,building);
            UIManager.Instance.Show<MainPanel>();
        }
        
    }
}


