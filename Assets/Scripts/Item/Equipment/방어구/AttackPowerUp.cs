using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPowerUp : Equipment
{
    // ���ݷ� ����
    // +%

    [SerializeField] float variation;

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            Debug.Log("�÷��̾� ���ݷ� +" + variation * 100 +"% ����");
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.increasedAttackPower += variation;
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.increasedAttackPower -= variation;
        }
    }
}
