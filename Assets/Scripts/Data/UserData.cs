using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//�÷��̾� ������ ȹ�� ���� ��..

public class UserData
{
    //EXP
    public int playerLevel;
    public int playerExp;
    public int playerPoint;
    
    //HP
    public float playerHPMax;
    public float playerHP;
    public float playerTempHP;

    //���� ����
    public float playerReductionRatio;
    public float[] playerResist = new float[11];

    //Attack
    public float playerPower;
    public float playerCritical;
    public float playerCriticalDamage;
    public float playerDrain;
    public float playerAttackRange;
    public float playerAttackSpeed;

    //Skill
    public float skillPower;
    public float skillCoolTime; //��
    

    //Move Speed
    public float playerDefaultSpeed;
    public float playerSpeed;
    public float playerRunSpeed;
    public float playerRunCoolTime;

    //Dodge
    public float playerDodgeSpeed;
    public float playerDodgeTime;

    //Item
    public int coin;
    public int key;
    public int dice;

    public string playerItem;

    //Equipment
    public string mainWeapon;
    public string[] armor = new string[3];

    //Skill
    public string activeSkill;

    //Chpater
    public int nowChapter;

    //Stat
    public int[] playerStat;

    //?
    public float playerLuck;



}
