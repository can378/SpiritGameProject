using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �������� �⺻ ����
// ����� �нú� ����
// ������ �⺻ ����
public class SkillItem : SelectItem
{
    [field: SerializeField] public SkillInstance skillInstance { get; protected set; }

    protected void Awake()
    {
        itemInstance = skillInstance;
        itemInstance.Init();

    }

    protected void OnValidate()
    {
        itemInstance = skillInstance;
        itemInstance.Init();

    }
}
