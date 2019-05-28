using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;

public class FireBaseConnect {
    public delegate void FireBaseConnectEvent (List<Rank> rank);
    public event FireBaseConnectEvent OnReadDataFinish = (e) => { };
    public event FireBaseConnectEvent OnReadDataFault = (e) => { };

    public List<Rank> rankList = new List<Rank> ();
    public FireBaseConnect (string domain_name = "ColorBlind") {
        // Set these values before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://colorblind-86332.firebaseio.com/");
        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        Debug.Log ("Connect Finish");
    }
    public void ReadData () {
        FirebaseDatabase.DefaultInstance
            .GetReference ("user-rank")
            .OrderByValue ().ValueChanged += HandleValueChanged;
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
    void HandleValueChanged (object sender, ValueChangedEventArgs args) {
        if (args.DatabaseError != null) {
            Debug.LogError (args.DatabaseError.Message);
            return;
        }
        // Do something with the data in args.Snapshot
        DataSnapshot snapshot = args.Snapshot;
        Debug.Log (snapshot.ChildrenCount);

        foreach (var ds in snapshot.Children) {
            Rank r = new Rank ();
            r.username = ds.Key.Clone ().ToString ();
            r.score = (ds.Value).ToString ().Clone ().ToString ();
            rankList.Add (r);
        }
        // Do Unity stuff
        if (OnReadDataFinish != null) {
            OnReadDataFinish (rankList);
        }
    }
    public void PushData (string user, int score) {

    }
}