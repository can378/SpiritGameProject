using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : EnemyBasic
{
    protected override void AttackPattern()
    {
        // 근거리 공격
        if (enemyStatus.targetDis <= 2f)
        {
            StartCoroutine(worm());
        }
    }

    IEnumerator worm()
    {
        //stick to player
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        transform.position = enemyStatus.enemyTarget.transform.position;
        transform.parent = enemyStatus.enemyTarget.gameObject.transform;
        foreach(SpriteRenderer sprite in sprites)
        {
            sprite.sortingOrder = enemyStatus.enemyTarget.GetComponentInChildren<SpriteRenderer>().sortingOrder + 1;
        }
        

        //player move slowly
        enemyStatus.enemyTarget.GetComponent<PlayerStats>().MoveSpeed.DecreasedValue += 0.5f;
        yield return new WaitForSeconds(3f);
        enemyStatus.enemyTarget.GetComponent<PlayerStats>().MoveSpeed.DecreasedValue -= 0.5f;
        Destroy(this.gameObject);

    }

   


}
