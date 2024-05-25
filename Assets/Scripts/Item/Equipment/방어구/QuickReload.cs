using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickReload : Equipment
{
    bool isQuick = false;

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

        if(!isQuick && user.status.isReload)
        {
            isQuick = true;
        }
        else if(isQuick && !user.status.isReload)
        {
            isQuick = false;
        }
    }

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = target;
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
