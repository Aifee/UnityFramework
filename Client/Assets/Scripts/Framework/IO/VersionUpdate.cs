using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public delegate void VersionUpdateHandler ();
public delegate void VersionUpdateError(string error);

public class VersionUpdate : SingletonComponent<VersionUpdate> {
    public const string RVL = "DataVersionConfig.xml";
    public VersionUpdateHandler UpdateComplete;
    public VersionUpdateError UpdateErrorEvent;
    public VersionUpdateHandler OnUpdatingEvent;
    public long TotalSize { get; private set; }
    public int UpdatedSize { get; private set; }
    public int UpdatedResNumber { get; private set; }
    public int UpdateTotalNumber { get; private set; }
    private string LocalResRootPath = null;
    private string ServerResRootPath = null;
    private string LocalVersion = "";
    private string ServerVersion = "";
    private Dictionary<string, VersionData> LocalRVLDic = null;
    private Dictionary<string, VersionData> ServerRVLDic = null;
    private Queue<VersionData> UpdateResList = new Queue<VersionData>();
    private Queue<VersionData> RemoveQueue = new Queue<VersionData>();
    
    void Start() {
        LocalResRootPath = Application.persistentDataPath;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        LocalResRootPath = Application.dataPath + "/../AssetBundle/Test";
#elif UNITY_IPHONE
        LocalResRootPath = Application.persistentDataPath;
#elif UNITY_ANDROID
        LocalResRootPath = Application.persistentDataPath;
#endif
    }
    public void SetResServerUrl(string url) {
        ServerResRootPath = url;
    }
    public void CheckUpdater() {
        if (ServerResRootPath == null) {
            Debug.LogError("Resources Server Url is null...");
            return;
        }
        UpdatedSize = 0;
        TotalSize = 0;
        UpdatedResNumber = 0;
        UpdateTotalNumber = 0;
        StartCoroutine(OnUpdateVersionRes());
    }
    private IEnumerator OnUpdateVersionRes() {
        string localPath = LocalResRootPath + "/" + RVL;
        string serverPath = ServerResRootPath + "/" + RVL;

        yield return StartCoroutine(ParserServerConfig());
    }

    private IEnumerator ParserServerConfig() {
        string localPath = LocalResRootPath + "/" + RVL;
        string serverPath = ServerResRootPath + "/" + RVL;
        Debug.Log("serverPath = " + serverPath);
        Debug.Log("localPath = " + localPath);
        WWW www = new WWW(serverPath);
        yield return www;
        if (www.error == null) {
            Debug.Log(www.text);
            ServerRVLDic = ParserResVersionList(www.text, ref ServerVersion);
            if (!File.Exists(localPath)) {
                SaveVersion(www.text);
                LocalRVLDic = ParserResVersionList(www.text, ref LocalVersion);
                foreach (VersionData data in LocalRVLDic.Values) {
                    TotalSize += data.Size;
                    UpdateTotalNumber++;
                    UpdateResList.Enqueue(data);
                }
            } else {
                yield return StartCoroutine(ParserLocalConfig());
                SaveVersion(www.text);
            }
            UpdateTotalNumber = UpdateResList.Count;
            //downLoad
            RemoveAsset();
        } else {
            Debug.Log(www.error);
        }
        www.Dispose();
    }
    private IEnumerator ParserLocalConfig() {
        string localPath = LocalResRootPath + "/" + RVL;
        WWW www = new WWW(localPath);
        yield return www;
        if (www.error == null) {
            LocalRVLDic = ParserResVersionList(www.text, ref LocalVersion);
            ComparisonAsset();
        } else {
            Debug.Log(www.error);
        }
        www.Dispose();
    }
         
    private void SaveVersion(string text) {
        string fileName = LocalResRootPath + "/" + RVL;
        FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate);
        StreamWriter st = new StreamWriter(fs);
        st.Write(text);
        st.Close();
        fs.Close();
    }
    private Dictionary<string, VersionData> ParserResVersionList(string xml, ref string version) {
        XmlDocument document = new XmlDocument();
        document.LoadXml(xml);

        Dictionary<string, VersionData> resVersionList = new Dictionary<string, VersionData>();

        XmlNode root = document.SelectSingleNode("files");
        XmlElement rootNode = root as XmlElement;
        if (rootNode != null) {
            version = rootNode.GetAttribute("version");
        }
        Debug.Log("root = " + root.ChildNodes.Count);
        foreach (XmlNode _node in root.ChildNodes) {
            XmlElement node = _node as XmlElement;
            if (node == null) { continue; }
            if (node.Name == "Item") {
                VersionData loadAsset = new VersionData();
                loadAsset.Md5 = node.GetAttribute("file");
                loadAsset.Size = System.Convert.ToInt64(node.GetAttribute("size"));
                resVersionList.Add(loadAsset.Md5, loadAsset);
            }
        }
        return resVersionList;
    }

    private void ComparisonAsset() {
        List<string> common = new List<string>();
        List<string> update = new List<string>();
        foreach (KeyValuePair<string, VersionData> kvp in ServerRVLDic) {
            if (LocalRVLDic.ContainsKey(kvp.Key)) {
                common.Add(kvp.Key);
            } else {
                update.Add(kvp.Key);
                
            }
        }
        foreach (KeyValuePair<string, VersionData> kvp in LocalRVLDic) {
            if (!common.Contains(kvp.Key) && !update.Contains(kvp.Key)) {
                RemoveQueue.Enqueue(kvp.Value);
            }
        }
        if (update.Count > 0) {
            foreach (string key in update) {
                VersionData data = ServerRVLDic[key];
                TotalSize += data.Size;
                UpdateTotalNumber++;
                UpdateResList.Enqueue(data);
            }
        }
    }

    private void RemoveAsset() {
        Debug.Log("RemoveQueue.Count == " + RemoveQueue.Count);
        while (RemoveQueue.Count > 0) {
            VersionData data = RemoveQueue.Dequeue();
            string path = string.Format("{0}/Data/{1}", LocalResRootPath, data.Md5);
            if (System.IO.File.Exists(path)) {
                System.IO.File.Delete(path);
            }
        }
        StartDownLoad();
    }
    private void StartDownLoad() {
        if (UpdateResList.Count > 0) {
            VersionData data = UpdateResList.Dequeue();
            StartCoroutine(DownLoadVersionData(data));
        } else if (UpdateComplete != null) {
            UpdateComplete();
        }
    }
    private IEnumerator DownLoadVersionData(VersionData data) {
        string url = string.Format("{0}/Data/{1}", ServerResRootPath, data.Md5);
        WWW www = new WWW(url);
        yield return www;
        if (www.error == null) {
            string localPath = string.Format("{0}/Data/{1}", LocalResRootPath, data.Md5);
            FileInfo t = new FileInfo(localPath);
            if (t.Exists) {
                File.Delete(localPath);
            }
            try {
                if (!t.Exists) {
                    Directory.CreateDirectory(t.DirectoryName);
                }

                File.WriteAllBytes(localPath, www.bytes);
                UpdatedSize += www.size;
                UpdatedResNumber++;
                if (OnUpdatingEvent != null)
                    OnUpdatingEvent();
                StartDownLoad();
            } catch (System.Exception ex) {
                Debug.LogError(ex.ToString());
            }
        } else {
            Debug.Log(www.error);
        }
    }
}
