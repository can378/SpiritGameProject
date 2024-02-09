using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum StatName { Hp, AttackPower, AttackSpeed, AttackRange, MoveSpeed, Dodge, SkillCoolTime };
public class StatSlot : MonoBehaviour
{
    public StatName statName;
    public int statIndex;
    public TMP_Text statLv;


    UserData userdata;

    void Start()
    {
        userdata = DataManager.instance.userData;

        statLv.text = userdata.playerStat[statIndex].ToString();
    }


    public void statBtn()
    {
        if (userdata.playerPoint > 0)
        {

            userdata.playerStat[statIndex]++;
            statLv.text = userdata.playerStat[statIndex].ToString();
            
            userdata.playerPoint--;
            MapUIManager.instance.UpdatePointUI();

            statApply();
        }
        else { print("no enough points"); }

    }

    void statApply() 
    { 
        switch (statName) 
        {
            case StatName.Hp:
                //HP up
                userdata.playerHPMax += 10;
                break;
            case StatName.AttackPower:
                //attack power up
                userdata.playerPower++;
                break; 
            case StatName.AttackSpeed:
                //attack speed up
                userdata.playerAttackSpeed++;
                break;
            case StatName.AttackRange:
                userdata.playerAttackRange++;
                //attack range up
                break;
            case StatName.MoveSpeed:
                //move speed
                userdata.playerSpeed += 1;
                userdata.playerRunSpeed += 1;
                userdata.playerRunCoolTime -= 1;
                break;
            case StatName.Dodge:
                //dodge
                userdata.playerDodgeSpeed += 1;
                userdata.playerDodgeTime -= 1;
                break;
            case StatName.SkillCoolTime:
                //skill cool time
                userdata.skillCoolTime -= 1;
                break;
            default:
                print("stat apply error");
                break;
        
        
        }
    
    }
}
