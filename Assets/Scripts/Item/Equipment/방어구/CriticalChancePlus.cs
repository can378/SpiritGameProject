using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalChancePlus : Equipment
{
    // ġ��Ÿ Ȯ�� ����
    // +%p

    [SerializeField] float variation;

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = target;
            Debug.Log("�÷��̾� ġ��ŸȮ�� +" + variation * 100 +"%p ����");
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.addCriticalChance += variation;
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.addCriticalChance -= variation;
            this.user = null;
        }
    }
}
