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

    protected override void Pattern()
    {
        if (enemyTargetDis <= 3f)
        {
            StartCoroutine("Bite");
        }
        else
        {
            StartCoroutine("Jump");
        }
    }

    // Start is called before the first frame update
    IEnumerator Bite()
    {
        print("bite");
        float angle = Mathf.Atan2(enemyTarget.transform.position.y - transform.position.y, enemyTarget.transform.position.x - transform.position.x) * Mathf.Rad2Deg;

        isAttack = true;
        isAttackReady = false;
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
        biteArea.GetComponent<HitDetection>().SetHitDetection(false,-1,false,-1,stats.attackPower,5,0,0,null);
        biteArea.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);  // 방향 설정
        yield return new WaitForSeconds(biteDelay * 0.5f);

        isAttack = false;
        isAttackReady = true;
        biteArea.SetActive(false);
    }

    IEnumerator Jump()
    {
        print("Jump");
        Vector2 direction = (enemyTarget.transform.position - transform.position).normalized;

        // 점프 전 준비
        isAttack = true;
        isAttackReady = false;
        yield return new WaitForSeconds(jumpDelay * 0.3f);

        // 점프
        stats.increasedMoveSpeed += 3f;
        moveVec = direction;
        yield return new WaitForSeconds(jumpDelay * 0.4f);

        // 착지 피해
        stats.increasedMoveSpeed -= 3f;
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
        jumpArea.GetComponent<HitDetection>().SetHitDetection(false, -1, false, -1, stats.attackPower * 0.6f, 20, 0, 0, null);
        yield return new WaitForSeconds(jumpDelay * 0.3f);

        jumpArea.SetActive(false);
        isAttack = false;
        isAttackReady = true;
    }
}
