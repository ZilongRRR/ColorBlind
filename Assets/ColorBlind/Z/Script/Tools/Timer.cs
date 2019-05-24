using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZTools;

public class Timer : Singleton<Timer> {

    [Header ("Timer Parameter")]
    /// <summary>
    /// 紀錄當 StartTimer 是 true 時到現在所過的時間
    /// </summary>
    public float NowTime = 0;
    /// <summary>
    /// When StartTimer is true, it will let Timer run then the NowTime will have the time 
    /// </summary>
    public bool isStart = false;

    public float EndTime;

    public List<float> RecordTimeList = new List<float> ();

    public delegate void TimerMission (GameObject sender);
    /// <summary>
    /// 註冊給計時器當某個時間時的特發事件
    /// </summary>
    public event TimerMission TimeIsUp;
    bool isTimeisupMission = false;
    float Timetodo = -1;
    /// <summary>
    /// 註冊給計時器當狀態時間到時需要切換狀態
    /// </summary>
    public event TimerMission TimeToState;
    bool isStateTimeisup = false;
    float Timetostate = -1;

    void Start () {

    }

    void Update () {
        if (isStart)
            TimerRun ();
        if (isTimeisupMission && TimeIsUp != null && NowTime >= Timetodo && Timetodo >= 0) {
            isTimeisupMission = false;
            NowTime = 0;
            Timetodo = -1;
            StopTimer ();
            TimeIsUp (this.gameObject);
        }
        if (isStateTimeisup && TimeToState != null && NowTime >= Timetostate && Timetostate >= 0) {
            isStateTimeisup = false;
            NowTime = 0;
            Timetostate = -1;
            StopTimer ();
            TimeToState (this.gameObject);
        }
    }
    /// <summary>
    /// 時間計時器計時
    /// </summary>
    void TimerRun () {
        NowTime += Time.deltaTime;
        ShowMessage ("Now time is " + NowTime);
    }
    /// <summary>
    /// 啟動時間計時器
    /// </summary>
    public void StartTimer () {
        isStart = true;
    }

    /// <summary>
    /// 暫停時間計時器
    /// </summary>
    public void StopTimer () {
        isStart = false;
    }
    /// <summary>
    /// 重製時間計時器
    /// </summary>
    public void RestTime () {
        StopTimer ();
        NowTime = 0;
    }
    /// <summary>
    /// 紀錄當前時間
    /// </summary>
    public void RecordTime () {
        RecordTimeList.Add (NowTime);
    }
    /// <summary>
    /// 設定哪個時間由計時器觸發所註冊的時間事件 TimeIsUp()
    /// </summary>
    public void SetTimeisupMission (float t) {
        Timetodo = t;
        isTimeisupMission = true;
    }
    /// <summary>
    /// 設定哪個時間由計時器觸發所註冊的狀態事件 TimeToState()
    /// </summary>
    public void SetTimeToState (float t) {
        Timetostate = t;
        isStateTimeisup = true;
    }
}