using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
public class FireBaseConnect
{
    public FireBaseConnect(string domain_name = "ColorBlind")
    {
        // Set these values before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://colorblind-86332.firebaseio.com/");
        // Get the root reference location of the database.
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        Debug.Log("Connect Finish");
    }
    public void ReadData()
    {
        Debug.Log("Start to read");
        FirebaseDatabase.DefaultInstance
        .GetReference("user-rank")
        .GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("error");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log("Child Count");
                Debug.Log(snapshot.ChildrenCount + "");
                foreach (var data in snapshot.Children)
                {
                    Debug.Log("Raw Json");
                    Debug.Log(data.GetRawJsonValue());
                }
            }
        });
    }
}
