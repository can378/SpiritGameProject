using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeTimeUp : Equipment
{
    // 회피 시 추가 이동속도 감소
    // %
    // 회피 시간
    // %
    [SerializeField] float timeVariation;
    [SerializeField] float speedVariation;

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = target;
            Debug.Log("플레이어 회피 시 추가 이동속도 +" + speedVariation * 100 + "%p 증가");
            Debug.Log("플레이어 회피 시간 +" + timeVariation * 100 + "% 증가");
            PlayerStats playerStats = target.GetComponent<PlayerStats>();
            playerStats.decreasedDodgeSpeed += speedVariation;
            playerStats.increasedDodgeTime += timeVariation;
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();
            playerStats.decreasedDodgeSpeed -= speedVariation;
            playerStats.increasedDodgeTime -= timeVariation;
            this.user = null;
        }
    }
}
