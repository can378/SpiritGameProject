using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 아이템의 기본 정보
// 장비의 패시브 정보
// 무기의 기본 정보
public class SkillItem : SelectItem
{
    [field: SerializeField] public SkillData skillData { get; protected set; }

    protected void Awake()
    {
        itemData = skillData;
    }

    protected void OnValidate()
    {
        itemData = skillData;
    }
}
