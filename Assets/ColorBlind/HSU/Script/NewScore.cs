using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewScore : MonoBehaviour {
    public GameObject leaderBoard;
    public InputField nameInput;
    public Button confirmBtn;
    public Button cancelBtn;
    public Text valueText;
    // Start is called before the first frame update
    void Start () {
        confirmBtn.onClick.AddListener (ConfirmAction);
        cancelBtn.onClick.AddListener (CancelAction);
    }
    private void CancelAction () {
        leaderBoard.SetActive (true);
        this.gameObject.SetActive (false);
    }
    private void ConfirmAction () {
        string usr_name = nameInput.text;
        long usr_score = System.Convert.ToInt64 (valueText.text);
        leaderBoard.SetActive (true);
        leaderBoard.GetComponent<LeaderBoardControl> ().UploadRecord (usr_name, usr_score);
        this.gameObject.SetActive (false);
    }
    public void OpenScore (int score) {
        valueText.text = score.ToString ();
        // 打開 UI 
    }
}