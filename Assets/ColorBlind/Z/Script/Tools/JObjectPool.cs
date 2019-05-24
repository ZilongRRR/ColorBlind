using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabManage {
    public string Name;
    public PrefabSetting[] prefabSetting = new PrefabSetting[1];
}

[System.Serializable]
public class PrefabSetting {
    /// <summary>
    /// The Prefab being managed by this pool
    /// </summary>
    public GameObject Prefab;
    /// <summary>
    /// Automatically prepare when the game starts. If false, you'll need to call InstantiateObject() by yourself
    /// </summary>
    public bool AutoGenerate = true;
    /// <summary>
    /// Pool size
    /// </summary>
    public int initailSize;
    /// <summary>
    /// The Prefab name
    /// </summary>
    public string PrefabName {
        get {
            return Prefab.name;
        }
    }
    /// <summary>
    /// Name of the Pool
    /// </summary>
    public string PoolName;
}

public class JObjectPool : MonoBehaviour {

    public static JObjectPool Instance = null;
    [Header ("Singleton Parameter")]
    /// <summary>
    /// Keep persistent between scene changes
    /// </summary>
    public bool IsDontdestroyonload;
    /// <summary>
    /// Whether certain actions should be logged
    /// </summary>
    public bool DebugMode;
    /// <summary>
    /// Delete "(CLONE)" in all prefabs' name
    /// </summary>
    public bool HideClone;
    [Header ("Prefab Manage Setting")]
    /// <summary>
    /// Set your Prefab in PrefabManage
    /// </summary>
    public PrefabManage[] m_PrefabManage;

    public Dictionary<string, List<GameObject>> m_Dictionary = new Dictionary<string, List<GameObject>> ();
    public Dictionary<string, GameObject> m_DictionaryForParent = new Dictionary<string, GameObject> ();
    private void Awake () {
        if (IsDontdestroyonload)
            DontDestroyOnLoad (this.gameObject);

        //Check if there is already an instance of JObjectPool
        if (Instance == null)
            Instance = this;
        //If _InstanceJObjectPool already exists ,Destroy this 
        //this enforces our singleton pattern so there can only be one instance of JObjectPool.
        else if (Instance != this)
            Destroy (this.gameObject);

        Init ();
    }

    #region private function

    GameObject GetPrefabByName (string s) {
        if (m_Dictionary.ContainsKey (s)) {
            foreach (var item in m_PrefabManage) {
                for (int i = 0; i < item.prefabSetting.Length; i++) {
                    if (item.prefabSetting[i].PrefabName == s)
                        return item.prefabSetting[i].Prefab;
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Instantiate Gameobject in m_PrefabSetting into JObject pool if IsAutoGenerate is true
    /// </summary>
    /// <param name="s"></param>
    void Init () {
        ShowDebug ("Start Initialize JObjectPool");
        GameObject JPool = new GameObject ();
        JPool.name = "---JObject pool---";
        foreach (var item in m_PrefabManage) {
            for (int i = 0; i < item.prefabSetting.Length; i++) {
                if (item.prefabSetting[i].AutoGenerate) {
                    GameObject tempParent = new GameObject ();
                    tempParent.transform.SetParent (JPool.transform);
                    if (item.prefabSetting[i].PoolName != "")
                        tempParent.name = item.prefabSetting[i].PoolName;
                    else
                        tempParent.name = "[ " + item.prefabSetting[i].PrefabName + " ]";
                    List<GameObject> m_list = new List<GameObject> ();
                    for (int j = 0; j < item.prefabSetting[i].initailSize; j++) {
                        GameObject temp = (GameObject) Instantiate (item.prefabSetting[i].Prefab, tempParent.transform);
                        temp.SetActive (false);
                        if (HideClone)
                            temp.name = item.prefabSetting[i].PrefabName;
                        m_list.Add (temp);
                    }
                    m_Dictionary.Add (item.prefabSetting[i].PrefabName, m_list);
                    m_DictionaryForParent.Add (item.prefabSetting[i].PrefabName, tempParent);
                }
            }
        }
        ShowDebug ("Finish Initialize JObjectPool");
    }

    /// <summary>
    /// Debug something if DebugMode is true
    /// </summary>
    /// <param name="s"></param>
    void ShowDebug (string s) {
        if (DebugMode)
            Debug.Log (s);
    }

    /// <summary>
    /// Debug Error something if DebugMode is true
    /// </summary>
    /// <param name="s"></param>
    void ShowError (string s) {
        if (DebugMode)
            Debug.LogError ("[ " + s + " ]");

    }
    #endregion

    #region public function
    /// <summary>
    /// Get gameobject in JObject pool
    /// </summary>
    /// <param name="s"></param>
    public GameObject GetGameObject (string s, Vector3 pos, Quaternion qua) {
        if (s == null)
            return null;
        //尋找物件池裡有沒有這個名字的物件
        if (m_Dictionary.ContainsKey (s)) {
            for (int i = 0; i < m_Dictionary[s].Count; i++) {
                if (!m_Dictionary[s][i].activeSelf) {
                    m_Dictionary[s][i].transform.position = pos;
                    m_Dictionary[s][i].transform.rotation = qua;
                    m_Dictionary[s][i].SetActive (true);
                    ShowDebug (s + " is found");
                    return m_Dictionary[s][i];
                }
            }
            //如果都被使用的話，新創10個物件進入物件池。最後回傳最後一個物件
            ShowDebug ("Ur JObject pool is all uesd , now instantiate new 10 objects");
            for (int i = 0; i < 9; i++) {
                GameObject temp = (GameObject) Instantiate (GetPrefabByName (s), m_DictionaryForParent[s].transform);
                temp.SetActive (false);
                if (HideClone)
                    temp.name = m_Dictionary[s][0].name;
                m_Dictionary[s].Add (temp);
            }
            GameObject temp1 = (GameObject) Instantiate (GetPrefabByName (s), m_DictionaryForParent[s].transform);
            temp1.transform.position = pos;
            temp1.transform.rotation = qua;
            temp1.SetActive (true);
            if (HideClone)
                temp1.name = m_Dictionary[s][0].name;
            m_Dictionary[s].Add (temp1);
            return temp1;
        }
        //如果沒有這個物件在物件池，就回傳 null
        else {
            ShowError (s + " doesn't exist in JObject pool");
            return null;
        }
    }
    /// <summary>
    /// Get gameobject in JObject pool
    /// </summary>
    /// <param name="s"></param>
    public GameObject GetGameObject (string s, Vector3 pos) {
        return GetGameObject (s, pos, Quaternion.identity);
    }
    /// <summary>
    /// Get gameobject in JObject pool and set parnet
    /// </summary>
    /// <returns></returns>
    public GameObject GetGameObject (string s, Vector3 pos, Quaternion qua, Transform t) {
        GameObject tempObj = GetGameObject (s, pos, qua);
        tempObj.transform.SetParent (t);
        return tempObj;
    }

    /// <summary>
    /// Get gameobject in JObject pool and set parnet
    /// </summary>
    /// <returns></returns>
    public GameObject GetGameObject (string s, Vector3 pos, Transform t) {
        return GetGameObject (s, pos, Quaternion.identity, t);
    }

    /// <summary>
    /// Recovery Gameobject and set its position to (0,0,0)
    /// </summary>
    /// <param name="s"></param>
    public void Recovery (GameObject g) {
        Recovery (g, Vector3.zero);
    }
    /// <summary>
    /// Recovery Gameobject and set its position
    /// </summary>
    /// <param name="s"></param>
    public void Recovery (GameObject g, Vector3 pos) {
        string s = g.name;
        if (s.Contains ("Clone")) {
            s = s.Replace ("(Clone)", "");
        }
        if (m_Dictionary.ContainsKey (s)) {
            g.SetActive (false);
            g.transform.position = pos;
            g.transform.SetParent (m_DictionaryForParent[s].transform);
        } else
            ShowError (s + " doesn't exist in JObject pool");
    }

    /// <summary>
    /// Recovery Certain Gameobjects and set their position to (0,0,0)
    /// </summary>
    /// <param name="s"></param>
    /// <param name="pos"></param>
    public void RecoveryCertainObj (string s) {
        RecoveryCertainObj (s, Vector3.zero);
    }

    /// <summary>
    /// Recovery Certain Gameobjects and set their position
    /// </summary>
    /// <param name="s"></param>
    /// <param name="pos"></param>
    public void RecoveryCertainObj (string s, Vector3 pos) {
        if (m_Dictionary.ContainsKey (s)) {
            for (int i = 0; i < m_Dictionary[s].Count; i++) {
                m_Dictionary[s][i].SetActive (false);
                m_Dictionary[s][i].transform.position = pos;
                m_Dictionary[s][i].transform.SetParent (m_DictionaryForParent[s].transform);
            }
        } else
            ShowError (s + " doesn't exist in JObject pool");
    }

    /// <summary>
    /// Instantiate Objects into JObject pool , the parameter is the prefab's name 
    /// </summary>
    /// <param name="s"></param>
    public void InstantiateObject (string s) {
        int _arrayi = -1;
        int _arrayj = -1;
        for (int i = 0; i < m_PrefabManage.Length; i++) {
            for (int j = 0; j < m_PrefabManage[i].prefabSetting.Length; j++) {
                if (m_PrefabManage[i].prefabSetting[j].PrefabName == s) {
                    _arrayi = i;
                    _arrayj = j;
                }
            }
        }
        if (_arrayj < 0)
            return;

        if (m_Dictionary.ContainsKey (s)) {
            for (int j = 0; j < m_PrefabManage[_arrayi].prefabSetting[_arrayj].initailSize; j++) {
                GameObject temp = (GameObject) Instantiate (m_PrefabManage[_arrayi].prefabSetting[_arrayj].Prefab, m_DictionaryForParent[s].transform);
                temp.SetActive (false);
                if (HideClone)
                    temp.name = m_PrefabManage[_arrayi].prefabSetting[_arrayj].PrefabName;
                m_Dictionary[s].Add (temp);
            }
        } else {
            GameObject tempParent = new GameObject ();
            tempParent.name = m_PrefabManage[_arrayi].prefabSetting[_arrayj].PoolName;
            List<GameObject> m_list = new List<GameObject> ();
            for (int j = 0; j < m_PrefabManage[_arrayi].prefabSetting[_arrayj].initailSize; j++) {
                GameObject temp = (GameObject) Instantiate (m_PrefabManage[_arrayi].prefabSetting[_arrayj].Prefab, tempParent.transform);
                temp.SetActive (false);
                if (HideClone)
                    temp.name = m_PrefabManage[_arrayi].prefabSetting[_arrayj].PrefabName;
                m_list.Add (temp);
            }
            m_Dictionary.Add (m_PrefabManage[_arrayi].prefabSetting[_arrayj].PrefabName, m_list);
            m_DictionaryForParent.Add (m_PrefabManage[_arrayi].prefabSetting[_arrayj].PrefabName, tempParent);
        }
    }
    #endregion

}