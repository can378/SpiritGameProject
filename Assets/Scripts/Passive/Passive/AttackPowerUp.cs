using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/AttackPowerUp")]
public class AttackPowerUp : PassiveData
{
    // 공격력 증가
    // +%

    [SerializeField] float variation;

    public override void Update_Passive(ObjectBasic _User)
    { }

    public override void Apply(ObjectBasic _User)
    {
        _User.stats.AttackPower.IncreasedValue += variation;
    }

    // Update is called once per frame
    public override void Remove(ObjectBasic _User)
    {
        _User.stats.AttackPower.IncreasedValue -= variation;
    }

    public override string Update_Description(Stats _Stats)
    {
        return string.Format(PDescription, variation * 100);
    }
}
