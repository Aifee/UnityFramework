using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ePopupType{
    Cancel = 0,
    Ok = 1,
}

[PanelAttribute(PanelName = "OptionPopupPanel",Layer = UILayer.Pupop)]
public class OptionPopupPanel : Panel {
    public const string OPTIONPOPUP_SHOWINFO = "OptionPopup_ShowInfo";

    private UILabel m_Label_Info;
    private Dictionary<ePopupType,UIButton> types = new Dictionary<ePopupType, UIButton>();
    private System.Action<ePopupType> clickAction;

    public override IList<string> ListNotificationInterests()
    {
        return new List<string>(new string[]{OPTIONPOPUP_SHOWINFO});
    }
    public override void HandleNotification(INotification notification)
    {
        switch(notification.Name){
            case OPTIONPOPUP_SHOWINFO:
                Bundle bundle = notification.Body as Bundle;
                m_Label_Info.text = bundle.GetValue<string>("info");
                List<ePopupType> list = bundle.GetValue<List<ePopupType>>("list");
                if(list != null && list.Count > 0){
                    ShowBtn(list);
                }
                clickAction = bundle.GetValue<System.Action<ePopupType>>("action");
                break;
        }
    }
    protected override void Start()
    {
        UIButton btn = GetChild<UIButton>("Btn_Cancel");
        UIEventListener.Get(btn.gameObject).onClick += OnClickBtn;
        btn.gameObject.SetActive(false);
        types.Add(ePopupType.Cancel,btn);
        btn = GetChild<UIButton>("Btn_Ok");
        UIEventListener.Get(btn.gameObject).onClick += OnClickBtn;
        btn.gameObject.SetActive(false);
        types.Add(ePopupType.Ok,btn);
        m_Label_Info = GetChild<UILabel>("Label_Info");
    }
    protected override void Enable()
    {

    }
    protected override void Dormancy()
    {
        if(types.Count > 0){
            foreach(UIButton btn in types.Values){
                btn.gameObject.SetActive(false);
            }
        }
    }
    private void ShowBtn(List<ePopupType> list){
        if(list.Count == 1){
            if(types.ContainsKey(list[0])){
                types[list[0]].gameObject.SetActive(true);
                GameObject go = types[list[0]].gameObject;
                go.transform.ResetPositionX(0);
                go.SetActive(true);
            }
        }else if(list.Count == 2){
            GameObject go = null;
            if(types.ContainsKey(list[0])){
                go = types[list[0]].gameObject;
                go.transform.ResetPositionX(-110f);
                go.SetActive(true);
            }
            if(types.ContainsKey(list[1])){
                go = types[list[1]].gameObject;
                go.transform.ResetPositionX(110);
                go.SetActive(true);
            }
        }
    }
    private void OnClickBtn(GameObject go){
        if(types.Count > 0){
            foreach(KeyValuePair<ePopupType,UIButton> kvp in types){
                if(kvp.Value.gameObject == go){
                    if(clickAction != null){
                        clickAction(kvp.Key);
                    }
                }
            }
        }
    }
	
}
