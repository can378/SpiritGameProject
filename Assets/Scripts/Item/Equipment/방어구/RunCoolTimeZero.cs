using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunCoolTimeZero : Equipment
{
    // �޸��� �� �̵� �ӵ�
    // -%
    // �޸��� ��� �ð� ����
    // -��
    [SerializeField] float speedVariation;
    [SerializeField] float coolTimeVariation;

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            Debug.Log("�÷��̾� �޸��� �̵��ӵ� +" + speedVariation + "% ����");
            Debug.Log("�÷��̾� ��� �ð� +" + coolTimeVariation + "�� ����");
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
