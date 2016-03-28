using UnityEngine;
using System.Collections;
using System.IO;

public sealed class PathUtil
{
    public static string streamingFilePath {
        get {
            #if UNITY_STANDALONE || UNITY_EDITOR
            return Application.dataPath + "/StreamingAssets/";
            #elif UNITY_IPHONE
            return Application.dataPath + "/Raw/";
            #elif UNITY_ANDROID
            return "jar:file://" + Application.dataPath + "!/assets/";
            #else
            return Application.streamingAssetsPath + "/";
            #endif
        }
    }
    public static string streamingURLPath {
        get {
            #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || UNITY_EDITOR
            return "file:///" +Application.dataPath + "/StreamingAssets/";
            #elif UNITY_IPHONE
            return "file://" + Application.dataPath + "/Raw/";
            #elif UNITY_STANDALONE || UNITY_EDITOR
            return "file://" + Application.dataPath + "/StreamingAssets/";
            #elif UNITY_ANDROID
            return "jar:file://" + Application.dataPath + "!/assets/";
            #else
            return "file://" + Application.streamingAssetsPath + "/";
            #endif
        }
    }
    public static string persistentDataFilePath {
        get {
            return Application.persistentDataPath + "/";
        }
    }
    
    public static string persistentDataURLPath {
        get {
            #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || UNITY_EDITOR
            return "file:///" + Application.persistentDataPath + "/";
            #else
            return "file://" + Application.persistentDataPath + "/";
            #endif
        }
    }

	/// <summary>
	/// 检测制定文件路径是否存在
	/// </summary>
	/// <param name="filePath"> 相对路径("Assets" 目录) </param>
	/// <returns></returns>
	public static bool RelativeFileExist(string filePath)
	{
		bool exist = File.Exists(filePath);
		return exist;
	}
	/// <summary>
	/// 如果存在的路径(路径应该开始与“Assets”)
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static bool RelativePathExist(string path)
	{
		bool exist = Directory.Exists(path);
		return exist;
	}

}
