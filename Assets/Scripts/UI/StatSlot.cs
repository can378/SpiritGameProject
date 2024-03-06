using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum StatName 
{ Hp, AttackPower, AttackSpeed, CriChance, CriPower, SkillPower, SkillCoolTime, MoveSpeed };
public class StatSlot : MonoBehaviour
{
    public StatName statName;
    public int statIndex;
    public TMP_Text statLv;

    void Start()
    {
        statLv.text = Player.instance.stats.playerStat[statIndex].ToString();
    }


    public void statBtn()
    {
        if (Player.instance.stats.point > 0)
        {

            Player.instance.stats.playerStat[statIndex]++;
            statLv.text = Player.instance.stats.playerStat[statIndex].ToString();

            Player.instance.stats.point--;
            MapUIManager.instance.UpdatePointUI();

            Player.instance.statApply();
        }
        else { print("no enough points"); }

    }
}
