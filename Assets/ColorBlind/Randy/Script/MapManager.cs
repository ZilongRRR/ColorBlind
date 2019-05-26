using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZTools;
using UnityEngine.UI;
public class MapManager : Singleton<MapManager> {
    // Start is called before the first frame update
    [SerializeField] public Camera cam;
    public string[] colorTextArray = new string[]{"紅","橙","黃","綠","藍","靛","紫","橘",
                                                "白","黑","灰","粉","棕","發","財","讚"};
    public Color[] colorArray = new Color[] { new Color(23/255, 28/255, 97/255), new Color(3/255, 110/255, 184/255),
                                            new Color(0, 162/255, 154/255), new Color(0, 162/255, 154/255),
                                            new Color(34/255, 172/255, 56/255), new Color(218/255, 224/255, 0),
                                            new Color(248/255, 182/255, 45/255), new Color(234/255, 85/255, 20/255),
                                            new Color(195/255, 13/255, 35/255), new Color(62/255, 58/255, 57/255),
                                            new Color(239/255, 239/255, 239/255)};
    public Vector3[] neightborVectors;
    public Dictionary<int, int[]> location_coord = new Dictionary<int, int[]> { { 0, new int[]{-1, 1 } }, { 1, new int[] { 0, 1 } } ,{ 2, new int[] { 1, 1 } } ,
                                                                                { 3, new int[] { -1, 0 } } ,{ 4, new int[] { 0, 0 } },{ 5, new int[] { 1, 0 } },
                                                                                { 6, new int[] { -1, -1 } },{ 7, new int[] { 0, -1 } },{ 8, new int[] { 1, -1 } }};
    public List<int[]> clicked_blocks = new List<int[]>();
    // 記錄現在正確的顏色
    public int currColor;
    public int Combo = 0;
    public Block currCenterBlock;
    public Text text;
    private float distance = 1.1f;

    void Start()
    {
        neightborVectors = new Vector3[9] { new Vector3(0,currCenterBlock.transform.localScale.x*distance*Mathf.Sqrt(2),0),
                                            new Vector3(currCenterBlock.transform.localScale.x*distance/Mathf.Sqrt(2),currCenterBlock.transform.localScale.x*distance/Mathf.Sqrt(2),0),
                                            new Vector3(currCenterBlock.transform.localScale.x*distance*Mathf.Sqrt(2),0,0),
                                            new Vector3(-currCenterBlock.transform.localScale.x*distance/Mathf.Sqrt(2),currCenterBlock.transform.localScale.x*distance/Mathf.Sqrt(2),0),
                                            new Vector3(0,0,0),
                                            new Vector3(currCenterBlock.transform.localScale.x*distance/Mathf.Sqrt(2),-currCenterBlock.transform.localScale.x*distance/Mathf.Sqrt(2),0),
                                            new Vector3(-currCenterBlock.transform.localScale.x*distance*Mathf.Sqrt(2),0,0),
                                            new Vector3(-currCenterBlock.transform.localScale.x*distance/Mathf.Sqrt(2),-currCenterBlock.transform.localScale.x*distance/Mathf.Sqrt(2),0),
                                            new Vector3(0,-currCenterBlock.transform.localScale.x*distance*Mathf.Sqrt(2),0)};
        
        for(int index = 0; index < 9; index++)
        {
            var blockIns = GameObject.Instantiate(currCenterBlock);
            blockIns.transform.position = currCenterBlock.transform.position + neightborVectors[index];
            blockIns.cooord_x = currCenterBlock.cooord_x + location_coord[index][0];
            blockIns.cooord_y = currCenterBlock.cooord_y + location_coord[index][1];
            blockIns.InitColor();
            currCenterBlock.SetNeightborBlocks(index, blockIns);
        }
        currCenterBlock.InitCenter();
        currCenterBlock.gameObject.SetActive(false);

        //create UI Text
        GameObject canvasGO = new GameObject();
        canvasGO.name = "Canvas";
        canvasGO.AddComponent<Canvas>();
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // Get canvas from the GameObject.
        Canvas canvas;
        canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Create the Text GameObject.
        GameObject textGO = new GameObject();
        textGO.transform.parent = canvasGO.transform;
        text = textGO.AddComponent<Text>();
        text.fontSize = 1;
        text.alignment = TextAnchor.MiddleCenter;
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
        text.text = GetRandomColorString();
        text.transform.position = currCenterBlock.transform.position;
        text.color = GetColorByIndex(currColor);
    }
}