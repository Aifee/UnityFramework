using UnityEngine;
using UnityEditor;
using System.Collections;

public class LoaderEditorUtils  {

    public static bool CheckPlatform(UnityEditor.BuildTarget target)
    {
        if (EditorUserBuildSettings.activeBuildTarget != target)
        {
            EditorUtility.DisplayDialog("目标平台与当前平台不一致，请先进行平台转换", "当前平台：" + EditorUserBuildSettings.activeBuildTarget + "\n目标平台：" + target, "OK");
            return false;
        }
        return true;
    }
}
