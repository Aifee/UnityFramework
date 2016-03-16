using UnityEngine;
using System.Collections;
using System.Text;
using System.Security.Cryptography;

public static class LoaderUtility 
{
    public static string StreamingAssetsPath
    {
        get
        {
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

    public static string StreamingAssetsURL
    {
        get
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            return "file:///" + Application.dataPath + "/StreamingAssets/";
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
    public static string persistentDataPathURL
    {
        get
        {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            return "file:///" + Application.persistentDataPath + "/";
#else
            return "file://" + Application.persistentDataPath + "/";
#endif
        }
    }

    public static string GetMD5(byte[] bytes)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] output = md5.ComputeHash(bytes);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < output.Length; i++)
        {
            sb.Append(output[i].ToString("x2"));
        }
        return sb.ToString();
    }
    public static string GetStringMD5(string input)
    {
        // Create a new instance of the MD5CryptoServiceProvider object.
        MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();

        // Convert the input string to a byte array and compute the hash.
        byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data 
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }
}
