using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    public static ScriptManager instance;

    public TextAsset txt;//대본 txt 파일

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

        string[] line = currentText.Split('\n');//txt파일의 한줄 한줄
        
        lineSize = line.Length;
        rowSize = line[0].Split('\t').Length;

        Sentence = new string[lineSize, rowSize];//문장들


        for (int i = 0; i < lineSize; i++)
        {
            string[] row = line[i].Split("\t");
            for (int j = 0; j < rowSize; j++)
            { Sentence[i, j] = row[j]; }
        }

    }

    public void ScriptTest()//활용 예시
    {
        //for (int i = 0; i < lineSize; i++)
        //{    if (Sentence[0,i, 0] == "할머니") { print(Sentence[0,i, 1]); }    }
    }


    public string NPCScript(int chapter,int index) 
    {
        if (Sentence[index % lineSize, 0] == chapter.ToString())
        {
            if (Sentence[index % lineSize, 1] == "npc")
            {
                return Sentence[index % lineSize, 2];
            }
            else if (Sentence[index % lineSize, 1] == "player")
            {
                return Sentence[index % lineSize, 2];
            }
            else if (Sentence[index % lineSize, 1] == "border")
            {
                return "border";
            }
        }
        
        return "wrong";
    }


}
