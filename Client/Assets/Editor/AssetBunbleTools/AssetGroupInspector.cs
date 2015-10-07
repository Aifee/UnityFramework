using UnityEngine;
using UnityEditor;
using System.Collections;

public class AssetGroupInspector : EditorSingleton<AssetGroupInspector> {
    private string Title;
    private string RootDirectory;
    private AssetGroup group;
    private Vector2 scroll = Vector2.zero;
    protected override void Init() {
        this.maxSize = new Vector2(500f, 1000f);
        this.minSize = new Vector2(500f, 500f);
        this.autoRepaintOnSceneChange = true;
    }
    public void SetData(string title, string rootDicrectory, AssetGroup.BundleType bt,AssetGroup ag = null) {
        Title = title;
        RootDirectory = rootDicrectory;
        if (ag == null) {
            group = new AssetGroup();
            group.bundleType = bt;
        } else {
            group = new AssetGroup(ag);
        }
    }
    protected override void Disable() {
        base.Disable();
    }
    void OnGUI() {
        if (group == null)
            return;
        EditorGUIUtility.fieldWidth = 80f;
        EditorGUILayout.LabelField(Title + ":");
        AssetBundleUtils.DrawSeparator(6f, 2f);
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Module *", GUILayout.Width(60f));
            group.Module = EditorGUILayout.TextField(group.Module); ;
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Path", GUILayout.Width(60f));
            EditorGUILayout.LabelField(RootDirectory);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Type *", GUILayout.Width(60f));
            group.Type = (AssetGroup.AssetType)EditorGUILayout.EnumPopup(group.Type);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUI.BeginDisabledGroup(group.Items.Count <= 0 || group.Module == null || group.Module == "");
        {
            if (GUILayout.Button("Add/Update All")) {
                if (AssetBundleInspector.Instance != null) {
                    bool isHasGroup = false;
                    if (group.bundleType == AssetGroup.BundleType.Asset) {
                        isHasGroup = AssetBundleInspector.Instance.Setting.AddAssetGroup(group);
                    }else if (group.bundleType == AssetGroup.BundleType.Dependencies) {
                        isHasGroup = AssetBundleInspector.Instance.Setting.AddDependencies(group);
                    }
                    if (isHasGroup) {
                        this.Close();
                    }
                }
            }
        }
        EditorGUI.EndDisabledGroup();
        AssetBundleUtils.DrawSeparator(6f, 2f);
        AssetBundleUtils.HighlightLine(Color.black, 20f);
        EditorGUILayout.LabelField("Assets");
        scroll = EditorGUILayout.BeginScrollView(scroll);
        {
            if (group.Items.Count > 0) {
                foreach (AssetItem item in group.Items.Values) {
                    AssetBundleUtils.HighlightLine(Color.white, 25f);
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField(item.Name);
                        EditorGUILayout.LabelField(GetObjectType(item.Obj), GUILayout.Width(100f));
                        if (GUILayout.Button("X", GUILayout.Width(20f))) {
                            group.Remove(item.Name);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    AssetBundleUtils.DrawSeparator(6f, 2f);
                }
            }
        }
        EditorGUILayout.EndScrollView();
        group.CheckItems();
    }
    void OnSelectionChange() {
        SelectedObjects();
        Repaint();
    }
    private void SelectedObjects() {
        if (Selection.objects != null && Selection.objects.Length > 0) {
            Object[] objects = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
            foreach (Object o in objects) {
                if (group.Items != null && !group.Items.ContainsKey(o.name)) {
                    AssetItem item = new AssetItem();
                    item.Name = o.name;
                    item.Obj = o;
                    item.Path = AssetDatabase.GetAssetPath(o);
                    item.FullName = AssetBundleUtils.GetFullName(item.Path);
                    item.Type = AssetBundleUtils.GetType(item.FullName);
                    item.MD5 = GetMd5(item.Path);
                    group.AddItem(item);
                }
            }
        }
    }
    private string GetMd5(string path) {
        AssetImporter importer = AssetImporter.GetAtPath(path);
        int index = path.LastIndexOf("/Resources/");
        path = path.Substring(index + 11);
        System.Security.Cryptography.MD5 md5Calc = System.Security.Cryptography.MD5.Create();
        byte[] hash = md5Calc.ComputeHash(System.Text.Encoding.UTF8.GetBytes(path));
        string md5 = System.BitConverter.ToString(hash).Replace("-", "").ToLower();
        md5Calc.Clear();
        return md5;
    }
    private string GetObjectType(Object o) {
        string type = o.GetType().ToString();
        return type.Substring(12);
    }
}
