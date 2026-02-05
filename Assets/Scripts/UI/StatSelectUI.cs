using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;
using Unity.VisualScripting;

public class StatSelectUI : MonoBehaviour
{
    public StatSlot[] statSlots = new StatSlot[3];
    public Altar curAltar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CloseStatSelectUI();
    }

    public void SetStatSelectUI(Altar altar)
    {
        curAltar = altar;
        for (int i = 0; i < statSlots.Length; i++)
        {
            //statSlots[i].UpdateStatSelectUI(altar.table[i]);
        }
    }

    public void ExitStatSelectUI()
    {
        curAltar = null;
        for (int i = 0; i < statSlots.Length; i++)
        {
            statSlots[i].UpdateStatSelectUI(-1);
        }
    }

    void CloseStatSelectUI()
    {
        if(curAltar == null)
            return;
        
        for(int i = 0 ; i <statSlots.Length ; i++)
        {
            if(statSlots[i].isClick)
            {
                MapUIManager.instance.statSelectPanel.SetActive(false);
                curAltar.check = true;
                curAltar = null;
                statSlots[i].isClick = false;
            }
        }
    }
}
