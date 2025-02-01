using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalDamagePlus : Equipment
{
    // ġ��Ÿ ���ط� ����
    // +%p

    [SerializeField] float variation;

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = target;
            Debug.Log("�÷��̾� ġ��Ÿ ���ط� +" + variation * 100 +"% ����");
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.CriticalDamage.AddValue += variation;
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.CriticalDamage.AddValue -= variation;
            this.user = null;
        }
    }
}
