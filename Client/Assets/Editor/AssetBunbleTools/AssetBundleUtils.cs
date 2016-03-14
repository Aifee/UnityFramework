using UnityEngine;
using UnityEditor;
using System.Collections;

public class AssetBundleUtils  {

    static Texture2D mGradientTex;

    static public Texture2D blankTexture {
        get {
            return EditorGUIUtility.whiteTexture;
        }
    }
    static public Texture2D gradientTexture {
        get {
            if (mGradientTex == null) mGradientTex = CreateGradientTex();
            return mGradientTex;
        }
    }
    static public void DrawSeparator(float height,float lineHight) {
        GUILayout.Space(height);
        if (Event.current.type == EventType.Repaint) {
            Texture2D tex = blankTexture;
            Rect rect = GUILayoutUtility.GetLastRect();
            GUI.color = new Color(0f, 0f, 0f, 0.25f);
            float indexY = (height - lineHight) / 2 + 2;
            GUI.DrawTexture(new Rect(0f, rect.yMin + indexY, Screen.width, lineHight), tex);
            GUI.DrawTexture(new Rect(0f, rect.yMin + indexY, Screen.width, 1f), tex);
            GUI.DrawTexture(new Rect(0f, rect.yMin + indexY + lineHight - 1, Screen.width, 1f), tex);
            GUI.color = Color.white;
        }
    }
    static Texture2D CreateGradientTex() {
        Texture2D tex = new Texture2D(1, 16);
        tex.name = "[Generated] Gradient Texture";
        tex.hideFlags = HideFlags.DontSave;

        Color c0 = new Color(1f, 1f, 1f, 0f);
        Color c1 = new Color(1f, 1f, 1f, 0.6f);

        for (int i = 0; i < 16; ++i) {
            float f = Mathf.Abs((i / 15f) * 2f - 1f);
            f *= f;
            tex.SetPixel(0, i, Color.Lerp(c0, c1, f));
        }

        tex.Apply();
        tex.filterMode = FilterMode.Bilinear;
        return tex;
    }
    static public void HighlightLine(Color c, float height) {
        Rect rect = GUILayoutUtility.GetRect(Screen.width - 16f, height);
        GUILayout.Space(-height);
        c.a *= 0.3f;
        GUI.color = c;
        GUI.DrawTexture(rect, gradientTexture);
        c.r *= 0.5f;
        c.g *= 0.5f;
        c.b *= 0.5f;
        GUI.color = c;
        GUI.DrawTexture(new Rect(rect.x, rect.y + 1f, rect.width, 1f), blankTexture);
        GUI.DrawTexture(new Rect(rect.x, rect.y + rect.height - 1f, rect.width, 1f), blankTexture);
        GUI.color = Color.white;
    }
    /// <summary>
    /// 渐变
    /// </summary>
    /// <param name="c"></param>
    /// <param name="height"></param>
    static public void HighlightLine(Color c,Rect rect) {
        GUILayout.Space(-rect.height);
        c.a *= 0.3f;
        GUI.color = c;
        GUI.DrawTexture(rect, gradientTexture);
        c.r *= 0.5f;
        c.g *= 0.5f;
        c.b *= 0.5f;
        GUI.color = c;
        GUI.DrawTexture(new Rect(rect.x, rect.y + 1f, rect.width, 1f), blankTexture);
        GUI.DrawTexture(new Rect(rect.x, rect.y + rect.height - 1f, rect.width, 1f), blankTexture);
        GUI.color = Color.white;
    }
    /// <summary>
    /// 高光
    /// </summary>
    /// <param name="c"></param>
    /// <param name="rect"></param>
    static public void HighBackgroud(Color c, Rect rect) {
        GUI.color = c;
        GUI.DrawTexture(rect, blankTexture);
        GUI.color = new Color(0f, 0f, 0f, 0.25f);
        GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, 4f), blankTexture);
        GUI.DrawTexture(new Rect(rect.x, rect.y, 4f, rect.height), blankTexture);
        GUI.DrawTexture(new Rect(rect.x, rect.y + rect.height - 1f, rect.width, 4f), blankTexture);
        GUI.DrawTexture(new Rect(rect.x + rect.width, rect.y, 4f, rect.height + 3f), blankTexture);
        GUI.color = Color.white;
    }

    public static string GetFullName(string path) {
        return System.IO.Path.GetFileName(path);
    }
    public static string GetName(string fullName) {
        string[] fulls = fullName.Split('.');
        return fulls[0];
    }
    public static string GetType(string fullName) {
        string[] fulls = fullName.Split('.');
        return fulls[1];
    }
}
