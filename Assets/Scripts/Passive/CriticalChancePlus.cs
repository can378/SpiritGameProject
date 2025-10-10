using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/CriticalChancePlus")]
public class CriticalChancePlus : PassiveData
{
    // ġ��Ÿ Ȯ�� ����
    // +%p

    [SerializeField] float variation;

    public override void Update_Passive(ObjectBasic _User)
    {

    }

    public override void Apply(ObjectBasic target)
    {
        if (target.tag == "Player")
        {
            Debug.Log("�÷��̾� ġ��ŸȮ�� +" + variation * 100 + "%p ����");
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.CriticalChance.AddValue += variation;
        }
    }

    // Update is called once per frame
    public override void Remove(ObjectBasic target)
    {
        if (target.tag == "Player")
        {
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.CriticalChance.AddValue -= variation;
        }
    }

    public override string Update_Description(Stats _Stats)
    {
        return string.Format(PDescription, variation * 100);
    }
}
