using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeedPlus : Equipment
{
    // �̵��ӵ� ����
    // +��ġ
    [SerializeField] int variation;

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = target;
            Debug.Log("�÷��̾� �̵��ӵ� +" + variation + " ����");
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.MoveSpeed.AddValue += variation;
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.MoveSpeed.AddValue -= variation;
            this.user = null;
        }
    }
}
