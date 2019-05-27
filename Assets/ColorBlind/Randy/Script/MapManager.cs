using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZTools;
public class MapManager : Singleton<MapManager> {
    // Start is called before the first frame update
    [SerializeField] public Camera cam;
    public string[] colorTextArray = new string[] {
        "紅",
        "橙",
        "黃",
        "綠",
        "藍",
        "靛",
        "紫",
        "橘",
        "白",
        "黑",
        "灰",
        "粉",
        "棕",
        "發",
        "財",
        "讚"
    };
    public Color[] colorArray = new Color[] {
        new Color (23f / 255f, 28f / 255f, 97f / 255f), new Color (3f / 255f, 110f / 255f, 184f / 255f),
        new Color (0f, 162f / 255f, 154f / 255f), new Color (0f, 162f / 255f, 154f / 255f),
        new Color (34f / 255f, 172f / 255f, 56f / 255f), new Color (218f / 255f, 224f / 255f, 0f),
        new Color (248f / 255f, 182f / 255f, 45f / 255f), new Color (234f / 255f, 85f / 255f, 20f / 255f),
        new Color (195f / 255f, 13f / 255f, 35f / 255f), new Color (62f / 255f, 58f / 255f, 57f / 255f),
        new Color (239f / 255f, 239f / 255f, 239f / 255f)
    };
    public Vector3[] neightborVectors;
    public Dictionary<int, int[]> location_coord = new Dictionary<int, int[]> { { 0, new int[] {-1, 1 } },
        { 1, new int[] { 0, 1 } },
        { 2, new int[] { 1, 1 } },
        { 3, new int[] {-1, 0 } },
        { 4, new int[] { 0, 0 } },
        { 5, new int[] { 1, 0 } },
        { 6, new int[] {-1, -1 } },
        { 7, new int[] { 0, -1 } },
        { 8, new int[] { 1, -1 } }
    };
    public List<int[]> clicked_blocks = new List<int[]> ();
    // 記錄現在正確的顏色
    public int currColor;
    public int Combo = 0;
    public Block currCenterBlock;
    public Text text;
    [SerializeField, Header ("方塊間距")]
    private float distance = 2f;

    void Start () {
        neightborVectors = new Vector3[9] {
            new Vector3 (0, currCenterBlock.transform.localScale.x * distance * Mathf.Sqrt (2), 0),
            new Vector3 (currCenterBlock.transform.localScale.x * distance / Mathf.Sqrt (2), currCenterBlock.transform.localScale.x * distance / Mathf.Sqrt (2), 0),
            new Vector3 (currCenterBlock.transform.localScale.x * distance * Mathf.Sqrt (2), 0, 0),
            new Vector3 (-currCenterBlock.transform.localScale.x * distance / Mathf.Sqrt (2), currCenterBlock.transform.localScale.x * distance / Mathf.Sqrt (2), 0),
            new Vector3 (0, 0, 0),
            new Vector3 (currCenterBlock.transform.localScale.x * distance / Mathf.Sqrt (2), -currCenterBlock.transform.localScale.x * distance / Mathf.Sqrt (2), 0),
            new Vector3 (-currCenterBlock.transform.localScale.x * distance * Mathf.Sqrt (2), 0, 0),
            new Vector3 (-currCenterBlock.transform.localScale.x * distance / Mathf.Sqrt (2), -currCenterBlock.transform.localScale.x * distance / Mathf.Sqrt (2), 0),
            new Vector3 (0, -currCenterBlock.transform.localScale.x * distance * Mathf.Sqrt (2), 0)
        };
        for (int index = 0; index < 9; index++) {
            var blockIns = GameObject.Instantiate (currCenterBlock);
            blockIns.transform.position = currCenterBlock.transform.position + neightborVectors[index];
            blockIns.cooord_x = currCenterBlock.cooord_x + location_coord[index][0];
            blockIns.cooord_y = currCenterBlock.cooord_y + location_coord[index][1];
            blockIns.InitColor ();
            currCenterBlock.SetNeightborBlocks (index, blockIns);
        }
        currCenterBlock.InitCenter ();
        currCenterBlock.gameObject.SetActive (false);

        //create UI Text
        GameObject canvasGO = new GameObject ();
        canvasGO.name = "Canvas";
        canvasGO.AddComponent<Canvas> ();
        canvasGO.AddComponent<CanvasScaler> ();
        canvasGO.AddComponent<GraphicRaycaster> ();

        // Get canvas from the GameObject.
        Canvas canvas;
        canvas = canvasGO.GetComponent<Canvas> ();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Create the Text GameObject.
        GameObject textGO = new GameObject ();
        textGO.transform.parent = canvasGO.transform;
        text = textGO.AddComponent<Text> ();
        text.fontSize = 1;
        text.alignment = TextAnchor.MiddleCenter;
    }

    // Update is called once per frame
    void Update () {

    }

    public int GetRandomColorIndex () {
        return Random.Range (0, colorArray.Length);
    }
    //方塊本身的顏色
    public Color GetColorByIndex (int index) {
        return colorArray[index];
    }
    public string GetRandomColorString () {
        int index = Random.Range (0, colorTextArray.Length);
        return colorTextArray[index];
    }
    private void GetText () {
        // 從 currCenterBlock 的相鄰方塊隨機取得 text 的顏色
        // 並且從 GetRandomColorString() 取得文字
        currColor = Random.Range (0, currCenterBlock.GetNeightborBlocks ().Length);
        text.text = GetRandomColorString ();
        text.transform.position = currCenterBlock.transform.position;
        text.color = GetColorByIndex (currColor);
    }
}