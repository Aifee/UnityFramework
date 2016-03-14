using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class AssetSetting  {
    public static string SETTINGS_PATH = "ProjectSettings/AssetBundleSettings.asset";
    public enum TargetPlatform : int {
        StandaloneWindow = (int)BuildTarget.StandaloneWindows,
        StandaloneOSX = (int)BuildTarget.StandaloneOSXUniversal,
        Android = (int)BuildTarget.Android,
        iPhone = (int)BuildTarget.iOS,
        WebPlayer = (int)BuildTarget.WebPlayer,
    }
    public TargetPlatform Platform;
    public string RootDirectory;
    private Dictionary<string, AssetGroup> assetGroups = new Dictionary<string, AssetGroup>();
    private Dictionary<string, AssetGroup> dependenciesAssetGroups = new Dictionary<string, AssetGroup>();
    [System.NonSerialized]
    private Dictionary<string, AssetGroup> removeAssets = new Dictionary<string, AssetGroup>();
    [System.NonSerialized]
    private string readMe = "";
    public AssetSetting() {
        if (string.IsNullOrEmpty(RootDirectory)) {
            RootDirectory = Application.dataPath + "/../AssetBundle/";
        }
#if UNITY_STANDALONE_WIN
        Platform = TargetPlatform.StandaloneWindow;
#elif UNITY_STANDALONE_OSX
        Platform = TargetPlatform.StandaloneOSX;
#elif UNITY_ANDROID
        Platform = TargetPlatform.Android;
#elif UNITY_IPHONE
        Platform = TargetPlatform.iPhone;
#elif UNITY_WEBPLAYER
        Platform = TargetPlatform.WebPlayer;
#endif
    }
    public string ReadMe {
        get {
            if (readMe == null || readMe == "") {
                TextAsset text = AssetDatabase.LoadAssetAtPath("Assets/Editor/AssetBunbleTools/ReadMe.txt", typeof(TextAsset)) as TextAsset;
                if (text != null) {
                    readMe = text.text;
                }
            }
            return readMe;
        }
    }
    public Dictionary<string, AssetGroup> AssetGroups { get { return assetGroups; } }
    public bool AddAssetGroup(AssetGroup group) {
        if (assetGroups.ContainsKey(group.Module)) {
            assetGroups[group.Module] = group;
        } else {
            assetGroups.Add(group.Module, group);
        }
        return true;
    }
    public Dictionary<string, AssetGroup> DependenciesAssetGroups { get { return dependenciesAssetGroups; } }
    public bool AddDependencies(AssetGroup group) {
        if (dependenciesAssetGroups.ContainsKey(group.Module)) {
            dependenciesAssetGroups[group.Module] = group;
        } else {
            dependenciesAssetGroups.Add(group.Module, group);
        }
        return true;
    }
    public Dictionary<string, AssetGroup> RemoveAssets { get { return removeAssets; } }
    public bool AddRemove(AssetGroup group) {
        if (!removeAssets.ContainsKey(group.Module)) {
            removeAssets.Add(group.Module, group);
            return true;
        }
        return false;
    }

    public void CheckRemove() {
        if (removeAssets != null && removeAssets.Count > 0) {
            foreach (string groupKey in removeAssets.Keys) {
                if (assetGroups.ContainsKey(groupKey)) {
                    assetGroups.Remove(groupKey);
                }
                if (dependenciesAssetGroups.ContainsKey(groupKey)) {
                    dependenciesAssetGroups.Remove(groupKey);
                }
            }
            removeAssets.Clear();
        }
    }
    public static void Writeing(AssetSetting setting) {
        FileStream file = File.OpenWrite(SETTINGS_PATH);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(file, setting);
        file.SetLength(file.Position);
        file.Flush();
        file.Close();
        file.Dispose();
    }
    public static AssetSetting Reading() {
        if (File.Exists(SETTINGS_PATH)) {
            FileStream file = File.OpenRead(SETTINGS_PATH);
            BinaryFormatter formatter = new BinaryFormatter();
            AssetSetting ret = formatter.Deserialize(file) as AssetSetting;
            file.Close();
            file.Dispose();
            ret.removeAssets = new Dictionary<string, AssetGroup>();
            if (ret.AssetGroups.Count > 0) {
                foreach (AssetGroup group in ret.AssetGroups.Values) {
                    if (group.Items.Count > 0) {
                        foreach (AssetItem item in group.Items.Values) {
                            item.DeserializeObj();
                        }
                    }
                }
            }
            if (ret.DependenciesAssetGroups.Count > 0) {
                foreach (AssetGroup group in ret.DependenciesAssetGroups.Values) {
                    if (group.Items.Count > 0) {
                        foreach (AssetItem item in group.Items.Values) {
                            item.DeserializeObj();
                        }
                    }
                }
            }
            return ret;
        }
        AssetSetting settings = new AssetSetting();
        return settings;
    }
    
    
    
}
