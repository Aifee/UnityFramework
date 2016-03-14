using UnityEngine;
using UnityEditor;
using System.Collections;

public class UnityMenu  {

	[MenuItem("Custom/ExcelTool")]
	static public void OpenExcelTool(){
		EditorWindow.GetWindow<ExcelTool> (false, "Excel Tool", true).Show ();
	}
	[MenuItem("Custom/AssetBundleTool")]
	public static void OpenAssetBundleTool(){
		EditorWindow.GetWindow<AssetBundleInspector> (false, "AssetBundle Tool", true).Show ();
	}
	static public void OpenAssetGroupEditor(string title){
		EditorWindow.GetWindow<AssetGroupInspector> (false, title, true).Show ();
	}
}
