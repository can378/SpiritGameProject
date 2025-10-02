using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/DefensivePowerPlus")]
public class DefensivePowerPlus : PassiveData
{
    // ���� ����
    // +%p ��ġ
    [SerializeField] float variation;

    public override void Update_Passive(ObjectBasic _User) { }

    public override void Apply(ObjectBasic _User)
    {
        Debug.Log("�÷��̾� ����+" + variation * 100 + "%p ����");
        Stats stats = _User.GetComponent<PlayerStats>();
        stats.DefensivePower.AddValue += variation;
    }

    public override void Remove(ObjectBasic _User)
    {
        Debug.Log("�÷��̾� ����+" + variation * 100 + "%p ����");
        Stats stats = _User.GetComponent<PlayerStats>();
        stats.DefensivePower.AddValue -= variation;
    }


}
