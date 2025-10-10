using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Item/Skill/LittleStarSkill")]

public class LittleStarSkillData : SkillData
{
    [field: SerializeField, Header("Information"), Tooltip("Little Skill Buff")] public LittleStarBuff LSBuff { get; private set; }

    public override string Update_NumText(Stats _Stats)
    {
        return (LSBuff.defalutDamage + LSBuff.ratio * _Stats.SkillPower.Value).ToString() + " X 3";
    }
}
