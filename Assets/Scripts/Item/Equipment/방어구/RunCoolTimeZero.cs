using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunCoolTimeZero : Equipment
{
    // 달리기 시 이동 속도
    // -%
    // 달리기 대기 시간 감소
    // -초
    [SerializeField] float speedVariation;
    [SerializeField] float coolTimeVariation;

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            Debug.Log("플레이어 달리기 이동속도 +" + speedVariation + "% 감소");
            Debug.Log("플레이어 대기 시간 +" + coolTimeVariation + "초 감소");
            PlayerStats playerStats = target.GetComponent<PlayerStats>();
            playerStats.decreasedRunSpeed += speedVariation;
            playerStats.addRunCoolTime -= coolTimeVariation;
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            PlayerStats playerStats = target.GetComponent<PlayerStats>();
            playerStats.decreasedRunSpeed -= speedVariation;
            playerStats.addRunCoolTime += coolTimeVariation;
        }
    }
}
