using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;
using ZTools;
public class LeaderBoardControl : MonoBehaviour {
    private float last_time = 0f;
    private const float time_out = 200f;
    public GameObject rank_template;
    public GameObject rankParent;
    FireBaseConnect database = null;
    public Text statusText;
    public Image loadingFigure;
    public Image finishFigure;
    public Image errorFigure;
    private List<string> user_record = new List<string> ();
    public GameObject current_rank;
    public GameObject content;
    public Button cancelBtn;

    private void Start () {
        ReadUserRecord ();
        InitLeaderBoard ();
        cancelBtn.onClick.AddListener (CloseLeaderBoard);
    }

    void SaveUserRecord () {
        string record = "";
        for (int i = 0; i < user_record.Count; i++) {
            record += user_record[i];
            if ((i + 1) < user_record.Count) {
                record += ",";
            }
        }
        PlayerPrefs.SetString ("UserRecord", record);
    }
    void ReadUserRecord () {
        if (!PlayerPrefs.HasKey ("UserRecord")) {
            user_record = new List<string> ();
            return;
        }
        string record = PlayerPrefs.GetString ("UserRecord");
        string[] recordArray = record.Split (',');
        foreach (var item in recordArray) {
            user_record.Add (item);
        }
    }

    public void CloseLeaderBoard () {
        content.SetActive (false);
    }
    public void InitLeaderBoard () {
        if (database == null) {
            database = new FireBaseConnect ();
            database.OnReadDataFinish += UpdateValue;
            database.OnShowStatus += ShowStatus;
            database.Connection ();
            database.InitReadDataEvent ();
        }
    }
    public void ShowLeaderBoard () {
        this.gameObject.SetActive (true);
    }
    private void ShowStatus (string status) {
        // Debug.Log("The time of " + status + " is " + Time.time);
        // last_time = Time.time;
        // 幾秒後漸淡
        // loadingFigure.gameObject.SetActive(false);
        switch (status) {
            case FirebaseStatus.CONNECTING:
                loadingFigure.gameObject.SetActive (true);
                statusText.text = status;
                break;
            case FirebaseStatus.CONNECT_FINISH:
                loadingFigure.gameObject.SetActive (false);
                finishFigure.gameObject.SetActive (true);
                statusText.text = status;
                break;
            case FirebaseStatus.CONNECT_FAILED:
                loadingFigure.gameObject.SetActive (false);
                errorFigure.gameObject.SetActive (true);
                statusText.text = status;
                break;
            case FirebaseStatus.DATA_READING:
                finishFigure.gameObject.SetActive (false);
                loadingFigure.gameObject.SetActive (true);
                statusText.text = status;
                break;
            case FirebaseStatus.DATA_READING_FAILED:
                loadingFigure.gameObject.SetActive (false);
                errorFigure.gameObject.SetActive (true);
                statusText.text = status;
                break;
            case FirebaseStatus.NO_CONNECT:
                statusText.text = status;
                break;
            case FirebaseStatus.UPDATE_DATA:
                errorFigure.gameObject.SetActive (false);
                statusText.text = "";
                break;
        }
    }

    private void UpdateValue (List<Rank> rank) {
        foreach (Transform child in rankParent.transform) {
            Destroy (child.gameObject);
        }
        // Debug.Log(rank.Count);
        // Debug.Log(user_record.ToString());
        for (int i = 1; i <= rank.Count; i++) {
            try {
                Rank r = rank[rank.Count - i];
                GameObject rankObject = Instantiate (rank_template);
                rankObject.transform.SetParent (rankParent.transform, false);
                rankObject.GetComponent<RankEntity> ().FillRankUIValue (i, r.username, r.score);
                if (user_record.Contains (r.key)) {
                    if (user_record[user_record.Count - 1] == r.key) {
                        current_rank.GetComponent<RankEntity> ().FillRankUIValue (i, r.username, r.score);
                    }
                    rankObject.GetComponent<RankEntity> ().HighLight ();
                }
            } catch (Exception e) {
                Debug.Log (e);
            }
        }
    }
    public void OpenUI (bool toshow = false) {
        content.SetActive (true);
        current_rank.SetActive (toshow);
    }
    public void UploadRecord (string usr_name, long score) {
        if (database != null) {
            string database_key = database.AddScoreToLeaders (usr_name, score);
            user_record.Add (database_key);
            SaveUserRecord ();
            OpenUI (true);
        } else {
            NotificationManager.Instance.DoNotificationAndFade ("連線失敗");
        }
    }
    // Update is called once per frame
    void Update () { }
}