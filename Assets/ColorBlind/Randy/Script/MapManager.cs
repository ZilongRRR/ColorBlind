using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using ZTools;

public class MapManager : Singleton<MapManager> {
    // Start is called before the first frame update
    [Header ("鏡頭設定")]
    public Transform camTrans;
    public float camTranslateDuration = 0.3f;
    [Header ("顏色設定")]
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
        new Color (0f, 162f / 255f, 154f / 255f), new Color (34f / 255f, 172f / 255f, 56f / 255f),
        new Color (218f / 255f, 224f / 255f, 0f), new Color (248f / 255f, 182f / 255f, 45f / 255f),
        new Color (234f / 255f, 85f / 255f, 20f / 255f), new Color (195f / 255f, 13f / 255f, 35f / 255f),
        new Color (62f / 255f, 58f / 255f, 57f / 255f), new Color (239f / 255f, 239f / 255f, 239f / 255f)
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
    public List<GameObject> allBlocks = new List<GameObject> ();
    // 記錄現在正確的顏色
    public int currColor;
    public int Combo = 0;
    [Header ("初始方塊設定")]
    public GameObject blockPrefab;
    public Vector3 initBlockPosition;
    public Block currCenterBlock;
    [Header ("UI")]
    public Text text;
    public Canvas canvas;
    public Sprite[] images;
    int spritesIndex = 0;
    [SerializeField, Header ("方塊間距")]
    private float distance = 2f;

    void Start () {
        canvas.GetComponentInChildren<Image>().sprite = images[spritesIndex];
        this.InvokeRepeating("changeADSprites", 0, 5f);

        initBlockPosition = new Vector3(camTrans.position.x, camTrans.position.y, 0);
        neightborVectors = new Vector3[9] {
            new Vector3 (0, 1 * distance * Mathf.Sqrt (2), 0),
            new Vector3 (1 * distance / Mathf.Sqrt (2), 1 * distance / Mathf.Sqrt (2), 0),
            new Vector3 (1 * distance * Mathf.Sqrt (2), 0, 0),
            new Vector3 (-1 * distance / Mathf.Sqrt (2), 1 * distance / Mathf.Sqrt (2), 0),
            new Vector3 (0, 0, 0),
            new Vector3 (1 * distance / Mathf.Sqrt (2), -1 * distance / Mathf.Sqrt (2), 0),
            new Vector3 (-1 * distance * Mathf.Sqrt (2), 0, 0),
            new Vector3 (-1 * distance / Mathf.Sqrt (2), -1 * distance / Mathf.Sqrt (2), 0),
            new Vector3 (0, -1 * distance * Mathf.Sqrt (2), 0)
        };
        for (int index = 0; index < 9; index++) {
            Block blockIns = GameObject.Instantiate (blockPrefab).GetComponent<Block> ();
            allBlocks.Add (blockIns.gameObject);
            blockIns.transform.position = initBlockPosition + neightborVectors[index];
            blockIns.cooord_x = location_coord[index][0];
            blockIns.cooord_y = location_coord[index][1];
            blockIns.InitColor ();
            if (index == 4) {
                currCenterBlock = blockIns;
            }
        }
        currCenterBlock.InitCenter ();
    }

    public int GetRandomColorIndex () {
        return Random.Range (0, colorArray.Length);
    }
    //方塊本身的顏色
    public Color GetColorByIndex (int index) {
        return colorArray[index];
    }
    public void NextStep (Block nextBlock) {
        currCenterBlock = nextBlock;
        Vector3 newPos = new Vector3 (nextBlock.transform.position.x, nextBlock.transform.position.y, camTrans.position.z);
        camTrans.DOMove (newPos, camTranslateDuration);
        GetText ();
    }

    public string GetRandomColorString () {
        int index = Random.Range (0, colorTextArray.Length);
        return colorTextArray[index];
    }
    private void GetText () {
        // 從 currCenterBlock 的相鄰方塊隨機取得 text 的顏色
        // 並且從 GetRandomColorString() 取得文字
        currColor = currCenterBlock.GetRandomColorIndexFromNeightbor ();
        text.text = GetRandomColorString ();
        text.color = GetColorByIndex (currColor);
    }

    private void changeADSprites()
    {
        spritesIndex++;
        if (spritesIndex == images.Length)
            spritesIndex = 0;
        canvas.GetComponentInChildren<Image>().sprite = images[spritesIndex];
    }
}