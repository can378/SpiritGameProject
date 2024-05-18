using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunSpeedPlus : Equipment
{
    // �޸��� �� �߰� �̵� �ӵ�
    // %p
    [SerializeField] float variation;

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = target;
            Debug.Log("�÷��̾� �޸��� �̵��ӵ� +" + variation * 100 + "%p ����");
            PlayerStats playerStats = target.GetComponent<PlayerStats>();
            playerStats.addRunSpeed += variation;
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();
            playerStats.addRunSpeed -= variation;
            this.user = null;
        }
    }
}
