using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/MaxHPPlus")]
public class MaxHPPlus : PassiveData
{
    // 최대 체력 증가
    // + 수치
    [SerializeField] int variation;
    public override void Update_Passive(ObjectBasic _User)
    { }
    public override void Apply(ObjectBasic _User)
    {
        Debug.Log("플레이어 최대체력 +" + variation + " 증가");
        Stats stats = _User.GetComponent<PlayerStats>();
        stats.HPMax.AddValue += variation;
    }

    public override void Remove(ObjectBasic _User)
    {
        Debug.Log("플레이어 최대체력 +" + variation + " 증가");
        Stats stats = _User.GetComponent<PlayerStats>();
        stats.HPMax.AddValue -= variation;
    }


}
