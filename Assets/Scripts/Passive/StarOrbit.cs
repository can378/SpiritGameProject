using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/StarOrbit")]
public class StarOrbit : PassiveData
{
    // 활성화 시 버프 형태로 별
    // 현재 사용 불가

    [field: SerializeField] public int defalutDamage { get; private set; }
    [field: SerializeField] public float ratio { get; private set; }

    [field: SerializeField] public LittleStarBuff lsBuff { get; private set; }


    public override void Update_Passive(ObjectBasic _User)
    {

        if (_User.FindBuff(lsBuff) == null)
        {
            _User.ApplyBuff(lsBuff);
        }

    }

    public override void Apply(ObjectBasic _User)
    {
        // 버프 적용
        _User.ApplyBuff(lsBuff);
    }

    public override void Remove(ObjectBasic _User)
    {
        _User.RemoveBuff(lsBuff);
    }
}
