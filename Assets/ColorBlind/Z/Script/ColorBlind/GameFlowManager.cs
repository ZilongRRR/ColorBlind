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
        public TextMeshProUGUI comboText;
        public Animator pauseUIAnimator;
        [Header ("鏡頭")]
        public Transform cameraTr;
        public Vector3 preCameraPos;
        public float shakeDuration;
        public float shakeStrength;
        public int shakeVibrato;
        [Header ("遊戲內參數")]
        public float time;
        public float score = 0;
        public int combo = 0;
        public float comboDuration;
        public float comboDurationMax = 5;
        public bool isCombo = false;
        [Header ("提示訊息")]
        public string defaultMessage = "請選擇跟字一樣顏色的方塊";

        bool isCountDown = false;
        void Start () {
            time = 99;
            preCameraPos = cameraTr.position;
            CancelCombo ();
            NotificationManager.Instance.DoNotification (defaultMessage);
        }
        void Update () {
            if (isCountDown) {
                if (time > 0) {
                    time -= Time.deltaTime;
                    SetTimeText ();
                } else {
                    GameTimeOut ();
                }
                // combo 延續的時間
                if (comboDuration > 0) {
                    comboDuration -= Time.deltaTime;
                } else {
                    if (isCombo) {
                        ComboReset ();
                    }
                }
            }
        }
        // combo 中斷
        void ComboReset () {
            isCombo = false;
            combo = 0;
            comboDuration = 0;
            comboText.text = combo.ToString () + " combo";
        }
        public void ColorCorrect () {
            // 如果第一次就把遊戲開始
            if (!isCountDown) {
                isCountDown = true;
                NotificationManager.Instance.DoNotificationFade ();
            }
            // combo
            if (!isCombo) isCombo = true;
            combo++;
            comboDuration = comboDurationMax;
            comboText.text = combo.ToString () + " combo";
            // 計分
            int c = combo / 10;
            float ratio = ((float) c) / 10;
            score += 10 * (1 + ratio);
            scoreText.text = ((int) score).ToString ();
        }
        public void ColorError () {
            // 發送提示訊息
            if (isCountDown) {
                NotificationManager.Instance.DoNotificationAndFade (defaultMessage);
            }
            // 鏡頭震動
            cameraTr.DOPause ();
            preCameraPos = cameraTr.position;
            cameraTr
                .DOShakePosition (shakeDuration, shakeStrength, shakeVibrato)
                .OnPause (() => {
                    cameraTr.position = preCameraPos;
                })
                .OnComplete (() => {
                    cameraTr.position = preCameraPos;
                });
            // combo 中斷
            ComboReset ();
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
        void CancelCombo () {
            combo = 0;
            comboDuration = 0;
            comboText.text = "";
        }
        public void GamePause () {
            isCountDown = false;
            pauseUIAnimator.Play ("In");
        }
        public void GameRestart () {
            isCountDown = true;
            pauseUIAnimator.Play ("Out");
        }
    }
}