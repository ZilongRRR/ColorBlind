using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewScore : MonoBehaviour
{
    public GameObject LeaderBoard;
    public InputField NameInput;
    public Button Confirm;
    public Button Cancel;
    public Text Value;
    // Start is called before the first frame update
    void Start()
    {
        System.Random rnd = new System.Random();
        int random_score = rnd.Next(100, 2000);
        Value.text = random_score.ToString();
        Confirm.onClick.AddListener(ConfirmAction);
        Cancel.onClick.AddListener(CancelAction);
    }
    private void CancelAction()
    {
        LeaderBoard.SetActive(true);
        this.gameObject.SetActive(false);
    }
    private void ConfirmAction()
    {
        string usr_name = NameInput.text;
        long usr_score = System.Convert.ToInt64(Value.text);
        LeaderBoard.SetActive(true);
        LeaderBoard.GetComponent<LeaderBoardControl>().UploadRecord(usr_name, usr_score);
        this.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
