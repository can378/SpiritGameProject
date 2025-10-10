using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/Immunity")]
public class Immunity : PassiveData
{

    [SerializeField] BuffType ImmunBuffType;

    public override void Update_Passive(ObjectBasic _User)
    { }

    public override void Apply(ObjectBasic _User)
    {
        _User.stats.SEResist[(int)ImmunBuffType].AddValue++;
        Debug.Log(ImmunBuffType + "번호 디버프에 면역");
    }

    // Update is called once per frame
    public override void Remove(ObjectBasic _User)
    {
        _User.stats.SEResist[(int)ImmunBuffType].AddValue--;
    }

    public override string Update_Description(Stats _Stats)
    {
        return string.Format(PDescription, ImmunBuffType);
    }
}
