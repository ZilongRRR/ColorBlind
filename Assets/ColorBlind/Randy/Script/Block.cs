using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZTools;

public class Block : MonoBehaviour {
    private Block[] neightborBlocks = new Block[9];
    public GameObject tmpblock;
    private bool isClick = false;
    public int colorIndex;

    public int cooord_x = 0;
    public int cooord_y = 0;

    //public BlockFeedback blockFeedback;
    void Start () {

    }

    // Update is called once per frame
    void Update () { }

    public void SetNeightborBlocks (int index, Block block) {
        neightborBlocks[index] = block;
    }
    public Block[] GetNeightborBlocks () {
        return neightborBlocks;
    }
    public void InitCenter () {
        // 判斷 neightborBlocks 哪幾個位置沒有方塊
        // 沒有方塊的 index 就產生方塊存入，並且呼叫 InitColor()
        // 並且把對應的方塊指給每一個 neightborBlock
        // 更新 MapManager 的 currCenterBlock 為自己
        // 更新 MapManager 的 currColor
        List<int> null_index = new List<int> ();
        GameObject[] _Blocks = GameObject.FindGameObjectsWithTag ("Block");
        foreach (KeyValuePair<int, int[]> item in MapManager.Instance.location_coord) {
            bool check = false;
            foreach (GameObject go in _Blocks) {
                if (go.GetComponent<Block> ().cooord_x == this.cooord_x + item.Value[0] && go.GetComponent<Block> ().cooord_y == this.cooord_y + item.Value[1]) {
                    check = true;
                    this.neightborBlocks[item.Key] = go.GetComponent<Block> ();
                }
            }
            if (item.Key != 4 && !check)
                null_index.Add (item.Key);
        }

        string s = "";
        foreach (int i in null_index) s += i + " ";
        //Debug.Log(s);

        foreach (int index in null_index) {
            var blockIns = GameObject.Instantiate (this);
            blockIns.gameObject.SetActive (true);
            blockIns.transform.position = this.transform.position + MapManager.Instance.neightborVectors[index];
            blockIns.cooord_x = this.cooord_x + MapManager.Instance.location_coord[index][0];
            blockIns.cooord_y = this.cooord_y + MapManager.Instance.location_coord[index][1];
            blockIns.InitColor ();
            SetNeightborBlocks (index, blockIns);
        }

        MapManager.Instance.currCenterBlock = this;
        MapManager.Instance.currColor = this.colorIndex;
    }
    public void InitColor () {
        // 跟 MapManager 取得隨機顏色的位置
        // 再從 MapManager 取得對應的顏色

        colorIndex = MapManager.Instance.GetRandomColorIndex ();
        this.GetComponent<MeshRenderer> ().material.color = MapManager.Instance.GetColorByIndex (colorIndex);
    }
    private void OnMouseDown () {
        // unity 內建函數
        // 跟 MapManager 比對顏色的位置是否ㄧ樣
        if (!isClick) {
            for (int i = 0; i < 9; i++) {
                if (MapManager.Instance.currCenterBlock.neightborBlocks[i] == this) {
                    Color color = gameObject.GetComponent<MeshRenderer> ().material.color;
                    color.a = 0f;
                    gameObject.GetComponent<MeshRenderer> ().material.color = color;
                    MapManager.Instance.clicked_blocks.Add (new int[] { cooord_x, cooord_y });

                    InitCenter ();
                    this.isClick = true;
                }
            }
        }
        /*if (same)
        {
            InitCenter();
            blockFeedback.BecomeCenter();
        }
        else
        {
            blockFeedback.ClickError();
        }*/
    }

    public static void SetMaterialRenderingModeTransparent (Material material) {
        material.SetInt ("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.One);
        material.SetInt ("_DstBlend", (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt ("_ZWrite", 0);
        material.DisableKeyword ("_ALPHATEST_ON");
        material.DisableKeyword ("_ALPHABLEND_ON");
        material.EnableKeyword ("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;
    }
}