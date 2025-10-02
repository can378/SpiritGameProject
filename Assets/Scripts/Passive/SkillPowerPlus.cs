using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/SkillPowerPlus")]

public class SkillPowerPlus : PassiveData
{
    // ���� ����
    // + ��ġ
    [SerializeField] int variation;
    public override void Update_Passive(ObjectBasic _User)
    { }
    public override void Apply(ObjectBasic target)
    {
        Debug.Log("�÷��̾� ���� +" + variation + " ����");
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
