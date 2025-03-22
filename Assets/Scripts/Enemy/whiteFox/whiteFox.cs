using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteFox : EnemyBasic
{
    WhiteFoxStatus whiteFoxStatus;
    [SerializeField] int defaulBlizzardCoolTime = 10;
    [SerializeField] int[] blizzardDebuff;

    protected override void Awake()
    {
        base.Awake();
        status = enemyStatus = whiteFoxStatus = GetComponent<WhiteFoxStatus>();
    }

    protected override void Update()
    {
        base.Update();
        whiteFoxStatus.blizzardCoolTime -= Time.deltaTime;
    }

    protected override void AttackPattern()
    {
        if (whiteFoxStatus.targetDis <= 3f)
        {
            enemyStatus.attackCoroutine = StartCoroutine(HitAndRun());
        }
        else if (whiteFoxStatus.targetDis <= enemyStats.maxAttackRange && whiteFoxStatus.blizzardCoolTime <= 0f)
        {
            enemyStatus.attackCoroutine = StartCoroutine(PeripheralAttack());
        }
    }

    IEnumerator HitAndRun()
    {
        HitDetection hitDetection;
        Vector3 hitDir = whiteFoxStatus.targetDirVec;

        whiteFoxStatus.isAttack = true;
        whiteFoxStatus.isAttackReady = false;
        yield return new WaitForSeconds(1f);

        if (hitEffects[0] != null)
        {
            hitDetection = hitEffects[0].GetComponent<HitDetection>();
            hitDetection.user = this;
            hitDetection.SetHit_Ratio(10, 1, enemyStats.AttackPower);
            hitEffects[0].transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(hitDir.y, hitDir.x) * Mathf.Rad2Deg - 90);
            hitEffects[0].SetActive(true);
            yield return new WaitForSeconds(0.6f);

            hitEffects[0].SetActive(false);
        }
        else
        {
            Debug.LogError("hitEffects[1]이 null");
        }
        whiteFoxStatus.isAttack = false;
        whiteFoxStatus.isAttackReady = true;
        RunAway(enemyStatus.enemyTarget.transform, 5.0f);
    }

    IEnumerator PeripheralAttack()
    {
        // 애니메이션 시작
        enemyAnim.animator.SetBool("isCry", true);

        // 공격 루틴 시작
        whiteFoxStatus.isAttack = true;
        whiteFoxStatus.isAttackReady = false;
        whiteFoxStatus.blizzardCoolTime = defaulBlizzardCoolTime;

        yield return new WaitForSeconds(0.5f);

        if (hitEffects[1] != null) { 
                // 영역 활성화
                HitDetection hitDetection = hitEffects[1].GetComponent<HitDetection>();
            hitDetection.user = this;
            hitDetection.SetHit_Ratio(1f, 0.1f, enemyStats.SkillPower);
            hitDetection.SetMultiHit(true, 10);
            hitDetection.SetSEs(blizzardDebuff);
            hitEffects[1].SetActive(true);
            yield return new WaitForSeconds(3f);

            // 영역 끄기
            hitEffects[1].SetActive(false);
            yield return new WaitForSeconds(1f);
        }
        else
        {
            Debug.LogError("hitEffects[1]이 null");
        }
        // 공격 루틴 끝
        whiteFoxStatus.isAttack = false;
        whiteFoxStatus.isAttackReady = true;


        if (enemyStatus.enemyTarget != null)
        {
            RunAway(enemyStatus.enemyTarget.transform, 5.0f);
        }
        else
        {
            Debug.LogWarning("enemyTarget이 null입니다. RunAway 실행 취소됨.");
        }


        // 애니메이션 끝
        enemyAnim.animator.SetBool("isCry", false);


    }

    public override void AttackCancle()
    {
        RunAway(whiteFoxStatus.enemyTarget, 5.0f);
        base.AttackCancle();
    }

    /*
        public IEnumerator hitAndRun()
        {
            //print("hit and run=" + stats.damage);
            //targetDis = Vector2.Distance(transform.position, enemyTarget.position);
            //if (targetDis < stats.detectionDis)
            //{
            //getting closer
            do
            {
                //Chase();
                targetDis = Vector2.Distance(transform.position, enemyTarget.position);
                yield return new WaitForSeconds(0.01f);
            } while (targetDis > 1.2f);


            //getting farther
            do
            {
                rigid.AddForce(-targetDirVec * stats.defaultMoveSpeed, ForceMode2D.Impulse);
                targetDis = Vector2.Distance(transform.position, enemyTarget.position);
                targetDirVec = (enemyTarget.transform.position - transform.position).normalized;
                yield return new WaitForSeconds(0.01f);
            } while (targetDis < 10f);

            //}
            yield return new WaitForSeconds(0.01f);
            rigid.velocity = Vector2.zero;
            yield return new WaitForSeconds(1f);


            attackIndex = 1;
        }

        public IEnumerator peripheralAttack()
        {

            //isCorRun = true;
            //extend collider itself

            float originRadius = transform.GetComponent<CircleCollider2D>().radius;
            transform.GetComponent<CircleCollider2D>().radius = radius;
            //print("peripheralAttack");

            yield return new WaitForSeconds(attackTime);

            transform.GetComponent<CircleCollider2D>().radius = originRadius;

            yield return new WaitForSeconds(3f);
            attackIndex = 0;
        }
    */


}
