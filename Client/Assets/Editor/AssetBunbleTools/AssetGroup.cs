using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
[System.Serializable]
public class AssetGroup {

    public enum AssetType {
        StreamedAssets = 1,
        StreamedScenes = 2,
    }
    public enum BundleType {
        Asset = 0,
        Dependencies = 1,
    }
    /// <summary> 所属模块 </summary>
    public string Module;
    /// <summary> 资源类型 </summary>
    public AssetType Type = AssetType.StreamedAssets;
    public BundleType bundleType = BundleType.Asset;
    /// <summary> 资源列表 </summary>
    private Dictionary<string, AssetItem> items = new Dictionary<string, AssetItem>();
    [System.NonSerialized]
    private List<string> removeItems = new List<string>();
    public Dictionary<string, AssetItem> Items { private set { items = value; } get { return items; } }

    public AssetGroup() {

    }
    public AssetGroup(BundleType type) {
        bundleType = type;
    }

    public AssetGroup(AssetGroup ag) {
        Module = ag.Module;
        Type = ag.Type;
        bundleType = ag.bundleType;
        items = ag.items;
    }
    public void AddItem(AssetItem item) {
        if (!items.ContainsKey(item.Name)) {
            items.Add(item.Name, item);
        }
    }
    public void Remove(string itemName) {
        if (!removeItems.Contains(itemName)) {
            removeItems.Add(itemName);
        }
    }
    public void CheckItems() {
        if (removeItems.Count > 0) {
            foreach (string key in removeItems) {
                if (items.ContainsKey(key))
                    items.Remove(key);
            }
            removeItems.Clear();
        }
    }
    public string[] AssetPaths() {
        List<string> paths = new List<string>();
        if (items.Count > 0) {
            foreach (AssetItem item in items.Values) {
                paths.Add(item.Path);
            }
        }
        return paths.ToArray();
    }
}
