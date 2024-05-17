using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCoolTimeMinus : Equipment
{
    // 도술 대기 시간 감소
    // -%p
    [SerializeField] float variation;

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            Debug.Log("플레이어 도술 재사용 대기시간 +" + variation * 100+ "% 감소");
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.addSkillCoolTime += variation;
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.addSkillCoolTime -= variation;
        }
    }
}
