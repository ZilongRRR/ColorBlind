using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace ZTools {
    public class GameFlowManager : Singleton<GameFlowManager> {
        [Header ("UI")]
        public TextMeshProUGUI timeText;
        public TextMeshProUGUI scoreText;
        [Header ("鏡頭")]
        public Transform cameraTr;
        public Vector3 preCameraPos;
        public float shakeDuration;
        public float shakeStrength;
        public int shakeVibrato;
        [Header ("遊戲內參數")]
        public float time;
        public int score;
        public int combo = 0;
        public float comboDuration;
        [Header ("提示訊息")]
        public string defaultMessage = "請選擇跟字一樣顏色的方塊";

        bool isCountDown = true;
        void Start () {
            time = 99;
            preCameraPos = cameraTr.position;
        }
        void Update () {
            if (isCountDown) {
                if (time > 0) {
                    time -= Time.deltaTime;
                    SetTimeText ();
                } else {
                    GameTimeOut ();
                }
            }
        }
        public void ColorCorrect () {

        }
        public void ColorError () {
            NotificationManager.Instance.DoNotificationAndFade (defaultMessage);
            preCameraPos = cameraTr.position;
            cameraTr.DOPause ();
            cameraTr.DOShakePosition (shakeDuration, shakeStrength, shakeVibrato).OnPause (() => {
                cameraTr.position = preCameraPos;
            }).OnComplete (() => {
                cameraTr.position = preCameraPos;
            });
        }
        void GameTimeOut () {
            isCountDown = false;
            time = 0;
            SetTimeText ();
        }
        void SetTimeText () {
            int t = (int) time;
            timeText.text = t.ToString ();
        }
        public void GameStart () {

        }
    }
}