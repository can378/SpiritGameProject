using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPassive", menuName = "Passive/CriticalChanceFull")]
public class CriticalChanceFull : PassiveData
{
    // ġ��Ÿ Ȯ�� ����
    // %p
    // ġ��Ÿ ���ط� ����
    // %p

    [SerializeField] float chanceVariation;
    [SerializeField] float damageVariation;

    public override void Update_Passive(ObjectBasic _User)
    {

    }

    public override void Apply(ObjectBasic _User)
    {
        if (_User.tag == "Player")
        {
            Debug.Log("�÷��̾� ġ��Ÿ Ȯ�� +" + chanceVariation * 100 + "%p ����");
            Debug.Log("�÷��̾� ġ��Ÿ ���ط� +" + damageVariation * 100 + "%p ����");
            PlayerStats playerStats = _User.GetComponent<PlayerStats>();
            playerStats.CriticalChance.AddValue += chanceVariation;
            playerStats.CriticalDamage.AddValue -= damageVariation;
        }
    }

    // Update is called once per frame
    public override void Remove(ObjectBasic _User)
    {
        if (_User.tag == "Player")
        {
            PlayerStats playerStats = _User.GetComponent<PlayerStats>();
            playerStats.CriticalChance.AddValue -= chanceVariation;
            playerStats.CriticalDamage.AddValue += damageVariation;
        }
    }
}
