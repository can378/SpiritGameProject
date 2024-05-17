using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunCoolTimeMinus : Equipment
{
    // 달리기 대기 시간 감소
    // -초
    [SerializeField] float variation;

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            Debug.Log("플레이어 달리기 대기시간 +" + variation + " 감소");
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.addRunCoolTime -= variation;
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.addRunCoolTime += variation;
        }
    }
}
