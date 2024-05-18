using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensivePowerPlus : Equipment
{
    // ���� ����
    // +%p ��ġ
    [SerializeField] float variation;

    public override void Equip(Player target)
    {
        if(target.tag == "Player")
        {
            this.user = target;
            Debug.Log("�÷��̾� ����+" + variation * 100 + "%p ����");
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.addDefensivePower += variation;
        }
    }

    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.addDefensivePower -= variation;
            this.user = null;
        }
    }


}
