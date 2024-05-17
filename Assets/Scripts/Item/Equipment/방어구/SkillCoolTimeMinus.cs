using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCoolTimeMinus : Equipment
{
    // ���� ��� �ð� ����
    // -%p
    [SerializeField] float variation;

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            Debug.Log("�÷��̾� ���� ���� ���ð� +" + variation * 100+ "% ����");
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.addSkillCoolTime += variation;
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.addSkillCoolTime -= variation;
        }
    }
}
