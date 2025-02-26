using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEResistPlus : Equipment
{
    // 상태이상 저항
    // +%p
    [SerializeField] float variation;

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = target;
            Debug.Log("플레이어 상태이상 저항 +" + variation * 100 + "%p 증가");
            PlayerStats playerStats = target.GetComponent<PlayerStats>();
            for(int i = 0; i< playerStats.SEResist.Length ; i++)
            {
                playerStats.SEResist[i].AddValue += variation;
            }
                
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();
            for (int i = 0; i < playerStats.SEResist.Length; i++)
            {
                playerStats.SEResist[i].AddValue -= variation;
            }
            this.user = null;
        }
    }
}
