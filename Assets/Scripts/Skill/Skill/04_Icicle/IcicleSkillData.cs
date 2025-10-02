using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Item/Skill/IcicleSkill")]

public class IcicleSkillData : SkillData
{
    [field: SerializeField, Header("Information"), Tooltip("기본 피해량")] public int defaultDamage { get; private set; }
    [field: SerializeField, Tooltip("도력 계수")] public float ratio { get; private set; }
    [field: SerializeField, Tooltip("넉백 수치")] public float knockBack { get; private set; }


    [field: SerializeField, Tooltip("투사체 크기")] public float defaultSize { get; private set; }
    [field: SerializeField, Tooltip("투사체 속도")] public float projectileSpeed { get; private set; }
    [field: SerializeField, Tooltip("투사체 시간")] public float projectileTime { get; private set; }

    [field: SerializeField, Tooltip("둔화 상태이상")] public BuffData[] statusEffect { get; private set; }

    [field: SerializeField, Header("GameObject"), Tooltip("이펙트 프리팹")] public GameObject iciclePrefab { get; private set; }
    [field: SerializeField, Tooltip("이펙트 시뮬 프리팹")] public GameObject icicleSimulPrefab { get; private set; }




}
