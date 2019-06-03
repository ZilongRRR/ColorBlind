using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Use : MonoBehaviour
{
    public LeaderBoardControl leaderBoardCtrl;
    // Start is called before the first frame update
    void Start()
    {
        leaderBoardCtrl.InitLeaderBoard();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
