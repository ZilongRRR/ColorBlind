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

        [Header ("遊戲內參數")]
        public float time;
        public int score;
        public int Combo = 0;

        public void ColorCorrect () {

        }
        public void ColorError () {

        }
    }
}