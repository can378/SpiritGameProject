using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunCoolTimeMinus : Equipment
{
    // �޸��� ��� �ð� ����
    // -��
    [SerializeField] float variation;

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            Debug.Log("�÷��̾� �޸��� ���ð� +" + variation + " ����");
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.addRunCoolTime -= variation;
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.addRunCoolTime += variation;
        }
    }
}
