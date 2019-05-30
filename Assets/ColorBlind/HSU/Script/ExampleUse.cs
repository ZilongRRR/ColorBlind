using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;

public class ExampleUse : MonoBehaviour
{
    public GameObject rank_template;
    public GameObject mainCanvas;
    FireBaseConnect database;
    // Start is called before the first frame update
    void Awake()
    {
        database = new FireBaseConnect();
        database.OnReadDataFinish += UpdateValue;
        database.ReadData();
    }

    void Start()
    {

    }
    private void UpdateValue(List<Rank> rank)
    {
        foreach (Transform child in mainCanvas.transform)
        {
            Destroy(child.gameObject);
        }
        Debug.Log(rank.Count);
        for (int i = 1; i <= rank.Count; i++)
        {
            try
            {
                Rank r = rank[rank.Count - i];
                GameObject rankObject = Instantiate(rank_template);
                rankObject.transform.SetParent(mainCanvas.transform, false);
                // rankObject.transform.localPosition = new Vector3(0f, 800f - 150f * i, 0f);
                rankObject.GetComponent<RankEntity>().FillRankUIValue(i, r.username, r.score);
                // Text rankObject_Order = rankObject.transform.Find("Order").gameObject.GetComponent<Text>();
                // rankObject_Order.text = "" + (i);
                // Text rankObject_Name = rankObject.transform.Find("Name").gameObject.GetComponent<Text>();
                // rankObject_Name.text = r.username;
                // Text rankObject_Score = rankObject.transform.Find("Score").gameObject.GetComponent<Text>();
                // rankObject_Score.text = r.score;
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
            Debug.Log(database.AddScoreToLeaders("Randy", random_score));
        }
    }
}