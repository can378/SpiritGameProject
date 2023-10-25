using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    public static ScriptManager instance;

    public TextAsset txt;
    string[,] Sentence;
    int lineSize, rowSize;


    void Start()
    {
        instance = this;
        LoadData();
    }


    void LoadData()
    {

        string currentText = txt.text.Substring(0, txt.text.Length - 1);
        //print(currentText);

        string[] line = currentText.Split('\n');//줄바꿈으로 구분
        lineSize = line.Length;
        rowSize = line[0].Split('\t').Length;//탭으로 구분

        Sentence = new string[lineSize, rowSize];


        for (int i = 0; i < lineSize; i++)
        {
            string[] row = line[i].Split("\t");
            for (int j = 0; j < rowSize; j++)
            { Sentence[i, j] = row[j]; }
        }
    }

    public void ScriptTest() 
    {
        //활용 예시
        for (int i = 0; i < lineSize; i++)
        {
            if (Sentence[i, 0] == "할머니") { print(Sentence[i, 1]); }
        }


    }
}
