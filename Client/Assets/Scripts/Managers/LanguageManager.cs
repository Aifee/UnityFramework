using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 多语言处理
/// </summary>
public class LanguageManager : Singleton<LanguageManager> {
    static readonly private string LANGUAGE_CONFIG = "LanguageConfig";
    static readonly private string PARAMETER_CONFIG = "ParameterConfig";

    private Dictionary<string, LanguageConfig> _languages;

    private SystemLanguage systemLanguage;
    public LanguageManager() {
        systemLanguage = Application.systemLanguage;
    }
    /// <summary>
    /// 获取多语言配置表字段
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GetLanguage(string id) {
        if (_languages == null) {
            LoadLanguageConfig();
        }
        LanguageConfig config = _languages.ContainsKey(id) ? _languages[id] : null;
        string str = "";
        
        if (config == null) {
            str = id.ToString();
        } else {
            switch (systemLanguage) {
                case SystemLanguage.Chinese:
                case SystemLanguage.ChineseSimplified: 
                    str = config.cn; break;
                case SystemLanguage.English: str = config.en; break;
            }
        }
        return str;
    }
    private void LoadLanguageConfig() {
        _languages = new Dictionary<string, LanguageConfig>();
        string text = ResourcesManager.Instance.LoadConfig(LANGUAGE_CONFIG);
        string[][] configArray = AssetToArray(text);
        if (configArray == null) {
            Debug.LogError("Data format error, data line number must be at least 2 lines");
            return ;
        }
        string[] titleNames = configArray[0];
        int rowCount = configArray.Length;
        for (int i = 2; i < rowCount; i++) {
            string[] column = configArray[i];
            int columnCount = column.Length;
            string[] values = configArray[i];
            LanguageConfig config = new LanguageConfig();
            config.id = values[0];
            config.cn = values[1];
            config.en = values[2];
            config.tw = values[3];
            _languages.Add(config.id, config);
        }
    }
    private string[][] AssetToArray(string assetText) {
        string[] rows = assetText.Split(new char[] { '\n' });
        int rowCount = rows.Length;
        if (rowCount < 2) {
            return null;
        }
        string[][] array = new string[rowCount][];
        for (int i = 0; i < rowCount; i++) {
            string[] column = rows[i].Split(new char[] { '|' });
            int columnCount = column.Length;
            array[i] = new string[columnCount];
            for (int j = 0; j < columnCount; j++) {
                array[i][j] = StringUtil.ReqularTerminator(column[j]);
            }
        }
        return array;
    }

         
}
