using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/DefensivePowerPlus")]
public class DefensivePowerPlus : PassiveData
{
    // 방어력 증가
    // +%p 수치
    [SerializeField] float variation;

    public override void Update_Passive(ObjectBasic _User) { }

    public override void Apply(ObjectBasic _User)
    {
        Debug.Log("플레이어 방어력+" + variation * 100 + "%p 증가");
        Stats stats = _User.GetComponent<PlayerStats>();
        stats.DefensivePower.AddValue += variation;
    }

    public override void Remove(ObjectBasic _User)
    {
        Debug.Log("플레이어 방어력+" + variation * 100 + "%p 증가");
        Stats stats = _User.GetComponent<PlayerStats>();
        stats.DefensivePower.AddValue -= variation;
    }


}
