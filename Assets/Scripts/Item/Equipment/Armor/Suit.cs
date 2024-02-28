using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suit : Armor
{
    protected override void EquipArmor()
    {
        Debug.Log("플레이어 공격 속도 증가");
        Player.instance.userData.playerAttackSpeed += 1f;
    }

    // Update is called once per frame
    protected override void UnEquipArmor()
    {
        Player.instance.userData.playerAttackSpeed -= 1f;
    }
}
