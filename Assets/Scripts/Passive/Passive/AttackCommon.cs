using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/AttackCommon")]
public class AttackCommon : PassiveData
{
    [field: SerializeField] public COMMON_TYPE CommonType { get; private set; }         // 타격 성공 시 효과


    public override void Update_Passive(ObjectBasic _User)
    {

    }

    public override void Apply(ObjectBasic user)
    {
        user.AddEnchant_Common(CommonType);
    }

    public override void Remove(ObjectBasic user)
    {
        user.RemoveEnchant_Common(CommonType);
    }
}
