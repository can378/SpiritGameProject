using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeedPlus : Equipment
{
    // 이동속도 증가
    // +수치
    [SerializeField] int variation;

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = target;
            Debug.Log("플레이어 이동속도 +" + variation + " 증가");
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.MoveSpeed.AddValue += variation;
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            PlayerStats plyaerStats = target.GetComponent<PlayerStats>();
            plyaerStats.MoveSpeed.AddValue -= variation;
            this.user = null;
        }
    }
}
