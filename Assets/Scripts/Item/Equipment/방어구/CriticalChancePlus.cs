using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalChancePlus : Equipment
{
    // 치명타 확률 증가
    // +%p

    [SerializeField] float variation;

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = target;
            Debug.Log("플레이어 치명타확률 +" + variation * 100 +"%p 증가");
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.addCriticalChance += variation;
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.addCriticalChance -= variation;
            this.user = null;
        }
    }
}
