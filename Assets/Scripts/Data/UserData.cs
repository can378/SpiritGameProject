using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//ÇÃ·¹ÀÌ¾î °³ÀÎÀÇ È¹µæ Á¤º¸ µî..

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
    public float playerReductionRatio;

    //Attack
    public float playerPower;
    public float playerCritical;
    public float playerCriticalDamage;
    public float playerDrain;
    public float playerAttackRange;
    public float playerAttackSpeed;


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

    //Weapon
    public string mainWeapon;
    public string subWeapon;

    //Skill
    public string activeSkill;
    public float skillCoolTime; //Äð°¨

    //Chpater
    public int nowChapter;

    //Stat
    public int[] playerStat;

    //?
    public float playerLuck;



}
