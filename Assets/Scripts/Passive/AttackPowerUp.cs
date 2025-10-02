using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/AttackPowerUp")]
public class AttackPowerUp : PassiveData
{
    // ���ݷ� ����
    // +%

    [SerializeField] float variation;

    public override void Update_Passive(ObjectBasic _User)
    { }

    public override void Apply(ObjectBasic _User)
    {
        Debug.Log("�÷��̾� ���ݷ� +" + variation * 100 + "% ����");
        _User.stats.AttackPower.IncreasedValue += variation;
    }

    // Update is called once per frame
    public override void Remove(ObjectBasic _User)
    {
        Debug.Log("�÷��̾� ���ݷ� +" + variation * 100 + "% ����");
        _User.stats.AttackPower.IncreasedValue -= variation;
    }
}
