using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ZTools;

namespace ZTools {
    [System.Serializable]
    public class TextTips {
        public Text tips;
        [Header ("Use Text tips or not")]
        public bool isUse = true;
        [Header ("Defalut words limit 17 * 3")]
        [TextArea (1, 3)]
        public string[] string_tips;
        public int stingIndex;
        public float stringTipsDelay = 2f;
    }

    [System.Serializable]
    public class SpriteTips {
        public Image tips;
        [Header ("Use Sprite tips or not")]
        public bool isUse = true;
        public Sprite[] sprite_tips;
        public int spriteIndex;
        public float imgTipsDelay = 3f;
    }

    [System.Serializable]
    public class LoadingTips {
        public TextTips textTips;
        public SpriteTips spriteTips;
    }

    [System.Serializable]
    public class LoadingBack {
        public Image loadingBackgroundImg;
        public Sprite[] spriteBackgroundImg;
        public int spriteIndex;
        public float imgBackgroundDelay = 10;
    }

    [System.Serializable]
    public class LoadingImgSet {
        public Image loadingImg;
        [Header ("Use Loading Image or not")]
        public bool isUse = true;
    }

    [System.Serializable]
    public class LoadingTextSet {
        public Text loadingText;
        [Header ("Use Loading text to show the progress rate")]
        public bool isUse = true;
        [Header ("Show the Number of digits")]
        public string numberOfDigits = "0.0";
    }

    [System.Serializable]
    public class LoadingBarSet {
        public Image loadingBarImg;
        public Image loadingBarBackImg;
        [Header ("Use Loading Bar or not")]
        public bool isUse = true;
        public float duration = 0.1f;
    }

    [System.Serializable]
    public class LoadingObjSet {
        public Image loadingObj;
        [Header ("Use Loading Object on the bar or not")]
        public bool isUse = true;
        [Header ("Edit the initPos and MaxX in run time")]
        public bool editMode;
        public Vector2 initPos;
        public float maxX;
        public float duration = 0.1f;
    }

    [System.Serializable]
    public class LoadingBar {
        public LoadingBarSet loadingBarSet;
        public LoadingObjSet loadingObjSet;
        public LoadingImgSet loadingImgSet;
        public LoadingTextSet loadingTextSet;
    }

    public class SceneLoading : Singleton<SceneLoading> {
        [Header ("Scene Loading Tips")]
        public LoadingTips m_LoadingTips;
        [Header ("Scene Loading Background")]
        public LoadingBack m_LoadingBack;
        [Header ("Scene Loading Bar")]
        public LoadingBar m_LoadingBar;
        [Header ("Scene Loading Setting")]
        public string LoadingSceneName;
        public bool AutoSceneChange = true;

        AsyncOperation async = null;
        // 預防重複讀取場景
        bool isLoading = false;

        public void SceneChange () {
            async.allowSceneActivation = true;
        }
        public void LoadScene () {
            LoadScene (LoadingSceneName);
        }

        public void LoadScene (string sceneName) {
            if (!isLoading) {
                isLoading = true;
                StartCoroutine (DisplayLoading (sceneName));
            } else {
                return;
            }
        }

        private void Update () {
            // 編輯位置
            if (m_LoadingBar.loadingObjSet.editMode) {
                Vector2 pos = m_LoadingBar.loadingObjSet.initPos;
                pos.x += m_LoadingBar.loadingObjSet.maxX;
                m_LoadingBar.loadingObjSet.loadingObj.rectTransform.anchoredPosition = pos;
            }
        }

        private IEnumerator LoopTextTips () {
            // 如果提示列為零，則不循環播
            if (m_LoadingTips.textTips.string_tips.Length == 0) {
                yield return null;
            } else {
                if (m_LoadingTips.textTips.stingIndex == m_LoadingTips.textTips.string_tips.Length)
                    m_LoadingTips.textTips.stingIndex = 0;
                m_LoadingTips.textTips.tips.text = m_LoadingTips.textTips.string_tips[m_LoadingTips.textTips.stingIndex];
                m_LoadingTips.textTips.tips.DOFade (1, 0.3f);
                yield return new WaitForSeconds (m_LoadingTips.textTips.stringTipsDelay);
                m_LoadingTips.textTips.stingIndex++;
                m_LoadingTips.textTips.tips.DOFade (0, 0.3f).OnComplete (() => StartCoroutine (LoopTextTips ()));
            }
        }

        private IEnumerator LoopImageTips () {
            // 如果提示列為零，則不循環播
            if (m_LoadingTips.spriteTips.sprite_tips.Length == 0) {
                yield return null;
            } else {
                if (m_LoadingTips.spriteTips.spriteIndex == m_LoadingTips.spriteTips.sprite_tips.Length)
                    m_LoadingTips.spriteTips.spriteIndex = 0;
                m_LoadingTips.spriteTips.tips.sprite = m_LoadingTips.spriteTips.sprite_tips[m_LoadingTips.spriteTips.spriteIndex];
                m_LoadingTips.spriteTips.tips.DOFade (1, 0.5f);
                yield return new WaitForSeconds (m_LoadingTips.textTips.stringTipsDelay);
                m_LoadingTips.spriteTips.spriteIndex++;
                m_LoadingTips.spriteTips.tips.DOFade (0, 0.5f).OnComplete (() => StartCoroutine (LoopImageTips ()));
            }
        }

        private void InitLoading () {
            // 是否顯示進度條
            m_LoadingBar.loadingBarSet.loadingBarImg.gameObject.SetActive (m_LoadingBar.loadingBarSet.isUse);
            // 是否顯示進度條（背景）
            m_LoadingBar.loadingBarSet.loadingBarBackImg.gameObject.SetActive (m_LoadingBar.loadingBarSet.isUse);
            // 是否顯示讀取圖片
            m_LoadingBar.loadingImgSet.loadingImg.gameObject.SetActive (m_LoadingBar.loadingImgSet.isUse);
            // 是否顯示動態移動物件
            m_LoadingBar.loadingObjSet.loadingObj.gameObject.SetActive (m_LoadingBar.loadingObjSet.isUse);
            // 是否顯示進度百分比
            m_LoadingBar.loadingTextSet.loadingText.gameObject.SetActive (m_LoadingBar.loadingTextSet.isUse);
            // 顯示背景圖片
            m_LoadingBack.loadingBackgroundImg.gameObject.SetActive (true);
            // 是否顯示圖片類的提示
            m_LoadingTips.spriteTips.tips.gameObject.SetActive (m_LoadingTips.spriteTips.isUse);
            // 是否顯示文字類的提示
            m_LoadingTips.textTips.tips.gameObject.SetActive (m_LoadingTips.textTips.isUse);
            // 隨機取得提示的開始文字
            m_LoadingTips.textTips.stingIndex = Random.Range (0, m_LoadingTips.textTips.string_tips.Length);
            // 隨機取得提示的開始圖片
            m_LoadingTips.spriteTips.spriteIndex = Random.Range (0, m_LoadingTips.spriteTips.sprite_tips.Length);
        }

        #region Loading

        private IEnumerator DisplayLoading (string sceneName) {
            InitLoading ();
            StartCoroutine (LoopTextTips ());
            StartCoroutine (LoopImageTips ());
            float progress = 0;
            float toProgress = 0;
            // 背景載入
            async = SceneManager.LoadSceneAsync (sceneName);

            // 取消自動切換場景
            async.allowSceneActivation = false;
            while (async.progress < 0.9f) {
                toProgress = (async.progress * 100);
                while (progress < toProgress) {
                    progress++;
                    SetLoading (progress);
                    yield return new WaitForEndOfFrame ();
                }
            }
            // 補足到100
            toProgress = 100;
            while (progress < toProgress) {
                progress++;
                SetLoading (progress);
                yield return new WaitForEndOfFrame ();
            }
            // 切換場景
            if (AutoSceneChange)
                async.allowSceneActivation = true;
            else
                ShowSceneChange ();
        }

        private void ShowSceneChange () {

        }

        private void SetLoading (float percent) {
            percent /= 100;
            // 如果使用進度條
            if (m_LoadingBar.loadingBarSet.isUse) {
                m_LoadingBar.loadingBarSet.loadingBarImg.DOFillAmount (percent, m_LoadingBar.loadingBarSet.duration);
            }
            // 如果使用進度條上的物件跟著移動
            if (m_LoadingBar.loadingObjSet.isUse) {
                Vector2 pos = m_LoadingBar.loadingObjSet.initPos;
                pos.x += m_LoadingBar.loadingObjSet.maxX * percent;
                m_LoadingBar.loadingObjSet.loadingObj.rectTransform.DOAnchorPos (pos, m_LoadingBar.loadingObjSet.duration);
            }
            // 如果顯示進度百分比
            if (m_LoadingBar.loadingTextSet.isUse) {
                m_LoadingBar.loadingTextSet.loadingText.text = (percent * 100).ToString (m_LoadingBar.loadingTextSet.numberOfDigits) + " %";
            }
        }
        #endregion
    }
}