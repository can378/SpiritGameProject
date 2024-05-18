using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalChanceFull : Equipment
{
    // ġ��Ÿ Ȯ�� ����
    // %p
    // ġ��Ÿ ���ط� ����
    // %p

    [SerializeField] float chanceVariation;
    [SerializeField] float damageVariation;

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = target;
            Debug.Log("�÷��̾� ġ��Ÿ Ȯ�� +" + chanceVariation * 100 +"%p ����");
            Debug.Log("�÷��̾� ġ��Ÿ ���ط� +" + damageVariation * 100 + "%p ����");
            PlayerStats playerStats = target.GetComponent<PlayerStats>();
            playerStats.addCriticalChance += chanceVariation;
            playerStats.addCriticalDamage -= damageVariation;
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();
            playerStats.addCriticalChance -= chanceVariation;
            playerStats.addCriticalDamage += damageVariation;
            this.user = null;
        }
    }
}
