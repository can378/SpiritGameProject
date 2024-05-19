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
        if (user == null || !user.hitTarget)
            return;

        if (user.hitTarget.gameObject.tag == "Wall" || user.hitTarget.gameObject.tag == "Door" || user.hitTarget.gameObject.tag == "ShabbyWall")
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
