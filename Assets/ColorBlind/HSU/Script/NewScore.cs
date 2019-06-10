using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZTools;
public class NewScore : MonoBehaviour {
    public LeaderBoardControl leaderBoard;
    public GameObject highlight;
    public InputField nameInput;
    public Button confirmBtn;
    public Button cancelBtn;
    public Button restartBtn;
    public Image backgroundImage;
    public Text valueText;
    public GameObject allObjects;
    public Sprite[] background = new Sprite[2];
    private bool isPushed = true;
    private bool toShow = false;
    // Start is called before the first frame update
    void Start () {
        confirmBtn.onClick.AddListener (ConfirmAction);
        cancelBtn.onClick.AddListener (CancelAction);
        restartBtn.onClick.AddListener (RestartAction);
    }
    private void RestartAction () {
        SceneLoading.Instance.LoadScene ("1.GameScene");
    }
    private void CancelAction () {
        SceneLoading.Instance.LoadScene ("0.TitleScene");
    }
    private void ConfirmAction () {
        if (!isPushed) {
            if (nameInput.text == "") {
                NotificationManager.Instance.DoNotificationAndFade ("名字不可為空白");
                return;
            }
            string usr_name = nameInput.text;
            long usr_score = System.Convert.ToInt64 (valueText.text);
            leaderBoard.UploadRecord (usr_name, usr_score);
            isPushed = true;
        } else {
            leaderBoard.OpenUI (toShow);
        }
    }
    public void OpenScore (int score) {
        allObjects.SetActive (true);
        int maxScore = -1;
        valueText.text = score.ToString ();
        if (PlayerPrefs.HasKey ("MaxScore")) {
            maxScore = PlayerPrefs.GetInt ("MaxScore");
        }
        if (score > maxScore) {
            toShow = true;
            isPushed = false;
            highlight.SetActive (true);
            valueText.text = score.ToString ();
            backgroundImage.sprite = background[0];
            PlayerPrefs.SetInt ("MaxScore", score);
        } else {
            highlight.SetActive (false);
            backgroundImage.sprite = background[1];
            nameInput.gameObject.SetActive (false);
        }
        // 打開 UI 
    }
}