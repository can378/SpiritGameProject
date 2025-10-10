using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/CriticalDamagePlus")]
public class CriticalDamagePlus : PassiveData
{
    // ġ��Ÿ ���ط� ����
    // +%p

    [SerializeField] float variation;

    public override void Update_Passive(ObjectBasic _User)
    {

    }

    public override void Apply(ObjectBasic _User)
    {
        if (_User.tag == "Player")
        {
            Debug.Log("�÷��̾� ġ��Ÿ ���ط� +" + variation * 100 + "% ����");
            PlayerStats plyaerStats = _User.GetComponent<PlayerStats>();
            plyaerStats.CriticalDamage.AddValue += variation;
        }
    }

    // Update is called once per frame
    public override void Remove(ObjectBasic _User)
    {
        if (_User.tag == "Player")
        {
            PlayerStats plyaerStats = _User.GetComponent<PlayerStats>();
            plyaerStats.CriticalDamage.AddValue -= variation;
        }
    }

    public override string Update_Description(Stats _Stats)
    {
        return string.Format(PDescription, variation * 100);
    }
}
