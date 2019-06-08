using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;

public struct FirebaseStatus
{
    public const string NO_CONNECT = "未連線";
    public const string CONNECTING = "讀取中";
    public const string CONNECT_FINISH = "連線成功";
    public const string CONNECT_FAILED = "連線失敗";
    public const string DATA_READING_FAILED = "資料讀取失敗";
    public const string DATA_READING = "資料讀取中";
    public const string UPDATE_DATA = "資料更新";
}

public class FireBaseConnect
{
    public delegate void FireBaseConnectEvent(List<Rank> rank);
    public event FireBaseConnectEvent OnReadDataFinish = (e) => { };
    public event FireBaseConnectEvent OnReadDataFault = (e) => { };

    public delegate void FireBaseStatusEvent(string status);
    public event FireBaseStatusEvent OnShowStatus = (e) => { };

    public List<Rank> rankList = new List<Rank>();
    private DatabaseReference reference;

    private bool isConnect = false;
    private string domain_name;

    public FireBaseConnect(string domain_name = "ColorBlind")
    {
        this.domain_name = domain_name;
    }
    public void Connection()
    {
        ShowStatus(FirebaseStatus.CONNECTING);
        try
        {
            // Set these values before calling into the realtime database.
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://colorblind-86332.firebaseio.com/");
            // Get the root reference location of the database.
            reference = FirebaseDatabase.DefaultInstance.RootReference;
            isConnect = true;
            ShowStatus(FirebaseStatus.CONNECT_FINISH);
        }
        catch (Exception e)
        {
            ShowStatus(FirebaseStatus.CONNECT_FAILED);
            Debug.Log(e);
        }
    }
    public void InitReadDataEvent()
    {
        if (!isConnect)
        {
            ShowStatus(FirebaseStatus.NO_CONNECT);
            return;
        }
        ShowStatus(FirebaseStatus.DATA_READING);
        reference
        .Child("user-rank")
        .OrderByChild("score")
        .ValueChanged += HandleValueChanged;
        ShowStatus(FirebaseStatus.DATA_READING_FAILED);
    }
    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        rankList = new List<Rank>();
        if (args.DatabaseError != null)
        {
            ShowStatus(FirebaseStatus.NO_CONNECT);
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        // Do something with the data in args.Snapshot
        DataSnapshot snapshot = args.Snapshot;
        // Debug.Log (snapshot.ChildrenCount);
        foreach (var users in snapshot.Children)
        {
            Rank r = new Rank();
            foreach (var user_info in users.Children)
            {
                if (user_info.Key.ToString() == "username")
                    r.username = (user_info.Value).ToString();
                if (user_info.Key.ToString() == "score")
                    r.score = (user_info.Value).ToString();
            }
            r.key = (users.Key).ToString();
            rankList.Add(r);
        }
        ShowStatus(FirebaseStatus.UPDATE_DATA);
        // Do Unity stuff
        if (OnReadDataFinish != null)
        {
            OnReadDataFinish(rankList);
        }
    }
    public string AddScoreToLeaders(string username, long score)
    {
        // // Update complex data that could be corrupted by concurrent updates.
        // reference.Child("user-rank").RunTransaction(mutableData =>
        // {
        //     List<object> leaders = mutableData.Value as List<object>;

        //     if (leaders == null)
        //     {
        //         leaders = new List<object>();
        //     }
        //     // Add the new high score.
        //     Dictionary<string, object> newScoreMap =
        //                      new Dictionary<string, object>();
        //     newScoreMap["score"] = score;
        //     newScoreMap["username"] = username;
        //     leaders.Add(newScoreMap);
        //     mutableData.Value = leaders;
        //     return TransactionResult.Success(mutableData);
        // });
        // Create new entry at /user-scores/$userid/$scoreid and at
        // /leaderboard/$scoreid simultaneously
        string key = reference.Child("user-rank").Push().Key;
        Dictionary<string, System.Object> entryValues = new Dictionary<string, System.Object> { { "username", username },
            { "score", score }
        };
        Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();
        childUpdates["/user-rank/" + key] = entryValues;

        reference.UpdateChildrenAsync(childUpdates);
        return key;
    }
    private void ShowStatus(string status)
    {
        if (OnShowStatus != null)
        {
            OnShowStatus(status);
        }
        Debug.Log(status);
    }
}