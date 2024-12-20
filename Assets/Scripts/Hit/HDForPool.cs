using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 기본 공격 판정
// pool 용
public class HDForPool : HitDetection
{
    protected override void OnTriggerEnter2D(Collider2D other) 
    {

        if (isProjectile)
        {
            //print(other.tag);
            if (other.gameObject.tag == "Wall" || other.gameObject.tag == "Door" || 
                other.gameObject.tag == "ShabbyWall" || other.CompareTag("EnemyWall"))
            {
                gameObject.SetActive(false);
            }
            


            if (!this.isMultiHit)
            {
                if (this.penetrations >= 1)
                {
                    penetrations--;
                }
                else if (this.penetrations == 0)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }

        /*
        if (isMultiHit)
        {
            if(!target.Contains(other.gameObject))
                target.Add(other.gameObject);
        }
        */

        if (!user)
            return;

        if (!other.gameObject.GetComponent<ObjectBasic>())
            return;

        if (other.gameObject.GetComponent<ObjectBasic>().status.isInvincible)
            return;

        user.GetComponent<ObjectBasic>().status.hitTarget = other.gameObject;
    }

    public override void SetProjectileTime(float time)
    {
        StartCoroutine(DisableCor(time));
    }

    IEnumerator DisableCor(float time)
    {
        yield return new WaitForSeconds(time);

        this.gameObject.SetActive(false);
    }

}
