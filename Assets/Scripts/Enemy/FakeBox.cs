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
        HitDetection hitDetection;
        Vector3 hitDir = enemyStatus.targetDirVec;

        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;
        yield return new WaitForSeconds(0.5f);

        hitDetection = mouth.GetComponent<HitDetection>();
        hitDetection.user = this.gameObject;
        hitDetection.SetHitDetection(false, -1, false, -1, enemyStats.attackPower, 5);
        mouth.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(hitDir.y, hitDir.x) * Mathf.Rad2Deg - 90);
        mouth.SetActive(true);
        yield return new WaitForSeconds(2f);

        mouth.SetActive(false);
        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = true;

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
