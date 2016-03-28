using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum eMainPanelShowType{
    /// <summary>
    /// 防御塔
    /// </summary>
    Tower = 0,
    /// <summary>
    /// 详情
    /// </summary>
    Details = 1,
    /// <summary>
    /// 升级
    /// </summary>
    Upgrade = 2,
    /// <summary>
    /// 建造
    /// </summary>
    Build = 3,
    /// <summary>
    /// 回收
    /// </summary>
    Recovery = 4,
}

[PanelAttribute(PanelName = "MainPanel", Layer = UILayer.Default)]
public class MainPanel : Panel {
    public const string MAINPANEL_SHOWOPTIONS = "MainPanel_ShowOptions";
    public const string MAINPANEL_HIDEOPTIONS = "MainPanel_HideOptions";

    /// <summary> 购买金币 </summary>
    private UIButton m_Btn_AddGold;
    /// <summary> 商店 </summary>
    private UIButton m_Btn_Store;
    /// <summary> 升级建筑 </summary>
    private UIButton m_Btn_UpgradeAll;
    /// <summary> 设置 </summary>
    private UIButton m_Btn_Setting;
    /// <summary> 帮助 </summary>
    private UIButton m_Btn_Help;
    /// <summary> 战役 </summary>
    private UIButton m_Btn_Battle;
    /// <summary> 任务 /summary>
    private UIButton m_Btn_Quests;
    /// <summary> 社区 </summary>
    private UIButton m_Btn_Community;
    /// <summary> 金币label </summary>
    private UILabel m_Label_Gold;
    /// <summary> 钻石label </summary>
    private UILabel m_Label_Gems;
    /// <summary> 货币lebel </summary>
    private UILabel m_Label_Currency;
    /// <summary> 食物label </summary>
    private UILabel m_Label_Food;
    /// <summary> 等级label </summary>
    private UILabel m_Label_Level;
    /// <summary> 排行榜label </summary>
    private UILabel m_Label_Leaderboard;
    /// <summary> 工人label </summary>
    private UILabel m_Label_Worker;

    private Transform m_Move;
    private Dictionary<eMainPanelShowType,UIButton> options = new Dictionary<eMainPanelShowType, UIButton>();
    private Action<eMainPanelShowType> action;

    public override System.Collections.Generic.IList<string> ListNotificationInterests()
    {
        return new List<string>(new string[]{MAINPANEL_SHOWOPTIONS,
            MAINPANEL_HIDEOPTIONS});
    }
    public override void HandleNotification(INotification notification)
    {
        switch(notification.Name){
            case MAINPANEL_SHOWOPTIONS:
                Bundle bundle = notification.Body as Bundle;
                showOption(bundle.GetValue<List<eMainPanelShowType>>("optionList"));
                action = bundle.GetValue<Action<eMainPanelShowType>>("action");
                break;
            case MAINPANEL_HIDEOPTIONS:
                HideOption();
                break;
        }
    }

	protected override void Start()
    {
        m_Btn_AddGold = GetChild<UIButton>("Btn_AddGold");
        m_Btn_Store = GetChild<UIButton>("Btn_Store");
        m_Btn_UpgradeAll = GetChild<UIButton>("Btn_UpgradeAll");
        m_Btn_Setting = GetChild<UIButton>("Btn_Setting");
        m_Btn_Help = GetChild<UIButton>("Btn_Help");
        m_Btn_Battle = GetChild<UIButton>("Btn_Battle");
        m_Btn_Quests = GetChild<UIButton>("Btn_Quests");
        m_Btn_Community = GetChild<UIButton>("Btn_Community");
        m_Label_Gold = GetChild<UILabel>("Label_Gold");
        m_Label_Gems = GetChild<UILabel>("Label_Gems");
        m_Label_Currency = GetChild<UILabel>("Label_Currency");
        m_Label_Food = GetChild<UILabel>("Label_Food");
        m_Label_Level = GetChild<UILabel>("Label_Level");
        m_Label_Leaderboard = GetChild<UILabel>("Label_Leaderboard");
        m_Label_Worker = GetChild<UILabel>("Label_Worker");
        m_Move = GetChild("Bottom/Move").transform;
        m_Move.transform.ResetPositionY(-50);
        UIButton btn = GetChild<UIButton>("Btn_Tower");
        UIEventListener.Get(btn.gameObject).onClick += OnClickOption;
        btn.gameObject.SetActive(false);
        options.Add(eMainPanelShowType.Tower,btn);

        btn = GetChild<UIButton>("Btn_Details");
        UIEventListener.Get(btn.gameObject).onClick += OnClickOption;
        btn.gameObject.SetActive(false);
        options.Add(eMainPanelShowType.Details,btn);

        btn = GetChild<UIButton>("Btn_Upgrade");
        UIEventListener.Get(btn.gameObject).onClick += OnClickOption;
        btn.gameObject.SetActive(false);
        options.Add(eMainPanelShowType.Upgrade,btn);

        btn = GetChild<UIButton>("Btn_Build");
        UIEventListener.Get(btn.gameObject).onClick += OnClickOption;
        btn.gameObject.SetActive(false);
        options.Add(eMainPanelShowType.Build,btn);

        btn = GetChild<UIButton>("Btn_Recovery");
        UIEventListener.Get(btn.gameObject).onClick += OnClickOption;
        btn.gameObject.SetActive(false);
        options.Add(eMainPanelShowType.Recovery,btn);

        UIEventListener.Get(m_Btn_AddGold.gameObject).onClick += OnClickAddGold;
        UIEventListener.Get(m_Btn_Store.gameObject).onClick += OnClickStore;
        UIEventListener.Get(m_Btn_UpgradeAll.gameObject).onClick += OnClickUpgradeAll;
        UIEventListener.Get(m_Btn_Setting.gameObject).onClick += OnClickSetting;
        UIEventListener.Get(m_Btn_Help.gameObject).onClick += OnClickHelp;
        UIEventListener.Get(m_Btn_Battle.gameObject).onClick += OnClickBattle;
        UIEventListener.Get(m_Btn_Quests.gameObject).onClick += OnClickQuests;
        UIEventListener.Get(m_Btn_Community.gameObject).onClick += OnClickCommunity;
    }
    protected override void Enable()
    {
        base.Enable();
    }
    protected override void Dormancy()
    {
        HideOption();
    }

    private void showOption(List<eMainPanelShowType> list){
        bool isOdd = list.Count % 2 != 0;
        float startX = 0;
        if(isOdd){
            startX = -(list.Count - 1) / 2 * 80;
        }else{
            startX = -list.Count / 2 * 80 + 40;
        }
        for(int i = 0, iMax = list.Count; i < iMax ; i ++){
            if(options.ContainsKey(list[i])){
                options[list[i]].transform.ResetPositionX(startX + i * 80);
                options[list[i]].gameObject.SetActive(true);
            }
        }
        optionAni(true);
    }
    private void HideOption(){
        foreach(UIButton btn in options.Values){
            btn.gameObject.SetActive(false);
        }
        optionAni(false);
    }

    private void optionAni(bool active){
        float targetY = active ? 50: -50;
        iTween.MoveTo(m_Move.gameObject,iTween.Hash("time",0.2,"islocal",true,"y",targetY));
    }

    /// <summary>
    /// 点击购买金币
    /// </summary>
    /// <param name="go">Go.</param>
    private void OnClickAddGold(GameObject go){

    }
    /// <summary>
    /// 点击商店
    /// </summary>
    /// <param name="go">Go.</param>
    private void OnClickStore(GameObject go){
        
    }
    /// <summary>
    /// 点击升级建筑
    /// </summary>
    /// <param name="go">Go.</param>
    private void OnClickUpgradeAll(GameObject go){
        
    }
    /// <summary>
    /// 点击设置
    /// </summary>
    /// <param name="go">Go.</param>
    private void OnClickSetting(GameObject go){
        
    }
    /// <summary>
    /// 点击帮助
    /// </summary>
    /// <param name="go">Go.</param>
    private void OnClickHelp(GameObject go){
        
    }
    /// <summary>
    /// 点击战役
    /// </summary>
    /// <param name="go">Go.</param>
    private void OnClickBattle(GameObject go){
        
    }
    /// <summary>
    /// 点击任务
    /// </summary>
    /// <param name="go">Go.</param>
    private void OnClickQuests(GameObject go){
        
    }
    /// <summary>
    /// 点击社区
    /// </summary>
    /// <param name="go">Go.</param>
    private void OnClickCommunity(GameObject go){

    }
    private void OnClickOption(GameObject go){
        if(action == null)
            return;
        foreach(KeyValuePair<eMainPanelShowType,UIButton> kvp in options){
            if(go == kvp.Value.gameObject){
                action(kvp.Key);
            }
        }
        HideOption();
    }

}
