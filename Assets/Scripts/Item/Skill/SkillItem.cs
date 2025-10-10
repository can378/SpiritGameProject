using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �������� �⺻ ����
// ����� �нú� ����
// ������ �⺻ ����
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
