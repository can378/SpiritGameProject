using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeSpeedPlus : Equipment
{
    // 회피 시 추가 이동속도 증가
    // %
    // 회피 시간
    // %
    [SerializeField] float timeVariation;
    [SerializeField] float speedVariation;

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            Debug.Log("플레이어 회피 시 추가 이동속도 +" + speedVariation * 100 + "%p 증가");
            Debug.Log("플레이어 회피 시간 -" + timeVariation * 100 + "% 감소");
            PlayerStats playerStats = target.GetComponent<PlayerStats>();
            playerStats.increasedDodgeSpeed += speedVariation;
            playerStats.decreasedDodgeTime += timeVariation;
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();
            playerStats.increasedDodgeSpeed -= speedVariation;
            playerStats.decreasedDodgeTime -= timeVariation;
        }
    }
}
