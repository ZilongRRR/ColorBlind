using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;

public class FireBaseConnect
{
    private List<Rank> rankList = new List<Rank>();
    public FireBaseConnect(string domain_name = "ColorBlind")
    {
        // Set these values before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://colorblind-86332.firebaseio.com/");
        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        Debug.Log("Connect Finish");
    }
    public void ReadData(string[] rank_info_name, string[] rank_info_score)
    {
        var getTask = FirebaseDatabase.DefaultInstance
        .GetReference("user-rank")
        .OrderByValue()
        .GetValueAsync().ContinueWith(task =>
         {
             if (task.IsFaulted)
             {
                 Debug.Log("Error loading");
             }
             else if (task.IsCompleted)
             {
                 DataSnapshot snapshot = task.Result;
                 Debug.Log(snapshot.ChildrenCount);
                 int i = 0;
                 foreach (var ds in snapshot.Children)
                 {
                     Debug.Log(ds.Key);
                     Debug.Log(ds.Value.ToString());
                     rank_info_name[i] = ds.Key.Clone().ToString();
                     rank_info_score[i] = (ds.Value).ToString();
                     i++;
                 }
             }
         });
    }
    public void PushData(string user, int score)
    {

    }
}
