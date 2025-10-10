using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/MaxHPPlus")]
public class MaxHPPlus : PassiveData
{
    // �ִ� ü�� ����
    // + ��ġ
    [SerializeField] int variation;
    public override void Update_Passive(ObjectBasic _User)
    { }
    public override void Apply(ObjectBasic _User)
    {
        Stats stats = _User.GetComponent<PlayerStats>();
        stats.HPMax.AddValue += variation;
    }

    public override void Remove(ObjectBasic _User)
    {
        Stats stats = _User.GetComponent<PlayerStats>();
        stats.HPMax.AddValue -= variation;
    }

    public override string Update_Description(Stats _Stats)
    {
        return string.Format(PDescription, variation);
    }

}
