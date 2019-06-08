using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankEntity : MonoBehaviour
{
    public Text rankOrder;
    public Text rankName;

    public Text rankScore;
    public Image Background;
    // Start is called before the first frame update
    void Start()
    {
    }
    public void FillRankUIValue(int order, string username, string userscore)
    {
        rankOrder.text = order.ToString();
        rankName.text = username;
        rankScore.text = userscore;
    }
    public void HighLight()
    {
        Background.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
