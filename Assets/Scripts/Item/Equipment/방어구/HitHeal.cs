using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitHeal : Equipment
{
    // �÷��̾ ���� ���� �� variation ��ŭ ȸ��
    [SerializeField] float variation;

    void Update()
    {
        Passive();
    }

    void Passive()
    {
        if (user == null || !user.playerStatus.hitTarget)
            return;

        if (user.playerStatus.hitTarget.gameObject.tag == "Wall" || user.playerStatus.hitTarget.gameObject.tag == "Door" || user.playerStatus.hitTarget.gameObject.tag == "ShabbyWall")
            return;

        user.Damaged(-variation);
    }

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = target;
            Debug.Log("�÷��̾ �⺻ ���� ���� �� ȸ��");
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = null;
        }
    }
}
