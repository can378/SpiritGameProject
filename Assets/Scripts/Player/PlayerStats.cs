using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Stats
{
    //EXP
    //public int level = 1;
    //public int exp = 1;
    //public int point = 10;

    //Attack

    // Critical
    // UI : 치명타 확률 0%
    // 치명타 = 무작위(0 ~ 100) < 치명타 확률 * 100 ? 참 : 거짓
    // 최소 0%, 최대 100%
    [field: SerializeField] public Stat CriticalChance = new Stat(0.0f, 1.0f, 0.0f);

    //CriticalDamage
    // UI : 치명타 피해량 150%
    // 피해량 = 치명타 ? 치명타 피해량 * 기본 피해량 : 기본 피해량
    // 최소 100%, 최대 300%
    [field: SerializeField] public Stat CriticalDamage { get; set; } = new Stat(1.5f, 3.0f, 1.0f);

    // attackSpeed
    // UI 0%
    // 높을수록 빨라짐
    // 초당 공격속도 = 무기 초당 공격 속도 * 플레이어 공격속도
    // 최소 0%, 최대 300%
    [field: SerializeField] public Stat AttackSpeed { get; set; } = new Stat(1.0f, 3.0f, 0.33f);


    // SkillCoolTime
    // 도술 재사용 대기 시간
    // UI 재사용 대기 시간 0%
    // 낮을수록 도술 자수 사용 가능
    // 도술 재사용 대기 시간 = 도술 기본 재사용 대기 시간 * 재사용 대기시간
    // 최소 -80% ,최대 80%
    [field: SerializeField] public Stat SkillCoolTime = new Stat(0.0f, 0.8f, -0.8f);

    // Move

    // 달리기 폐기
    /*
    // RunSpeed
    // UI 달리기 시 이동속도 50%
    // 달리기 시 속도
    // 이동속도 = 이동속도 (달리기 ? 달리기 시 속도 : 1)
    // 최소 100%
    [field: SerializeField] public float defaultRunSpeed { get; set; } = 1f;
    public float addRunSpeed { get; set; }
    public float increasedRunSpeed {  get; set; }
    public float decreasedRunSpeed { get; set; }
    public float runSpeed
    {
        get { return Mathf.Clamp((defaultRunSpeed + addRunSpeed) * (1f + increasedRunSpeed) * (1f - decreasedRunSpeed), 0f, 3f); }
    }


    //RunCoolTime
    // 달리기 재사용 대기 시간
    // UI : 달리기 재사용 대기 시간 5초
    // 달리기 재사용 대기 시간 = 달리기 재사용 대기 시간
    // 최소 0초
    [field: SerializeField] public float defaultRunCoolTime { get; set; } = 5f;
    public float addRunCoolTime { get; set; }
    public float increasedRunCoolTime { get; set; }
    public float decreasedRunCoolTime {  get; set; }
    public float runCoolTime
    {
        get { return Mathf.Clamp((defaultRunCoolTime + addRunCoolTime) * (1f + increasedRunCoolTime) * (1f - decreasedRunCoolTime), 0f, 10f); }
    }
    */

    // Dodge
    // 회피 속도
    // UI : 회피 시 추가 이동 속도 66%
    // 최소 50%
    [field: SerializeField] public float defaultDodgeSpeed { get; set; } = 0.66f;
    public float addDodgeSpeed { get; set; }
    public float increasedDodgeSpeed { get; set; }
    public float decreasedDodgeSpeed { get; set; }

    public float dodgeSpeed
    {
        get
        {
            return Mathf.Clamp(
                (defaultDodgeSpeed + addDodgeSpeed) * (1f + increasedDodgeSpeed) * (1f - decreasedDodgeSpeed), 0f, 2f);
        }
    }

    // 회피 시간
    // UI : 회피 시간 0.8초
    // 회피 시간 = 회피 시간
    // 최소 0.1초
    [field: SerializeField] public float defaultDodgeTime { get; set; } = 0.4f;
    public float addDodgeTime { get; set; }
    public float increasedDodgeTime { get; set; }
    public float decreasedDodgeTime { get; set; }

    public float dodgeTime
    {
        get
        {
            return Mathf.Clamp(
                (defaultDodgeTime + addDodgeTime) * (1f + increasedDodgeTime) * (1f - decreasedDodgeTime), 0.1f, 0.8f);
        }
    }

    //n초간 실명
    //-->cameraManager에서 효과적용
    public float blind;

    //Item
    public int coin;
    public int key;
    public int dice;
    public int item;

    //Equipments
    public PlayerWeapon weapon;

    public int maxSkillSlot;
    public SkillInstance[] skill = new SkillInstance[5];

    public int maxEquipment;
    public EquipmentInstance[] equipments = new EquipmentInstance[5];

    //Stat
    public int[] playerStat = new int[8];
}
