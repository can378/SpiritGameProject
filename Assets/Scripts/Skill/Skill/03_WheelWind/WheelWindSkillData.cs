using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Item/Skill/WheelWindSkill")]

public class WheelWindSkillData : SkillData
{
    [field: SerializeField, Header("Information"), Tooltip("기본 피해량")] public int defaultDamage { get; private set; }
    [field: SerializeField, Tooltip("공격력 계수")] public float ratio { get; private set; }

    [field: SerializeField, Tooltip("초당 타격 횟수")] public int DPS { get; private set; }
    [field: SerializeField, Tooltip("기본 크기")] public float defaultSize { get; private set; }


    [field: SerializeField, Header("GameObject"), Tooltip("이펙트 프리팹")] public GameObject WheelWindPrefab { get; private set; }



}
