using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Stats
{
    //EXP
    public int level = 1;
    public int exp = 1;
    public int point = 10;

    //Attack
    public float critical = 0;
    public float criticalDamage = 0.5f;
    public float drain = 0;
    public float attackSpeed = 1;

    //Skill
    public float skillPower = 1;
    public float skillCoolTime = 0; //��

    //move speed
    public float speed = 1;
    public float runSpeed = 1.66f;
    public float runCoolTime = 5f;

    //Dodge
    public float dodgeSpeed = 2f;
    public float dodgeTime = 0.5f;

    //Item
    public int coin = 0;
    public int key = 0;
    public int dice = 0;
    public string item = "";

    //Equipments
    public MainWeapon mainWeapon = null;
    public Skill skill = null;
    public int maxArmor = 3;
    public Armor[] armors = new Armor[3];

    //Stat
    public int[] playerStat = new int[8];
}
