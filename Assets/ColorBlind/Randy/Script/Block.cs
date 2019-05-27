﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZTools;

public class Block : MonoBehaviour {
    [SerializeField]
    private Block[] neightborBlocks = new Block[9];
    private bool isClick = false;
    public int colorIndex;

    public int cooord_x = 0;
    public int cooord_y = 0;

    public BlockFeedback blockFeedback;
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
        foreach (KeyValuePair<int, int[]> item in MapManager.Instance.location_coord) {
            bool check = false;
            foreach (GameObject go in MapManager.Instance.allBlocks) {
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
            MapManager.Instance.allBlocks.Add (blockIns.gameObject);
            blockIns.transform.position = this.transform.position + MapManager.Instance.neightborVectors[index];
            blockIns.cooord_x = this.cooord_x + MapManager.Instance.location_coord[index][0];
            blockIns.cooord_y = this.cooord_y + MapManager.Instance.location_coord[index][1];
            blockIns.InitColor ();
            SetNeightborBlocks (index, blockIns);
        }
        MapManager.Instance.NextStep (this);
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
            foreach(Block b in MapManager.Instance.currCenterBlock.neightborBlocks)
            {
                if(b == this && b != MapManager.Instance.currCenterBlock)
                {
                    // 確認點擊正確
                    if (colorIndex == MapManager.Instance.currColor)
                    {
                        // 點選到正確的動態回饋
                        this.isClick = true;
                        Color color = gameObject.GetComponent<MeshRenderer>().material.color;
                        color.a = 0f;
                        gameObject.GetComponent<MeshRenderer>().material.color = color;
                        MapManager.Instance.clicked_blocks.Add(new int[] { cooord_x, cooord_y });
                        // 判斷周圍是否還有可以點擊的位置

                        InitCenter();
                        // 周圍都沒有可點擊點
                        int clicked_num = 0;
                        foreach(Block tmp_b in this.neightborBlocks)
                        {
                            if (tmp_b.isClick)
                                clicked_num++;
                            else
                                break;
                        }
                        if(clicked_num == 9)
                        {
                            foreach(GameObject tmp_go in MapManager.Instance.allBlocks)
                            {
                                if(tmp_go.GetComponent<Block>().isClick==false)
                                {
                                    tmp_go.GetComponent<Block>().isClick = true;
                                    Color c = tmp_go.GetComponent<MeshRenderer>().material.color;
                                    c.a = 0f;
                                    tmp_go.GetComponent<MeshRenderer>().material.color = c;
                                    MapManager.Instance.clicked_blocks.Add(new int[] { tmp_go.GetComponent<Block>().cooord_x, tmp_go.GetComponent<Block>().cooord_y });
                                    tmp_go.GetComponent<Block>().InitCenter();
                                }
                            }
                        }
                    }
                    else
                    {
                        // 點擊錯誤的動態
                    }
                }
            }
        }
    }

    public int GetRandomColorIndexFromNeightbor () {
        // 從周圍的方塊隨機取得一個顏色值
        // 要確保沒被點擊過
        List<int> color_indexes = new List<int>();
        foreach(Block b in neightborBlocks)
        {
            if (b != this && !b.isClick)
                color_indexes.Add(b.colorIndex);
        }
        return color_indexes[Random.Range(0, color_indexes.Count)];
    }
}