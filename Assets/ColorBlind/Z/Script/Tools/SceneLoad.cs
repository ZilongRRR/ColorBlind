using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZTools;

public class SceneLoad : Singleton<SceneLoad> {

    [Header ("SceneLoad Parameter")]
    public string SceneName;

    void Start () {

    }

    void Update () {
        /*
        if (Input.GetKeyDown(KeyCode.S))
            Load(SceneName);
        */

    }
    public void Load (string s, LoadSceneMode mode = LoadSceneMode.Single) {
        SceneManager.LoadScene (s, mode);
    }
    public void Load (string s) {
        Load (s, LoadSceneMode.Single);
    }
}