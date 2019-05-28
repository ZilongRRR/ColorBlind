using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FakeADManager : MonoBehaviour {
    public Image adImage;
    public Sprite[] ads;
    public int changeTime = 5;
    int nowAdIndex = 0;
    // Start is called before the first frame update
    void Start () {
        StartCoroutine (ChangeAD ());
    }

    IEnumerator ChangeAD () {
        yield return new WaitForSeconds (changeTime);
        nowAdIndex = (nowAdIndex + 1) % ads.Length;
        adImage.sprite = ads[nowAdIndex];
        StartCoroutine (ChangeAD ());
    }
}