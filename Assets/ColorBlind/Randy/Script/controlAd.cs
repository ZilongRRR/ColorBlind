using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controlAd : MonoBehaviour
{
    [SerializeField] GameObject content;
    public GameObject closeAdButton;
    void Start()
    {
        closeAdButton.SetActive(false);
        this.Invoke("activeButton", 15f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void closeAd()
    {
        content.SetActive(false);
    }
    void activeButton()
    {
        closeAdButton.SetActive(true);
    }
}
