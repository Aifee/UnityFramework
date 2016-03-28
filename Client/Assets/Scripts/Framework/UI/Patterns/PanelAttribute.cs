using System;

[AttributeUsage(AttributeTargets.Class)]
public class PanelAttribute : Attribute {
    public string PanelName { get; set; }
    public UILayer Layer { get; set; }

    public ICommand Type;
}
