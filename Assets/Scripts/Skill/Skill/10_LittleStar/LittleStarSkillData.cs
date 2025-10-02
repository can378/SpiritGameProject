using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Item/Skill/LittleStarSkill")]

public class LittleStarSkillData : SkillData
{
    [field: SerializeField, Header("Information"), Tooltip("Little Skill Buff")] public BuffData LSBuff { get; private set; }
}
