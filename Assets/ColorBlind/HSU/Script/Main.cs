using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour {
    public GameObject rank_template;
    public GameObject mainCanvas;
    FireBaseConnect database;
    // Start is called before the first frame update
    void Awake () {
        database = new FireBaseConnect ();
        database.OnReadDataFinish += UpdateValue;
        database.ReadData ();
    }

    void Start () {

    }
    private void UpdateValue (List<Rank> rank) {
        Debug.Log (rank.Count);
        for (int i = 1; i <= rank.Count; i++) {
            try {
                Rank r = rank[rank.Count - i];
                GameObject rankObject = Instantiate (rank_template);
                rankObject.transform.parent = mainCanvas.transform;
                rankObject.transform.localPosition = new Vector3 (0f, 350f - 150f * i, 0f);
                Text rankObject_Order = rankObject.transform.Find ("Order").gameObject.GetComponent<Text> ();
                rankObject_Order.text = "" + (i);
                Text rankObject_Name = rankObject.transform.Find ("Name").gameObject.GetComponent<Text> ();
                rankObject_Name.text = r.username;
                Text rankObject_Score = rankObject.transform.Find ("Score").gameObject.GetComponent<Text> ();
                rankObject_Score.text = r.score;
            } catch (Exception e) {
                Debug.Log (e);
            }
        }
    }

    // Update is called once per frame
    void Update () {

    }
}