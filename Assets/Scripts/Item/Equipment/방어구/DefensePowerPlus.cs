using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensivePowerPlus : Equipment
{
    // ���� ����
    // +% ��ġ
    float variation;

    public override void Equip(Player target)
    {
        if(target.tag == "Player")
        {
            Debug.Log("�÷��̾� ����+" + variation * 100 + "% ����");
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
        }
    }


}
