using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevivalEquipment : Equipment
{
    [SerializeField] float revivalCoolTime = 0f;

    void Update()
    {
        Passive();
    }

    void Passive()
    {
        if (user == null)
        {
            return;
        }

        revivalCoolTime -= Time.deltaTime;

        if (revivalCoolTime <= 0f && user.stats.HP <= 0f)
        {
            Revival();
        }
    }

    void Revival()
    {
        revivalCoolTime = 20f;
        Debug.Log("부활!!!!");
        user.stats.HP = user.stats.HPMax * 0.5f;
    }

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = target;
            Debug.Log("부활");
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
