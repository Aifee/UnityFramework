using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[PanelAttribute(PanelName = "LoginPanel", Layer = UILayer.Default)]
public class LoginPanel : Panel {

    private UIButton Btn_Login;

    public override IList<string> ListNotificationInterests()
    {
        return new List<string>();
    }
    public override void HandleNotification(INotification notification)
    {
        switch(notification.Name){
            default:

                break;
        }
    }

	protected override void Start()
    {
        Btn_Login = GetChild<UIButton>("Btn_Login");
        UIEventListener.Get(Btn_Login.gameObject).onClick += OnClickLoginBtn;
    }
    protected override void Enable()
    {
        base.Enable();
    }
    protected override void Dormancy()
    {
        base.Dormancy();
    }


    private void OnClickLoginBtn(GameObject go){
        GameStatesManager.Instance.SwitchState(SceneName.Manager.ToString());
    }
}
