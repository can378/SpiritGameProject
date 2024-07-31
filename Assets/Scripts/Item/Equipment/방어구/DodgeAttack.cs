using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeAttack : Equipment
{
    [SerializeField] GameObject dodgeHitBoxPrefab;
    GameObject dodgeHitBox;

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

        if(!dodgeHitBox.activeSelf && user.playerStatus.isDodge)
        {
            dodgeHitBox.GetComponent<HitDetection>().SetHitDetection(false,-1,false,-1,user.playerStats.attackPower * 0.1f + 1,10,0,0,null);
            dodgeHitBox.SetActive(true);
        }
        else if(dodgeHitBox.activeSelf && !user.playerStatus.isDodge)
        {
            dodgeHitBox.SetActive(false);
        }

    }

    public override void Equip(Player target)
    {
        if (target.tag == "Player")
        {
            this.user = target;
            dodgeHitBox = Instantiate(dodgeHitBoxPrefab,user.transform.position,Quaternion.identity);
            dodgeHitBox.transform.parent = user.gameObject.transform;
            dodgeHitBox.SetActive(false);
        }
    }

    // Update is called once per frame
    public override void UnEquip(Player target)
    {
        if (target.tag == "Player")
        {
            Destroy(dodgeHitBox);
            this.user = null;
        }
    }
}
