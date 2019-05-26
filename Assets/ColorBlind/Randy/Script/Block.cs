using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZTools;

public class Block : MonoBehaviour {
    private Block[] neightborBlocks = new Block[8];
    private bool isClick = false;
    public int colorIndex;
    
    //public BlockFeedback blockFeedback;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetNeightborBlocks(int index, Block block)
    {
        neightborBlocks[index] = block;
    }
    public Block[] GetNeightborBlocks()
    {
        return neightborBlocks;
    }
    public void InitCenter()
    {
        // 判斷 neightborBlocks 哪幾個位置沒有方塊
        // 沒有方塊的 index 就產生方塊存入，並且呼叫 InitColor()
        // 並且把對應的方塊指給每一個 neightborBlock
        // 更新 MapManager 的 currCenterBlock 為自己
        // 更新 MapManager 的 currColor
        List<int> null_index = new List<int>();
        for (int i = 0; i < this.neightborBlocks.Length; i++)
        {
            if (this.neightborBlocks[i] == null)
                null_index.Add(i);
        }

        foreach (int index in null_index)
        {
            var blockIns = GameObject.Instantiate(this);
            blockIns.transform.position = this.transform.position + MapManager.Instance.neightborVectors[index];
            blockIns.InitColor();
            SetNeightborBlocks(index, blockIns);
        }

        MapManager.Instance.currCenterBlock = this;
        MapManager.Instance.currColor = this.colorIndex;
    }
    public void InitColor()
    {
        // 跟 MapManager 取得隨機顏色的位置
        // 再從 MapManager 取得對應的顏色

        colorIndex = MapManager.Instance.GetRandomColorIndex();
        this.GetComponent<MeshRenderer>().material.color = MapManager.Instance.GetColorByIndex(colorIndex);
        Debug.Log(colorIndex + "   " + this.GetComponent<MeshRenderer>().material.color.ToString());
    }
    /*
    private void OnMouseClick()
    {
        // unity 內建函數
        // 跟 MapManager 比對顏色的位置是否ㄧ樣
        if (same)
        {
            InitCenter();
            blockFeedback.BecomeCenter();
        }
        else
        {
            blockFeedback.ClickError();
        }
    }*/
}