using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class AssetBundleInspector : EditorSingleton<AssetBundleInspector> {
    public AssetSetting Setting;
    private Vector2 Ascroll = Vector2.zero;
    private Vector2 Dscroll = Vector2.zero;
    private bool isShowNotify = true;

    protected override void Init() {
        this.maxSize = new Vector2(500f, 500f);
        this.minSize = new Vector2(500f, 300f);
        this.autoRepaintOnSceneChange = true;
        Setting = AssetSetting.Reading();
    }
    protected override void Disable() {
        base.Disable();
    }

    void OnGUI() {
        EditorGUIUtility.fieldWidth = 80f;
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Refresh", GUILayout.Width(80f))) {
                AssetDatabase.Refresh();
            }
            EditorGUILayout.LabelField("   ");
            if (GUILayout.Button("Help", GUILayout.Width(60f))) {
                isShowNotify = !isShowNotify;
            }
        }
        EditorGUILayout.EndHorizontal();
        AssetBundleUtils.DrawSeparator(6f, 2f);
        if (isShowNotify) {
            EditorGUILayout.HelpBox(Setting.ReadMe, MessageType.Info,true);
        }
        AssetBundleUtils.HighlightLine(Color.black, 20f);
        EditorGUILayout.LabelField("Asset Setting");
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Platform *", GUILayout.Width(100f));
            Setting.Platform = (AssetSetting.TargetPlatform)EditorGUILayout.EnumPopup(Setting.Platform);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Root Directory *", GUILayout.Width(100f));
            EditorGUILayout.TextField(Setting.RootDirectory);
            if (GUILayout.Button("Browse", GUILayout.Width(60f))) {
                string path = EditorUtility.OpenFolderPanel("Select floder for building bundles", Setting.RootDirectory, "");
                Setting.RootDirectory = path.Length > 0 ? path : Setting.RootDirectory;
                GUI.FocusControl("Browse");
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Clear")) {
                ClearAssetBundle();
            }
            if (GUILayout.Button("Apply")) {
                ApplyAssetBundleSettings();
                AssetSetting.Writeing(Setting);
            }
            if (GUILayout.Button("Build")) {
                AssetBundleManifest maniface = BuildPipeline.BuildAssetBundles(Setting.RootDirectory, BuildAssetBundleOptions.DeterministicAssetBundle, EditorUserBuildSettings.activeBuildTarget);
                ImportXML();
            }
        }
        EditorGUILayout.EndHorizontal();
        Setting.CheckRemove();
        AssetBundleUtils.DrawSeparator(6f, 2f);
        if (Setting.DependenciesAssetGroups.Count > 0) {
            Dscroll = EditorGUILayout.BeginScrollView(Dscroll);
            {
                foreach (AssetGroup group in Setting.DependenciesAssetGroups.Values) {
                    DrawBundleGroupItem(group);
                }
            }
            EditorGUILayout.EndScrollView();
        }
        if (GUILayout.Button("Add Dependencies AssetGroup")) {
            if (AssetGroupInspector.Instance == null) {
                UnityMenu.OpenAssetGroupEditor("New Dependencies Asset Group");
                if (AssetGroupInspector.Instance != null) {
                    AssetGroupInspector.Instance.SetData("New Dependencies Asset Group", Setting.RootDirectory, AssetGroup.BundleType.Dependencies);
                }
            } else {
                bool options = EditorUtility.DisplayDialog("Do you want to Replace or Cancel?", "Are you sure Replace or Cancel?", "Replace", "Cancel");
            }
        }
        AssetBundleUtils.DrawSeparator(6f, 2f);
        if (Setting.AssetGroups.Count > 0) {
            Ascroll = EditorGUILayout.BeginScrollView(Ascroll);
            {
                foreach (AssetGroup group in Setting.AssetGroups.Values) {
                    DrawBundleGroupItem(group);
                }
            }
            EditorGUILayout.EndScrollView();
        }
        if (GUILayout.Button("Add AssetGroup")) {
            if (AssetGroupInspector.Instance == null) {
                UnityMenu.OpenAssetGroupEditor("New Asset Group");
                if (AssetGroupInspector.Instance != null) {
                    AssetGroupInspector.Instance.SetData("New Asset Group", Setting.RootDirectory, AssetGroup.BundleType.Asset);
                }
            } else {
                bool options = EditorUtility.DisplayDialog("Do you want to Replace or Cancel?", "Are you sure Replace or Cancel?", "Replace", "Cancel");
            }
        }
        
    }
    private void DrawBundleGroupItem(AssetGroup group) {
        Color color = group.bundleType == AssetGroup.BundleType.Dependencies ? Color.red : Color.blue;
        AssetBundleUtils.HighlightLine(color, 25f);
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(group.Module);
            if (GUILayout.Button("Info")) {
                if (AssetGroupInspector.Instance == null) {
                    UnityMenu.OpenAssetGroupEditor(group.Module + " Asset Group");
                    if (AssetGroupInspector.Instance != null) {
                        AssetGroupInspector.Instance.SetData(group.Module + " Asset Group", Setting.RootDirectory, group.bundleType, group);
                    }
                } else {
                    bool options = EditorUtility.DisplayDialog("Do you want to Replace or Cancel?", "Are you sure Replace or Cancel?", "Replace", "Cancel");
                }
            }
            if (GUILayout.Button("X", GUILayout.Width(20f))) {
                Setting.AddRemove(group);
            }
        }
        EditorGUILayout.EndHorizontal();
        AssetBundleUtils.DrawSeparator(6f, 2f);
    }
    private void ClearAssetBundle() {
        string[] assetbundleNames = AssetDatabase.GetAllAssetBundleNames();
        for (int i = 0, imax = assetbundleNames.Length; i < imax; i++) {
            string[] paths = AssetDatabase.GetAssetPathsFromAssetBundle(assetbundleNames[i]);
            for (int j = 0, jmax = paths.Length; j < jmax; j++) {
                AssetImporter importer = AssetImporter.GetAtPath(paths[j]);
                importer.assetBundleName = null;
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        AssetDatabase.RemoveUnusedAssetBundleNames();
    }
    private void ApplyAssetBundleSettings() {
        Debug.Log(Setting.DependenciesAssetGroups.Count);
        if (Setting.DependenciesAssetGroups.Count > 0) {
            foreach (AssetGroup group in Setting.DependenciesAssetGroups.Values) {
                if (group.Items.Count > 0) {
                    foreach (AssetItem item in group.Items.Values) {
                        ApplyDependencyAssetBundles(item);
                    }
                }
            }
        }
        if (Setting.AssetGroups.Count > 0) {
            foreach (AssetGroup group in Setting.AssetGroups.Values) {
                if (group.Items.Count > 0) {
                    foreach (AssetItem item in group.Items.Values) {
                        string path = item.Path;
                        ApplySingleAssetBundle(path);
                    }
                }
            }
        }
    }
    private void ApplyDependencyAssetBundles(AssetItem item/*string rootObjPath, bool ignoreTexAndShader, bool ignoreMesh*/) {
        if (string.IsNullOrEmpty(item.Path)) { return; }
        string[] dependencies = AssetDatabase.GetDependencies(new string[] { item.Path });
        for (int i = 0, imax = dependencies.Length; i < imax; i++) {
            string path = dependencies[i];
            Object asset = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
            if (asset is Texture || asset is Shader) { continue; }
            if (path.ToLower().EndsWith("fbx") || asset is Mesh) { continue; }
            ApplySingleAssetBundle(path);
        }
    }
    private void ApplySingleAssetBundle(string path) {
        if (string.IsNullOrEmpty(path)) { return; }
        string pathLower = path.ToLower();
        if (path.StartsWith("Assets/Scripts/") || pathLower.EndsWith(".js") ||
            pathLower.EndsWith(".cs") || pathLower.EndsWith(".dll")) { return; }
        AssetImporter importer = AssetImporter.GetAtPath(path);
        int index = path.LastIndexOf("/Resources/");
        path = path.Substring(index + 11);
        System.Security.Cryptography.MD5 md5Calc = System.Security.Cryptography.MD5.Create();
        byte[] hash = md5Calc.ComputeHash(System.Text.Encoding.UTF8.GetBytes(path));
        string md5 = System.BitConverter.ToString(hash).Replace("-", "").ToLower();
        md5Calc.Clear();
        importer.assetBundleName = md5 + ".ab";
    }

    private void ImportXML() {
        string[] paths = System.IO.Directory.GetFiles(Setting.RootDirectory);
        XmlDocument doc = new XmlDocument();
        doc.CreateXmlDeclaration("1.0", "uft-8", "yes");
        XmlElement root = doc.CreateElement("files");
        root.SetAttribute("version", "album1");
        doc.AppendChild(root);
        if (paths.Length > 0) {
            foreach (string str in paths) {
                string fileName = AssetBundleUtils.GetFullName(str);
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(str);
                XmlElement item = doc.CreateElement("Item");
                item.SetAttribute("file", fileName);
                item.SetAttribute("size", fileInfo.Length.ToString());
                root.AppendChild(item);
            }
        }
        doc.Save(Setting.RootDirectory + "VersionConfig.xml");
    }

}
