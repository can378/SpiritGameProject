using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/MoveSpeedPlus")]

public class MoveSpeedPlus : PassiveData
{
    // �̵��ӵ� ����
    // +��ġ
    [SerializeField] int variation;
    public override void Update_Passive(ObjectBasic _User)
    { }
    public override void Apply(ObjectBasic target)
    {
        Debug.Log("�÷��̾� �̵��ӵ� +" + variation + " ����");
        Stats stats = target.GetComponent<PlayerStats>();
        stats.MoveSpeed.AddValue += variation;
    }

    // Update is called once per frame
    public override void Remove(ObjectBasic target)
    {
        Stats stats = target.GetComponent<PlayerStats>();
        stats.MoveSpeed.AddValue -= variation;
    }

    public override string Update_Description(Stats _Stats)
    {
        return string.Format(PDescription, variation);
    }
}
