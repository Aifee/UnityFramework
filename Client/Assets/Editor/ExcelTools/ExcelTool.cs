//----------------------------------------------
//            MobArts PiDan Project
// Copyright © 2010-2015 MobArts Entertainment
// Created by : Liu Aifei (329737941@qq.com)
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Text;
using System.Xml;
using Excel;

public class ExcelTool : EditorWindow {

    /// <summary> excel文件目录 </summary>
    public static string PATH_EXCEL = "Assets/Configs/";
    /// <summary> 导出CS类目录 </summary>
    private static string PATH_CS = "Assets/Scripts/Configs/";
    /// <summary> 生成的data文件目录 </summary>
    public static string PATH_DATA = "Assets/Resources/Data/";
    /// <summary> 工具类对象实例 </summary>
    public static ExcelTool instance;
    /// <summary> 路径是否允许编辑 </summary>
    private bool pathEditor = true;
    //private ConfigManager.ConfigType configType;
    /// <summary> 加载excel文件类型 </summary>
    private List<string> ExcelType = new List<string>(new string[]{".xlsx",".xls"});


    //private string ValueTypeToString(ConfigManager.PropertiesType type) {
    //    string ret = null;
    //    switch (type) {
    //        case ConfigManager.PropertiesType.Unknown: break;
    //        case ConfigManager.PropertiesType.INT: ret = "int"; break;
    //        case ConfigManager.PropertiesType.FLOAT: ret = "float"; break;
    //        case ConfigManager.PropertiesType.STR: ret = "string"; break;
    //        case ConfigManager.PropertiesType.LIST_INT: ret = "List<int>"; break;
    //        case ConfigManager.PropertiesType.LIST_STR: ret = "List<string>"; break;
    //        case ConfigManager.PropertiesType.LIST_FLOAT: ret = "List<float>"; break;
    //        case ConfigManager.PropertiesType.LIST_DICT_INT_INT: ret = "List<Dictionary<int,int>>"; break;
    //        case ConfigManager.PropertiesType.LIST_DICT_INT_STR: ret = "List<Dictionary<int,string>>"; break;
    //        case ConfigManager.PropertiesType.LIST_DICT_INT_FLOAT: ret = "List<Dictionary<int,float>>"; break;
    //        case ConfigManager.PropertiesType.LIST_DICT_STR_STR: ret = "List<Dictionary<string,string>>"; break;
    //        case ConfigManager.PropertiesType.LIST_DICT_STR_INT: ret = "List<Dictionary<string,int>>"; break;
    //        case ConfigManager.PropertiesType.LIST_DICT_STR_FLOAT: ret = "List<Dictionary<string,float>>"; break;
    //        case ConfigManager.PropertiesType.LIST_DICT_FLOAT_FLOAT: ret = "List<Dictionary<float,float>>"; break;
    //        case ConfigManager.PropertiesType.LIST_DICT_FLOAT_INT: ret = "List<Dictionary<float,int>>"; break;
    //        case ConfigManager.PropertiesType.LIST_DICT_FLOAT_STR: ret = "List<Dictionary<float,string>>"; break;
    //        case ConfigManager.PropertiesType.DICT_INT_INT: ret = "Dictionary<int,int>"; break;
    //        case ConfigManager.PropertiesType.DICT_INT_STR: ret = "Dictionary<int,string>"; break;
    //        case ConfigManager.PropertiesType.DICT_INT_FLOAT: ret = "Dictionary<int,float>"; break;
    //        case ConfigManager.PropertiesType.DICT_INT_LIST_INT: ret = "Dictionary<int,List<int>>"; break;
    //        case ConfigManager.PropertiesType.DICT_INT_LIST_STR: ret = "Dictionary<int,List<string>>"; break;
    //        case ConfigManager.PropertiesType.DICT_INT_LIST_FLOAT: ret = "Dictionary<int,List<float>>"; break;
    //        case ConfigManager.PropertiesType.DICT_STR_STR: ret = "Dictionary<string,string>"; break;
    //        case ConfigManager.PropertiesType.DICT_STR_INT: ret = "Dictionary<string,int>"; break;
    //        case ConfigManager.PropertiesType.DICT_STR_FLOAT: ret = "Dictionary<string,float>"; break;
    //        case ConfigManager.PropertiesType.DICT_STR_LIST_INT: ret = "Dictionary<string,List<int>>"; break;
    //        case ConfigManager.PropertiesType.DICT_STR_LIST_STR: ret = "Dictionary<string,List<string>>"; break;
    //        case ConfigManager.PropertiesType.DICT_STR_LIST_FLOAT: ret = "Dictionary<string,List<float>>"; break;
    //        case ConfigManager.PropertiesType.DICT_FLOAT_FLOAT: ret = "Dictionary<float,float>"; break;
    //        case ConfigManager.PropertiesType.DICT_FLOAT_INT: ret = "Dictionary<float,int>"; break;
    //        case ConfigManager.PropertiesType.DICT_FLOAT_STR: ret = "Dictionary<float,string>"; break;
    //        case ConfigManager.PropertiesType.DICT_FLOAT_LIST_INT: ret = "Dictionary<float,List<int>>"; break;
    //        case ConfigManager.PropertiesType.DICT_FLOAT_LIST_STR: ret = "Dictionary<float,List<string>>"; break;
    //        case ConfigManager.PropertiesType.DICT_FLOAT_LIST_FLOAT: ret = "Dictionary<float,List<float>>"; break;
    //    }
    //    return ret;
    //}


    private void ProcessExcel() {
        if (string.IsNullOrEmpty(PATH_EXCEL)) { return; }
        PATH_EXCEL = PATH_EXCEL.Replace('/', '\\');
        if (!Directory.Exists(PATH_EXCEL)) {
            Debug.Log(string.Format("{0},Directory does not exist", PATH_EXCEL));
            return;
        }
        List<string> excelFiles = new List<string>();
        string[] paths = Directory.GetFiles(PATH_EXCEL);
        foreach(string path in paths){
            string extension = Path.GetExtension(path).ToLower();
            if (ExcelType.Contains(extension)) {
                excelFiles.Add(path);
            }
        }
        ///遍历所有的配置文件
        foreach (string file in excelFiles) {
            FileStream stream = File.OpenRead(file);
            IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            DataSet data = reader.AsDataSet();
            DataTable table = data.Tables[0];
            if (table.Rows.Count < 3) {
                Debug.LogError("Please check the configuration table format is correct, at least 3 lines of data, the first line is the field, the second line is the field key, the third line is the field type");
                continue;
            }
            string csName = Path.GetFileNameWithoutExtension(file) + "Data";
            string excelData = Path.GetFileNameWithoutExtension(file) + "Config";
            CreateCSFile(csName, table, excelData);
            //if (configType == ConfigManager.ConfigType.Xml) {
            //    CreateXML(excelData,table);
            //} else if (configType == ConfigManager.ConfigType.Json) {
            //    CreateJson(excelData, table);
            //} else if (configType == ConfigManager.ConfigType.Csv) {
            //    CreateCsv(excelData, table);
            //}
        }

    }
    /// <summary>
    /// 生成CS类文件
    /// </summary>
    /// <param name="className"></param>
    /// <param name="voName"></param>
    /// <param name="dataName"></param>
    /// <param name="table"></param>
    public void CreateCSFile(string className,DataTable table,string excelData) {
        StringBuilder content = new StringBuilder();
        content.AppendLine("//----------------------------------------------");
        content.AppendLine("//    Automatically generate configuration file data class");
        content.AppendLine("//    all the game to the configuration file is to be processed by the!");
        content.AppendLine("//    Copyright © Aifei-Liu,!");
        content.AppendLine("//    Created by : Liu Aifei (329737941@qq.com)");
        content.AppendLine("//----------------------------------------------");
        content.AppendLine();
        content.AppendLine("using UnityEngine;");
        content.AppendLine("using System.Collections.Generic;");
        content.AppendLine();
        content.AppendLine(string.Format("public class {0} : Config {{", className));
        content.AppendLine();
        content.AppendLine(string.Format("    public {0}(){{", className));
        content.AppendLine(string.Format("          tableName = \"{0}\";", excelData));
        content.AppendLine("    }");
        int columns = table.Columns.Count;
        string[] explains = new string[columns];
        string[] fieldNames = new string[columns];
        //ConfigManager.PropertiesType[] fieldTypes = new ConfigManager.PropertiesType[columns];
        //DataRow rowExplain = table.Rows[0];
        //DataRow rowTitles = table.Rows[1];
        //DataRow rowTypes = table.Rows[2];
        //for (int i = 0; i < columns; i++) {
        //    explains[i] = rowExplain[i].ToString();
        //    fieldNames[i] = rowTitles[i].ToString();
        //    ConfigManager.PropertiesType valueType = ConfigManager.StringToValueType(rowTypes[i].ToString());
        //    if (valueType == ConfigManager.PropertiesType.Unknown) {
        //        Debug.LogError(string.Format("Unknown Value Type '{0}' !", rowTypes[i].ToString()));
        //        continue;
        //    }
        //    fieldTypes[i] = valueType;
        //    content.AppendLine("    /// <summary>");
        //    content.AppendLine(string.Format("    /// {0}", explains[i]));
        //    content.AppendLine("    /// <summary>");
        //    content.AppendLine(string.Format("    public {0} {1};", ValueTypeToString(fieldTypes[i]), fieldNames[i]));
        //}
        //content.AppendLine("}");

        //if (!Directory.Exists(PATH_CS)) {
        //    Directory.CreateDirectory(PATH_CS);
        //}

        File.WriteAllText(string.Concat(PATH_CS + "/", className, ".cs"), content.ToString(), Encoding.UTF8);
    }
    /// <summary>
    /// 生成xml文件
    /// </summary>
    /// <param name="xmlName"></param>
    /// <param name="table"></param>
    private void CreateXML(string xmlName, DataTable table) {
        XmlDocument doc = new XmlDocument();
        XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
        doc.AppendChild(dec);
        //创建一个根节点（一级）
        XmlElement root = doc.CreateElement(xmlName);
        doc.AppendChild(root);
        DataRow rowExplain = table.Rows[0];
        DataRow rowTitles = table.Rows[1];
        DataRow rowTypes = table.Rows[2];
        int columns = table.Columns.Count;
        int rowIndex = table.Rows.Count;
        for (int i = 3; i < rowIndex; i++) {
            XmlElement element = doc.CreateElement("Item");
            for (int j = 0; j < columns; j++) {
                //ConfigManager.PropertiesType valueType = ConfigManager.StringToValueType(rowTypes[j].ToString());
                //if (valueType == ConfigManager.PropertiesType.Unknown) {
                //    Debug.LogError(string.Format("Unknown Value Type '{0}' !", rowTypes[i].ToString()));
                //    continue;
                //}
                //root.SetAttribute(rowTitles[j].ToString(), rowTypes[j].ToString());
                //element.SetAttribute(rowTitles[j].ToString(), table.Rows[i][j].ToString());
            }
            root.AppendChild(element);
        }
        if (!Directory.Exists(PATH_DATA)) {
            Directory.CreateDirectory(PATH_DATA);
        }
        Debug.Log(doc.OuterXml);
        string xmlPath = string.Format(@"{0}/{1}.xml", PATH_DATA, xmlName);
        doc.Save(xmlPath);
    }
    /// <summary>
    /// 生成JSON文件
    /// </summary>
    /// <param name="jsonName"></param>
    /// <param name="table"></param>
    private void CreateJson(string jsonName, DataTable table) {
        Dictionary<string,object> jsonDic = new Dictionary<string,object>();
        Dictionary<string, object> typeDic = new Dictionary<string, object>();
        int columns = table.Columns.Count;
        int rowIndex = table.Rows.Count;
        DataRow rowExplain = table.Rows[0];
        DataRow rowTitles = table.Rows[1];
        DataRow rowTypes = table.Rows[2];
        for (int i = 0; i < columns; i++) {
            //ConfigManager.PropertiesType valueType = ConfigManager.StringToValueType(rowTypes[i].ToString());
            //if (valueType == ConfigManager.PropertiesType.Unknown) {
            //    Debug.LogError(string.Format("Unknown Value Type '{0}' !", rowTypes[i].ToString()));
            //    continue;
            //}
            //typeDic.Add(rowTitles[i].ToString(), rowTypes[i].ToString());
        }
        List<object> list = new List<object>();
        for (int i = 3; i < rowIndex; i++) {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            for (int j = 0; j < columns; j++) {
                //ConfigManager.PropertiesType valueType = ConfigManager.StringToValueType(rowTypes[j].ToString());
                //if (valueType == ConfigManager.PropertiesType.Unknown) {
                //    Debug.LogError(string.Format("Unknown Value Type '{0}' !", rowTypes[i].ToString()));
                //    continue;
                //}
                //dic.Add(rowTitles[j].ToString(), table.Rows[i][j].ToString());
            }
            list.Add(dic);
        }
        jsonDic.Add("Type",typeDic);
        jsonDic.Add("Data",list);
        //string json = MiniJSON.Json.Serialize(jsonDic);
        //json = System.Text.RegularExpressions.Regex.Unescape(json);
        //if (!Directory.Exists(PATH_DATA)) {
        //    Directory.CreateDirectory(PATH_DATA);
        //}
        //File.WriteAllText(string.Concat(PATH_DATA + "/", jsonName, ".json"), json, Encoding.UTF8);
    }
    /// <summary>
    /// 生成CSV文件
    /// </summary>
    /// <param name="csvName"></param>
    /// <param name="table"></param>
    private void CreateCsv(string csvName, DataTable table) {
        StringBuilder content = new StringBuilder();
        DataRow rowExplain = table.Rows[0];
        DataRow rowTitles = table.Rows[1];
        DataRow rowTypes = table.Rows[2];
        int columns = table.Columns.Count;
        int rowIndex = table.Rows.Count;
        for (int i = 0; i < rowIndex; i++) {
            string item = "";
            for (int j = 0; j < columns; j++) {
                //ConfigManager.PropertiesType valueType = ConfigManager.StringToValueType(rowTypes[j].ToString());
                //if (valueType == ConfigManager.PropertiesType.Unknown) {
                //    Debug.LogError(string.Format("Unknown Value Type '{0}' !", rowTypes[i].ToString()));
                //    continue;
                //}
                //item += table.Rows[i][j].ToString();
                //if (j < columns - 1)
                //    item += "|";
            }
            content.AppendLine(item);
        }
        if (!Directory.Exists(PATH_DATA)) {
            Directory.CreateDirectory(PATH_DATA);
        }
        File.WriteAllText(string.Concat(PATH_DATA + "/", csvName, ".csv"), content.ToString(), Encoding.UTF8);
    }

    void OnEnable() 
    {
        instance = this;
        //configType = ConfigManager.ConfigType.Xml;
    }
    void OnDisable() { instance = null; }

    void OnGUI() {
        GUILayout.Space(3f);
        GUILayout.BeginVertical();
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("excel files path:", GUILayout.Width(100f));
                EditorGUI.BeginDisabledGroup(pathEditor);
                PATH_EXCEL = GUILayout.TextArea(PATH_EXCEL);
                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button("Open Excel Folder", GUILayout.Width(120f))) {
                    PATH_EXCEL = EditorUtility.OpenFolderPanel("Select excel file path", PATH_EXCEL,"");
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("cs files path:", GUILayout.Width(100f));
                EditorGUI.BeginDisabledGroup(pathEditor);
                PATH_CS = GUILayout.TextArea(PATH_CS);
                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button("Open cs Folder", GUILayout.Width(120f))) {
                    PATH_CS = EditorUtility.OpenFolderPanel("Select cs file folder", PATH_CS, "");
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("data files path:", GUILayout.Width(100f));
                EditorGUI.BeginDisabledGroup(pathEditor);
                PATH_DATA = GUILayout.TextField(PATH_DATA);
                EditorGUI.EndDisabledGroup();
                //configType = (ConfigManager.ConfigType)EditorGUILayout.EnumPopup("DataType", configType, GUILayout.Width(200f));
                if (GUILayout.Button("Open data Folder", GUILayout.Width(120f))) {
                    PATH_DATA = EditorUtility.OpenFolderPanel("Select data file folder", PATH_DATA, "");
                }
            }
            GUILayout.EndHorizontal();
            EditorGUI.BeginDisabledGroup(!Directory.Exists(PATH_EXCEL) || !Directory.Exists(PATH_CS) || !Directory.Exists(PATH_DATA));
            if (GUILayout.Button("Start converting resources")) {
                ProcessExcel();
            }
            EditorGUI.EndDisabledGroup();
        }
        GUILayout.EndVertical();
        
    }



}
