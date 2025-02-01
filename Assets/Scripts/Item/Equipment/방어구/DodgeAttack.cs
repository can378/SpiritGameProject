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
            dodgeHitBox.GetComponent<HitDetection>().SetHit_Ratio(5,10.0f, user.playerStats.DefensivePower);
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
