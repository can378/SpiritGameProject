using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/AttackSE")]
public class AttackSE : PassiveData
{
    [field: SerializeField] public SE_TYPE SEType { get; private set; }                 // 상태이상 효과

    public override void Update_Passive(ObjectBasic _User)
    {

    }

    public override void Apply(ObjectBasic user)
    {
        user.AddEnchant_SE(SEType);

    }

    public override void Remove(ObjectBasic user)
    {
        user.RemoveEnchant_SE(SEType);

    }
}
