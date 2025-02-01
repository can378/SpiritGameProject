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
            this.user = target;
            Debug.Log("�÷��̾� ���ݷ� +" + variation * 100 +"% ����");
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.AttackPower.IncreasedValue += variation;
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.AttackPower.IncreasedValue -= variation;
            this.user = null;
        }
    }
}
