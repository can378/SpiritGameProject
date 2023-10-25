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

        string[] line = currentText.Split('\n');//�ٹٲ����� ����
        lineSize = line.Length;
        rowSize = line[0].Split('\t').Length;//������ ����

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
        //Ȱ�� ����
        for (int i = 0; i < lineSize; i++)
        {
            if (Sentence[i, 0] == "�ҸӴ�") { print(Sentence[i, 1]); }
        }


    }
}
