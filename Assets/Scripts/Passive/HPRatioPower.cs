using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/HPRatioPower")]
public class HPRatioPower : PassiveData
{
    // ü�� ��ġ�� �ٲ�� �ٽ� ����ؼ� ����

    [SerializeField] float ratio;   // ���� ü�� 1%�� ��ġ
    [SerializeField] float maxPowerUP;
    [SerializeField] float curLostHP;

    public override void Update_Passive(ObjectBasic _User)
    {
        if (_User == null)
            return;

        if (curLostHP == (float)Math.Round((_User.stats.HPMax.Value - _User.stats.HP) / _User.stats.HPMax.Value, 2))
            return;

        RePower(_User);
    }

    void RePower(ObjectBasic _User)
    {
        _User.stats.AttackPower.IncreasedValue -= Mathf.Clamp(ratio * curLostHP, 0, maxPowerUP);
        _User.stats.SkillPower.IncreasedValue -= Mathf.Clamp(ratio * curLostHP, 0, maxPowerUP);

        curLostHP = (float)Math.Round((_User.stats.HPMax.Value - _User.stats.HP) / _User.stats.HPMax.Value, 2);

        _User.stats.AttackPower.IncreasedValue += Mathf.Clamp(ratio * curLostHP, 0, maxPowerUP);
        _User.stats.SkillPower.IncreasedValue += Mathf.Clamp(ratio * curLostHP, 0, maxPowerUP);

        Debug.Log(Mathf.Clamp(ratio * curLostHP, 0, maxPowerUP) * 100 + "% powerUp");
    }

    public override void Apply(ObjectBasic _User)
    {
        Debug.Log("ü�� ��� ���ݷ�, ���� ����");
        curLostHP = 0;
        RePower(_User);
    }

    public override void Remove(ObjectBasic _User)
    {
        if (_User.tag == "Player")
        {
            _User.stats.AttackPower.IncreasedValue -= Mathf.Clamp(ratio * curLostHP, 0, maxPowerUP);
            _User.stats.SkillPower.IncreasedValue -= Mathf.Clamp(ratio * curLostHP, 0, maxPowerUP);
        }
    }


}
