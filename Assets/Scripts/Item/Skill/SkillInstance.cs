using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class SkillInstance : ItemInstance
{
    public SkillData skillData; // SO ÂüÁ¶

    public override void Init()
    {
        itemData = skillData;
    }

    public void SetSI(SkillData _SkillData)
    {
        skillData = _SkillData;
        itemData = skillData;
    }

}
