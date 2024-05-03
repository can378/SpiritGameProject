using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor;

/*
public enum StatName 
{ Hp, AttackPower, AttackSpeed, CriChance, CriPower, SkillPower, SkillCoolTime, MoveSpeed };
*/
public class StatSlot : MonoBehaviour
{
    //public StatName statName;
    public int statIndex;
    //public TMP_Text statLv;

    public bool isClick = false;


    TMP_Text statTitle;
    Image statImage;

    void Awake()
    {
        statTitle = GetComponentInChildren<TMP_Text>();
        statImage = GetComponentInChildren<Image>();
    }
    
    void Start()
    {
        //statLv.text = Player.instance.stats.playerStat[statIndex].ToString();
    }

    public void ButtonDown()
    {
        isClick = true;
        statBtn();
    }

    public void ButtonUp()
    {
        isClick = false;
    }

    public void statBtn()
    {
        Player.instance.playerStats.playerStat[statIndex]++;
        Player.instance.statApply();
        MapUIManager.instance.UpdatePointUI();
        //MapUIManager.instance.statSelectPanel.SetActive(false);
        //Player.instance.nearObject.GetComponent<Altar>().check = true;
    }

    public void UpdateStatSelectUI(int statIndex)
    {
        this.statIndex = statIndex;
        switch (this.statIndex)
        {
            case 0:
                statTitle.text = "�ִ� ü�� ����";
                break;
            case 1:
                statTitle.text = "���ݷ� ����";
                break;
            case 2:
                statTitle.text = "���ݼӵ� ����";
                break;
            case 3:
                statTitle.text = "ġ��Ÿ Ȯ�� ����";
                break;
            case 4:
                statTitle.text = "ġ��Ÿ ���ط� ����";
                break;
            case 5:
                statTitle.text = "���� ����";
                break;
            case 6:
                statTitle.text = "���� ���� ���ð� ����";
                break;
            case 7:
                statTitle.text = "�̵��ӵ� ����";
                break;
            default:
                break;
        }
    }
}
