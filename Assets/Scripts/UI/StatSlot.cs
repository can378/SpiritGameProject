using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
public enum StatName 
{ Hp, AttackPower, AttackSpeed, CriChance, CriPower, SkillPower, SkillCoolTime, MoveSpeed };
*/
public class StatSlot : MonoBehaviour
{
    //public StatName statName;
    public int statIndex;
    //public TMP_Text statLv;

    private float clickTime;
    public float minClickTime = 1;
    private bool isClick;
    
    void Start()
    {
        //statLv.text = Player.instance.stats.playerStat[statIndex].ToString();
    }

    void Update()
    {
        if (isClick)
        {
            clickTime += Time.deltaTime;
            if (clickTime >= minClickTime)
            {
                print("ButtonHold");
                statBtn();
                isClick = false;
            }
        }
        else {
            clickTime = 0;
        }
    }

    public void ButtonDown()
    {
        isClick = true;
    }

    public void ButtonUp()
    {
        isClick = false;
    }

    public void statBtn()
    {
        if (Player.instance.stats.point > 0)
        {

            Player.instance.stats.playerStat[statIndex]++;
            //statLv.text = Player.instance.stats.playerStat[statIndex].ToString();

            //Player.instance.stats.point--;
            MapUIManager.instance.UpdatePointUI();

            Player.instance.statApply();
        }
        else { print("no enough points"); }

    }
}
