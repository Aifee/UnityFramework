using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Panel(PanelName = "LoadingPanel", Layer = UILayer.Default)]
public class LoadingPanel : Panel {
   
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
        
    }
    protected override void Enable()
    {
        
    }
    protected override void Dormancy()
    {
        
    }

}