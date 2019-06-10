using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewScore : MonoBehaviour
{
    public GameObject leaderBoard;
    public InputField nameInput;
    public Button confirmBtn;
    public Button cancelBtn;
    public Button restartBtn;
    public Image backgroundImage;
    public Text valueText;
    public GameObject allObjects;
    public Sprite[] background = new Sprite[2];
    // Start is called before the first frame update
    void Start()
    {
        confirmBtn.onClick.AddListener(ConfirmAction);
        cancelBtn.onClick.AddListener(CancelAction);
    }
    private void CancelAction()
    {
        leaderBoard.SetActive(true);
        this.gameObject.SetActive(false);
    }
    private void ConfirmAction()
    {
        string usr_name = nameInput.text;
        long usr_score = System.Convert.ToInt64(valueText.text);
        leaderBoard.SetActive(true);
        leaderBoard.GetComponent<LeaderBoardControl>().UploadRecord(usr_name, usr_score);
        this.gameObject.SetActive(false);
    }
    public void OpenScore(int score)
    {
        allObjects.SetActive(true);
        int maxScore = -1;
        valueText.text = score.ToString();
        if (PlayerPrefs.HasKey("MaxScore"))
        {
            maxScore = PlayerPrefs.GetInt("MaxScore");
        }
        if (score > maxScore)
        {
            valueText.text = score.ToString();
            backgroundImage.sprite = background[0];
            PlayerPrefs.SetInt("MaxScore", score);
        }
        else
        {
            backgroundImage.sprite = background[1];
            nameInput.gameObject.SetActive(false);
        }
        // 打開 UI 
    }
}