using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHPPlus : Equipment
{
    // �ִ� ü�� ����
    // + ��ġ
    int variation;

    public override void Equip(Player target)
    {
        if(target.tag == "Player")
        {
            Debug.Log("�÷��̾� �ִ�ü�� +" + variation + " ����");
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.HPMax += variation;
        }
    }

    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.HPMax -= variation;
        }
    }


}
