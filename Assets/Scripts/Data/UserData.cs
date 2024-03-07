using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//플레이어 개인의 획득 정보 등..

public class UserData
{
    //EXP
    public int playerLevel;
    public int playerExp;
    public int playerPoint;

    //HP
    public float playerHP;
    public float playerHPMax;
    public float playerTempHP;
    
    //Item
    public int playerCoin;
    public int playerKey;
    public int playerDice;

    public string playerItem;

    //Equipment
    public int playerMainWeapon;
    public int playerMaxArmor;
    public int[] playerArmor = new int[3];

    //Skill
    public int playerSkill;

    //Chpater
    public int nowChapter;

    //Stat
    public int[] playerStat = new int[8];

}
