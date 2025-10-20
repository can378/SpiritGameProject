using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class SkillInstance : ItemInstance
{
    public SkillData skillData; // SO ����

    public override void Init()
    {
        itemData = skillData;
    }

}
