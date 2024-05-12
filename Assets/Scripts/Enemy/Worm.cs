using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : EnemyBasic
{
    protected override void AttackPattern()
    {
        // 근거리 공격
        if (targetDis <= 2f)
        {
            StartCoroutine(worm());
        }
    }

    IEnumerator worm()
    {
        //stick to player
        isAttack = true;
        isAttackReady = false;

        transform.position = enemyTarget.transform.position;
        transform.parent = enemyTarget.gameObject.transform;
        sprite.sortingOrder = enemyTarget.GetComponentInChildren<SpriteRenderer>().sortingOrder + 1;

        //player move slowly
        enemyTarget.GetComponent<PlayerStats>().decreasedMoveSpeed += 0.5f;
        yield return new WaitForSeconds(3f);
        enemyTarget.GetComponent<PlayerStats>().decreasedMoveSpeed -= 0.5f;
        Destroy(this.gameObject);

    }

   


}
