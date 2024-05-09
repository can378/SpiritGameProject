using System.Collections;
using System.Collections.Generic;
using JetBrains.Rider.Unity.Editor;
using UnityEngine;

public class Haetae : NPCbasic
{
    // 물기
    public float biteDelay;
    public GameObject biteArea;

    // 도약
    public float jumpDelay;
    public GameObject jumpArea;

    protected override void Attack()
    {
        attackDelay -= Time.deltaTime;
        isAttackReady = attackDelay < 0;

        if (!enemyTarget)
            return;

        if (!isFlinch && !isAttack && isAttackReady && enemyTargetDis <= maxEnemyTargetDis)
        {
            //print("Attack");
            moveVec = Vector2.zero;
            isAttack = true;
            isAttackReady = false;

            if (enemyTargetDis <= 3f)
            {
                StartCoroutine("Bite");
                attackDelay = biteDelay * 1.5f;
            }
            else
            {
                StartCoroutine("Jump");
                attackDelay = jumpDelay * 1.5f;
            }

            Invoke("AttackOut", attackDelay);
        }
    }

    // Start is called before the first frame update
    IEnumerator Bite()
    {
        Debug.Log("Bite");

        float angle = Mathf.Atan2(enemyTarget.transform.position.y - transform.position.y, enemyTarget.transform.position.x - transform.position.x) * Mathf.Rad2Deg;

        yield return new WaitForSeconds(biteDelay * 0.5f);

        switch(side)
        {
            case 0:
            {
                biteArea.tag = "AllAttack";
                biteArea.layer = LayerMask.NameToLayer("AllAttack");
                break;
            }
            case 1:
            {
                biteArea.tag = "PlayerAttack";
                biteArea.layer = LayerMask.NameToLayer("PlayerAttack");
                break;
            }
            case 2:
            {
                biteArea.tag = "EnemyAttack";
                biteArea.layer = LayerMask.NameToLayer("EnemyAttack");
                break;
            }
        }
        biteArea.SetActive(true);
        biteArea.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);  // 방향 설정

        yield return new WaitForSeconds(biteDelay * 0.5f);

        biteArea.SetActive(false);

        Debug.Log("BiteOut");
    }

    IEnumerator Jump()
    {

        Vector2 direction = (enemyTarget.transform.position - transform.position).normalized;

        yield return new WaitForSeconds(jumpDelay * 0.3f);

        stats.increasedMoveSpeed += 3f;
        moveVec = direction;

        yield return new WaitForSeconds(jumpDelay * 0.4f);
        stats.increasedMoveSpeed -= 3f;
        moveVec = Vector2.zero;

        
        switch (side)
        {
            case 0:
                {
                    jumpArea.tag = "AllAttack";
                    jumpArea.layer = LayerMask.NameToLayer("AllAttack");
                    break;
                }
            case 1:
                {
                    jumpArea.tag = "PlayerAttack";
                    jumpArea.layer = LayerMask.NameToLayer("PlayerAttack");
                    break;
                }
            case 2:
                {
                    jumpArea.tag = "EnemyAttack";
                    jumpArea.layer = LayerMask.NameToLayer("EnemyAttack");
                    break;
                }
        }
        jumpArea.SetActive(true);

        yield return new WaitForSeconds(jumpDelay * 0.3f);

        jumpArea.SetActive(false);
    }
}
