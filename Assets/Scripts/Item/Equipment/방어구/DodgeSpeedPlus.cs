using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeSpeedPlus : Equipment
{
    // ȸ�� �� �߰� �̵��ӵ� ����
    // %
    // ȸ�� �ð�
    // %
    [SerializeField] float timeVariation;
    [SerializeField] float speedVariation;

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            Debug.Log("�÷��̾� ȸ�� �� �߰� �̵��ӵ� +" + speedVariation * 100 + "%p ����");
            Debug.Log("�÷��̾� ȸ�� �ð� -" + timeVariation * 100 + "% ����");
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
