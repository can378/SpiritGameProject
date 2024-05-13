using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeBox : EnemyBasic
{
    [SerializeField] GameObject mouth;
    [SerializeField] Player player;
    bool isWakeUp = false;

    protected override void Update()
    {
        base.Update();
        if (!isWakeUp && player != null)
        {
            if (player.iDown)
            {
                isWakeUp = true;
            }
        }
    }

    protected override void MovePattern()
    {
        if(isWakeUp)
        {
            Chase();
        }
    }

    protected override void AttackPattern()
    {
        if (isWakeUp)
        {
            StartCoroutine(fakeBox());
        }
    }

    //상호 작용 시 공격

    IEnumerator fakeBox() 
    {
        isAttack = true;
        isAttackReady = false;
        mouth.SetActive(true);
        mouth.GetComponent<HitDetection>().SetHitDetection(false,-1,false,-1,enemyStats.attackPower,10,0,0,null);
        yield return new WaitForSeconds(1f);

        mouth.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        
        isAttack = false;
        isAttackReady = true;
        
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        if(other.gameObject.tag == "Player")
        {
            player = other.gameObject.GetComponent<Player>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            player = null;
        }
    }
}
