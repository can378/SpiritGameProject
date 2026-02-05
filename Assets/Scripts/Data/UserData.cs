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

    public int playerItem;

    //Equipment
    public int playerWeapon;
    public int playerMaxEquipments;
    public int[] playerEquipments = new int[5];

    //Skill
    public int playerMaxSkillSlot;
    public int[] playerSkill = new int[5];

    //Chpater
    public int nowChapter;

    //Stat
    public int[] playerStat = new int[(int)Player.StatID.END];

}
