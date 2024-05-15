using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeedPlus : Equipment
{
    // �̵��ӵ� ����
    // +
    int variation;

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            Debug.Log("�÷��̾� �̵��ӵ� +" + variation + " ����");
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.addMoveSpeed += variation;
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.addMoveSpeed -= variation;
        }
    }
}
