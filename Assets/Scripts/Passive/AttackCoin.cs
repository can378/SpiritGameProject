using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/AttackCoin")]
public class AttackCoin : PassiveData
{
    public int m_Coin;

    // 기본 공격 적중 시 재화 + 1
    public override void Update_Passive(ObjectBasic _User)
    {
        if (_User == null || !_User.status.hitTarget)
            return;
        if (_User.status.hitTarget.layer == LayerMask.NameToLayer("Wall"))
            return;

        Player.instance.playerStats.coin += m_Coin;
    }

    public override void Apply(ObjectBasic _User)
    {

    }

    public override void Remove(ObjectBasic _User)
    {
        
    }
}
