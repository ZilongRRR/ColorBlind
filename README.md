---
tags: course
---
# ICG2019 Final Project
![](https://i.imgur.com/DRNTqUI.png)
## Team member

r07521606 劉鎧禎 <br>
r06521605 許舜翔 <br>
r06521609 趙君傑 <br>

---

#### 方塊位置和index關係
![](https://i.imgur.com/Ri3FNdl.png)<br>
#### 相對座標邏輯關係
Blocks之間有相對座標的邏輯關係去掌握各個block的位置來創造neightborBlocks，以最初的block<b>(Genesis Block)</b>為(0,0)，0號為(-1,1)、1號為(0,1)...依此類推。
```csharp=
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
```
每個Block都有各自的neightborBlocks矩陣，大小為9(包含自己)，當被創造時neightborBlocks也會跟著被建出，也各自存在自己的相對座標，這些相對座標會用來確保每一個現有block都有對應到的neightborBlocks。
#### 點擊後處理
MapManager控管著現在的中心block(currCenterBlock)以及現在的文字顏色(currColor)，當玩家點擊周圍的block時會先確認玩家是否點擊currCenterBlock的neightborBlocks，再確認block的顏色是否為currColor去進行下一步的處理。 

## Feedback
* 點對：會沈下去，浮起來新的區塊
* 點錯：方塊會晃一下，整個螢幕晃動

## Color
### 中文字
|紅|橙|黃|綠|藍|靛|紫|橘|白|黑|灰|粉|棕|發|財|讚|
|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|-|
### 顏色
|r, g, b|
|-------|
|23, 28, 97|
|3, 110, 184|
|0, 162, 154|
|0, 152, 52|
|34, 172, 56|
|218, 224, 0|
|248, 182, 45|
|234, 85, 20|
|195, 13, 35|
|62, 58, 57|
|239, 239, 239|

## Script

### 方塊相關
MapManager.cs
```csharp=
public string[] colorTextArray;
public Color[] colorArray;
// 記錄現在正確的顏色
public int currColor;
public int Combo = 0;
public Block currCenterBlock;
public Text text;

//方塊本身的顏色位置
public int GetRandomColorIndex(){}
//方塊本身的顏色
public Color GetColorByIndex(int index){
    return colorArray[index];
}
public string GetRandomColorString(){}

private void GetText(){
    // 從 currCenterBlock 的相鄰方塊隨機取得 text 的顏色
    // 並且從 GetRandomColorString() 取得文字
}

```

Block.cs
```csharp=
// parameters
private Block[] neightborBlocks = new Block[9];
private bool isClick = false;
public int colorIndex;
public int cooord_x = 0;
public int cooord_y = 0;
public BlockFeedback blockFeedback;

// function
public void SetNeightborBlocks(int index, Block block){
    neightborBlocks[index] = block;
}
public void InitCenter(){
    // 判斷 neightborBlocks 哪幾個位置沒有方塊
    // 沒有方塊的 index 就產生方塊存入，並且呼叫 InitColor()
    // 並且把對應的方塊指給每一個 neightborBlock
    // 更新 MapManager 的 currCenterBlock 為自己
    // 更新 MapManager 的 currColor
}
public void InitColor(){
    // 跟 MapManager 取得隨機顏色的位置
    // 再從 MapManager 取得對應的顏色
}

private void OnMouseDown(){
    // unity 內建函數
    // 跟 MapManager 比對顏色的位置是否ㄧ樣
    if(same){
        InitCenter();
        blockFeedback.BecomeCenter();
    } else {
        blockFeedback.ClickError();
    }
}
public int GetRandomColorIndexFromNeightbor () {
    // 從周圍的方塊隨機取得一個顏色值
    // 要確保沒被點擊過
}

```
BlockFeedback.cs
```csharp
// 產生的效果
public void Generate (){}
// 點擊正確成為中心時效果
public void BecomeCenter(){}
// 點擊錯誤效果
public void ClickError(){}
```

### 遊戲流程
GameFlowManager.cs
```csharp
// 顏色點擊正確時增加combo及分數
public void ColorCorrect (){}
// 顏色錯誤取消combo
public void ColorError (){}
```

### 排行榜
LeaderBoardControl.cs
```csharp=
private void HandelShowStatus(string status){}

public void UpdateValue(List<Rank> rank){

}
```
FireBaseConnect.cs
```csharp=
public delegate void FireBaseStatusEvent(string status);
public event FireBaseStatusEvent OnShowStatus = (e) => { };
// Connection to Firebase App
public void FireBaseConnect(string domain_name = "ColorBlind"){

}
public void ReadData(){

}
// return a unique id after push data 
public string AddScoreToLeaders(string username, long score){

}
```
![](https://i.imgur.com/d46cfNw.png =500x750)

### 提示顯示
* 連線中
    * 連線成功
    * 連線失敗
* 讀取資料中

## 額外功能
* 重新連線
* 隨機名稱
    怕玩家上排行榜臨時想不到要取什麼名字，因此提供了隨機名稱的功能。
    ![](https://i.imgur.com/KEMgyAb.png)

* 廣告
    1. 遊戲畫面中橫式廣告看板
        有五種廣告樣式每5秒撥放。
        ![](https://i.imgur.com/uWUDAtG.png)
    2. 遊戲結束後嵌入式影片廣告
        以影片形式撥放，在撥放5秒後會有關閉鍵。
        ![](https://i.imgur.com/cFiprGW.png)
* 展現走過的路徑
    在遊戲結束時當下會呈現你走過的路徑來讓玩家看到自己擴展過的版圖。鏡頭計算方式於`CameraManager.cs`
    ![](https://i.imgur.com/kn4UIuz.gif)

## Game Scene
![](https://i.imgur.com/0OZQdH7.gif)<br>
![](https://i.imgur.com/FN2qCiM.gif)<br>
![](https://i.imgur.com/nTnViNK.gif)

