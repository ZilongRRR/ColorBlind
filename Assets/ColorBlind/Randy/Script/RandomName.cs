using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomName : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField inputs;
    int[] choose = new int[3] { 0, 0, 0 };
    string[] adjs = new string[] { "無畏的", "硬硬的", "硬挺的", "軟嫩的", "lab做不出來的" , "下麵的", "樓上", "樓下", "濕黏"};
    string[] names = new string[] { "仕鴻", "家琛", "冠廷", "承洋", "信之", "Ling", "淳晴", "秦嘉", "Pei", "育誠",
                                    "東穎", "佳妤", "舜翔", "宇鵬", "韓粉", "子皓", "俊杉", "尚賢", "鎧禎", "翊良",
                                    "松霖", "君傑", "Shyan", "偉瀚", "東垣" };
    string[] animals = new string[] { "馬", "牛", "狗" , "貓", "豬", "馬爾濟斯", "福壽螺", "香菜"};
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void generateName()
    {
        inputs.text = "";
        choose = new int[3] { 0, 0, 0 };
        int index_adj = Random.Range(0, adjs.Length);
        int index_animal = Random.Range(0, animals.Length);
        int index_name = Random.Range(0, names.Length);
        string[] combination = new string[] { adjs[index_adj], names[index_name], animals[index_animal] };

        while ((choose[0]+ choose[1]+ choose[2]) == 0)
        {
            choose[0] = Random.Range(0, 2);
            choose[1] = Random.Range(0, 2);
            choose[2] = Random.Range(0, 2);
        }

        for(int i = 0; i < 3; i++)
        {
            if (choose[i] == 0)
            {
                continue;
            }
            inputs.text += combination[i];
        }
    }
}
