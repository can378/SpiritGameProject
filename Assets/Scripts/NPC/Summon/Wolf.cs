using System.Collections;
using System.Collections.Generic;
using JetBrains.Rider.Unity.Editor;
using UnityEngine;

public class Wolf : NPCbasic
{
    // 물기
    public float biteDelay;
    public GameObject biteArea;

    // 도약
    public float jumpDelay;
    public GameObject jumpArea;

    public bool testCoroutine;
    public float timer = 0;

    protected override void Update()
    {
        base.Update();
        timer += Time.deltaTime;
    }

    protected override void Attack()
    {
        attackDelay -= Time.deltaTime;
        isAttackReady = attackDelay < 0;

        if (!enemyTarget)
            return;

        if (!isFlinch && !isAttack && isAttackReady && enemyTargetDis <= maxEnemyTargetDis)
        {
            print("Attack");
            moveVec = Vector2.zero;
            isAttack = true;
            isAttackReady = false;

            StartCoroutine("Jump");
            attackDelay = jumpDelay;

            /*
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
            */


            Invoke("AttackOut", attackDelay);
        }
    }

    // Start is called before the first frame update
    IEnumerator Bite()
    {
        Debug.Log("Bite");

        float angle = Mathf.Atan2(enemyTarget.transform.position.y - transform.position.y, enemyTarget.transform.position.x - transform.position.x) * Mathf.Rad2Deg;

        yield return new WaitForSeconds(biteDelay * 0.3f);

        biteArea.SetActive(true);
        biteArea.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);  // 방향 설정

        yield return new WaitForSeconds(biteDelay * 0.5f);

        biteArea.SetActive(false);

        Debug.Log("BiteOut");
    }

    IEnumerator Jump()
    {
        if(testCoroutine)
            print("코루틴 재시작됨");


        testCoroutine = true;
        Debug.Log("Jump");

        Vector2 direction = (enemyTarget.position - transform.position).normalized;

        yield return new WaitForSeconds(0.3f);
        print(timer);

        stats.increasedMoveSpeed += 3f;
        moveVec = direction;

        yield return new WaitForSeconds(0.3f);
        print(timer);
        stats.increasedMoveSpeed -= 3f;
        moveVec = Vector2.zero;

        jumpArea.SetActive(true);

        yield return new WaitForSeconds(0.3f);
        print(timer);

        jumpArea.SetActive(false);

        Debug.Log("JumpOut");
        testCoroutine = false;
    }
}
