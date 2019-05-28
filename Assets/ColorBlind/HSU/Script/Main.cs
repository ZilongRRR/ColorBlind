using System.Collections;
using System.Collections.Generic;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public GameObject rank_template;
    public GameObject mainCanvas;
    private string[] rank_info_name = new string[5];
    private string[] rank_info_score = new string[5];
    FireBaseConnect database;
    // Start is called before the first frame update
    void Start()
    {
        database = new FireBaseConnect();
        StartCoroutine(UpdateValue());
        // rank_info = database.ReadData();
        // Debug.Log(rank_info.Keys.Count);
        // foreach (var user in rank_info.Keys)
        // {
        //     GameObject rankObject = Instantiate(rank_template);
        //     Text rankObject_Name = rankObject.transform.Find("Name").gameObject.GetComponent<Text>();
        //     rankObject_Name.text = user;
        //     Text rankObject_Score = rankObject.transform.Find("Score").gameObject.GetComponent<Text>();
        //     rankObject_Score.text = string.Format("%7d", rank_info[user]);
        // }
    }
    IEnumerator UpdateValue()
    {
        database.ReadData(rank_info_name, rank_info_score);
        yield return new WaitForSeconds(3.5f);
        for (int i = 0; i < 5; i++)
        {
            GameObject rankObject = Instantiate(rank_template);
            rankObject.transform.parent = mainCanvas.transform;
            rankObject.transform.localPosition = new Vector3(0f, 200f - 150f * i, 0f);
            Text rankObject_Order = rankObject.transform.Find("Order").gameObject.GetComponent<Text>();
            rankObject_Order.text = "" + (i + 1);
            Text rankObject_Name = rankObject.transform.Find("Name").gameObject.GetComponent<Text>();
            rankObject_Name.text = rank_info_name[i];
            Text rankObject_Score = rankObject.transform.Find("Score").gameObject.GetComponent<Text>();
            rankObject_Score.text = rank_info_score[i];
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
