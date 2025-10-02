using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/SkillPowerPlus")]

public class SkillPowerPlus : PassiveData
{
    // 도력 증가
    // + 수치
    [SerializeField] int variation;
    public override void Update_Passive(ObjectBasic _User)
    { }
    public override void Apply(ObjectBasic target)
    {
        Debug.Log("플레이어 도력 +" + variation + " 증가");
        target.stats.SkillPower.AddValue += variation;
    }

    // Update is called once per frame
    public override void Remove(ObjectBasic target)
    {
        target.stats.SkillPower.AddValue -= variation;
    }

    public override string Update_Description(Stats _Stats)
    {
        return string.Format(PDescription, variation);
    }
}
