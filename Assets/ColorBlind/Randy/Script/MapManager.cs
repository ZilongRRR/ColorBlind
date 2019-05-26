using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZTools;
public class MapManager : Singleton<MapManager> {
    // Start is called before the first frame update
    public string[] colorTextArray = new string[]{"紅","橙","黃","綠","藍","靛","紫","橘",
                                                "白","黑","灰","粉","棕","發","財","讚"};
    public Color[] colorArray = new Color[] { new Color(23, 28, 97), new Color(3, 110, 184),
                                            new Color(0, 162, 154), new Color(0, 162, 154),
                                            new Color(34, 172, 56), new Color(218, 224, 0),
                                            new Color(248, 182, 45), new Color(234, 85, 20),
                                            new Color(195, 13, 35), new Color(62, 58, 57),
                                            new Color(239, 239, 239)};
    public Vector3[] neightborVectors; 
    // 記錄現在正確的顏色
    public int currColor;
    public int Combo = 0;
    public Block currCenterBlock;
    public string text;

    void Start()
    {
        neightborVectors = new Vector3[8] { new Vector3(0,currCenterBlock.transform.localScale.x*1.2f*Mathf.Sqrt(2),0),
                                            new Vector3(-Mathf.Sqrt(currCenterBlock.transform.localScale.x*1.2f/2),Mathf.Sqrt(currCenterBlock.transform.localScale.x*1.2f/2),0),
                                            new Vector3(Mathf.Sqrt(currCenterBlock.transform.localScale.x*1.2f/2),Mathf.Sqrt(currCenterBlock.transform.localScale.x*1.2f/2),0),
                                            new Vector3(-currCenterBlock.transform.localScale.x*1.2f*Mathf.Sqrt(2),0,0),
                                            new Vector3(currCenterBlock.transform.localScale.x*1.2f*Mathf.Sqrt(2),0,0),
                                            new Vector3(-Mathf.Sqrt(currCenterBlock.transform.localScale.x*1.2f/2),-Mathf.Sqrt(currCenterBlock.transform.localScale.x*1.2f/2),0),
                                            new Vector3(Mathf.Sqrt(currCenterBlock.transform.localScale.x*1.2f/2),-Mathf.Sqrt(currCenterBlock.transform.localScale.x*1.2f/2),0),
                                            new Vector3(0,-currCenterBlock.transform.localScale.x*1.2f*Mathf.Sqrt(2),0)};

        currCenterBlock.InitCenter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetRandomColorIndex() {
        return Random.Range(0, colorArray.Length);
    }
    //方塊本身的顏色
    public Color GetColorByIndex(int index)
    {
        return colorArray[index];
    }
    public string GetRandomColorString() {
        int index = Random.Range(0, colorTextArray.Length);
        return colorTextArray[index];
    }

    private void GetText()
    {
        // 從 currCenterBlock 的相鄰方塊隨機取得 text 的顏色
        // 並且從 GetRandomColorString() 取得文字
        currColor = Random.Range(0, currCenterBlock.GetNeightborBlocks().Length);
        text = GetRandomColorString();
    }
}