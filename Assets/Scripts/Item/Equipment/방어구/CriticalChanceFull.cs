using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalChanceFull : Equipment
{
    // 치명타 확률 증가
    // %p
    // 치명타 피해량 감소
    // %p

    [SerializeField] float chanceVariation;
    [SerializeField] float damageVariation;

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = target;
            Debug.Log("플레이어 치명타 확률 +" + chanceVariation * 100 +"%p 증가");
            Debug.Log("플레이어 치명타 피해량 +" + damageVariation * 100 + "%p 감소");
            PlayerStats playerStats = target.GetComponent<PlayerStats>();
            playerStats.addCriticalChance += chanceVariation;
            playerStats.addCriticalDamage -= damageVariation;
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();
            playerStats.addCriticalChance -= chanceVariation;
            playerStats.addCriticalDamage += damageVariation;
            this.user = null;
        }
    }
}
