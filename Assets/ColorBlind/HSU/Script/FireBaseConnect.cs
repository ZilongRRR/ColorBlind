using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;

public class FireBaseConnect
{
    public delegate void FireBaseConnectEvent(List<Rank> rank);
    public event FireBaseConnectEvent OnReadDataFinish = (e) => { };
    public event FireBaseConnectEvent OnReadDataFault = (e) => { };

    public List<Rank> rankList = new List<Rank>();
    private DatabaseReference reference;
    public FireBaseConnect(string domain_name = "ColorBlind")
    {
        // Set these values before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://colorblind-86332.firebaseio.com/");
        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        Debug.Log("Connect Finish");
    }
    public void ReadData()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("user-rank")
            .OrderByChild("score").ValueChanged += HandleValueChanged;
        // .GetValueAsync ().ContinueWith (task => {
        //     if (task.IsFaulted) {
        //         Debug.Log ("Error loading");
        //         if (OnReadDataFault != null) {
        //             OnReadDataFault (null);
        //         }
        //     } else if (task.IsCompleted) {
        //         DataSnapshot snapshot = task.Result;
        //         Debug.Log (snapshot.ChildrenCount);

        //         foreach (var ds in snapshot.Children) {

        //             Rank r = new Rank ();
        //             r.username = ds.Key.Clone ().ToString ();
        //             r.score = (ds.Value).ToString ().Clone ().ToString ();
        //             rankList.Add (r);
        //         }
        //         // Do Unity stuff
        //         if (OnReadDataFinish != null) {
        //             OnReadDataFinish (rankList);
        //         }
        //     }
        // }, TaskScheduler.FromCurrentSynchronizationContext ());
    }
    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        rankList = new List<Rank>();
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        // Do something with the data in args.Snapshot
        DataSnapshot snapshot = args.Snapshot;
        Debug.Log(snapshot.ChildrenCount);

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
            rankList.Add(r);
        }
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
        Dictionary<string, System.Object> entryValues = new Dictionary<string, System.Object>{
            {"username", username},
            {"score", score}
        };

        Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();
        childUpdates["/user-rank/" + key] = entryValues;

        reference.UpdateChildrenAsync(childUpdates);
        return key;
    }
}