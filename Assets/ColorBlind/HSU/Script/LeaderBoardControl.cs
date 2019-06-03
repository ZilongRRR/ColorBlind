using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardControl : MonoBehaviour
{
    private float last_time = 0f;
    private const float time_out = 200f;
    public GameObject rank_template;
    public GameObject rankParent;
    FireBaseConnect database = null;
    public Text statusText;
    public Image loadingFigure;
    public Image finishFigure;
    public Image errorFigure;
    private List<string> user_record = new List<string>();
    // Start is called before the first frame update
    public void InitLeaderBoard()
    {
        if (database == null)
        {
            database = new FireBaseConnect();
            database.OnReadDataFinish += UpdateValue;
            database.OnShowStatus += ShowStatus;
            database.Connection();
            database.InitReadDataEvent();
        }
    }
    public void ShowLeaderBoard()
    {
        this.gameObject.SetActive(true);
    }
    private void ShowStatus(string status)
    {
        Debug.Log("The time of " + status + " is " + Time.time);
        last_time = Time.time;
        // 幾秒後漸淡
        // loadingFigure.gameObject.SetActive(false);
        switch (status)
        {
            case FirebaseStatus.CONNECTING:
                loadingFigure.gameObject.SetActive(true);
                statusText.text = status;
                break;
            case FirebaseStatus.CONNECT_FINISH:
                loadingFigure.gameObject.SetActive(false);
                finishFigure.gameObject.SetActive(true);
                statusText.text = status;
                break;
            case FirebaseStatus.CONNECT_FAILED:
                loadingFigure.gameObject.SetActive(false);
                errorFigure.gameObject.SetActive(true);
                statusText.text = status;
                break;
            case FirebaseStatus.DATA_READING:
                finishFigure.gameObject.SetActive(false);
                loadingFigure.gameObject.SetActive(true);
                statusText.text = status;
                break;
            case FirebaseStatus.DATA_READING_FAILED:
                loadingFigure.gameObject.SetActive(false);
                errorFigure.gameObject.SetActive(true);
                statusText.text = status;
                Debug.Log("You are reading fail nerd");
                break;
            case FirebaseStatus.NO_CONNECT:
                Debug.Log("You are nerd");
                statusText.text = status;
                break;
            case FirebaseStatus.UPDATE_DATA:
                Debug.Log("You are updating nerd");
                statusText.text = status;
                break;
        }
    }

    private void UpdateValue(List<Rank> rank)
    {
        foreach (Transform child in rankParent.transform)
        {
            Destroy(child.gameObject);
        }
        Debug.Log(rank.Count);
        Debug.Log(user_record.ToString());
        for (int i = 1; i <= rank.Count; i++)
        {
            try
            {
                Rank r = rank[rank.Count - i];
                GameObject rankObject = Instantiate(rank_template);
                rankObject.transform.SetParent(rankParent.transform, false);
                // rankObject.transform.localPosition = new Vector3(0f, 800f - 150f * i, 0f);
                rankObject.GetComponent<RankEntity>().FillRankUIValue(i, r.username, r.score);
                // Text rankObject_Order = rankObject.transform.Find("Order").gameObject.GetComponent<Text>();
                // rankObject_Order.text = "" + (i);
                // Text rankObject_Name = rankObject.transform.Find("Name").gameObject.GetComponent<Text>();
                // rankObject_Name.text = r.username;
                // Text rankObject_Score = rankObject.transform.Find("Score").gameObject.GetComponent<Text>();
                // rankObject_Score.text = r.score;
                Debug.Log(r.key);
                if (user_record.Contains(r.key))
                {
                    rankObject.GetComponent<RankEntity>().HighLight();
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            System.Random rnd = new System.Random();
            int random_score = rnd.Next(100, 2000);
            string database_key = database.AddScoreToLeaders("ABCD", random_score);
            user_record.Add(database_key);
        }
        // if (Time.time - last_time > time_out)
        // {
        //     ShowStatus(FirebaseStatus.DATA_READING_FAILED);
        // }
    }
}