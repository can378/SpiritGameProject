using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunSpeedPlus : Equipment
{
    // 달리기 시 추가 이동 속도
    // %p
    [SerializeField] float variation;

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = target;
            Debug.Log("플레이어 달리기 이동속도 +" + variation * 100 + "%p 증가");
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
