using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controlAd : MonoBehaviour {
    [SerializeField] GameObject content;
    public float time = 5;
    public StreamVideo streamVideo;
    public GameObject closeAdButton;

    public void OpenAd () {
        content.SetActive (true);
        streamVideo.StartPlay ();
        this.Invoke ("ActiveButton", time);
    }
    public void CloseAd () {
        streamVideo.StopViedo ();
        content.SetActive (false);
    }
    void ActiveButton () {
        closeAdButton.SetActive (true);
    }
}