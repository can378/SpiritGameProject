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
    public float playerHPMax;
    public float playerHP;
    public float playerTempHP;

    //Attack
    public float playerPower;
    public int playerCritical;
    public float playerCriticalDamage;
    public float playerDrain;
    public float playerAttackRange;
    public float playerAttackSpeed;
    

    //Move Speed
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
    public string passiveSkill;
    public int skillCoolTime;

    //Chpater
    public int nowChapter;

    //Stat
    public int[] playerStat;

    //?
    public float playerLuck;



}
