using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalDamagePlus : Equipment
{
    // 치명타 피해량 증가
    // +%p

    [SerializeField] float variation;

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = target;
            Debug.Log("플레이어 치명타 피해량 +" + variation * 100 +"% 증가");
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
