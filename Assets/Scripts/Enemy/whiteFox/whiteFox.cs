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
        StartCoroutine(RunAway(5f));
        yield return new WaitForSeconds(1f);

        hitDetection = hitEffects[0].GetComponent<HitDetection>();
        hitDetection.user = this.gameObject;
        hitDetection.SetHitDetection(false, -1, false, -1, enemyStats.attackPower, 10, 0, 0, null);
        hitEffects[0].transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(hitDir.y, hitDir.x) * Mathf.Rad2Deg - 90);
        hitEffects[0].SetActive(true);
        yield return new WaitForSeconds(0.6f);

        hitEffects[0].SetActive(false);
        whiteFoxStatus.isAttack = false;
        whiteFoxStatus.isAttackReady = true;
    }

    IEnumerator PeripheralAttack()
    {
        whiteFoxStatus.isAttack = true;
        whiteFoxStatus.isAttackReady = false;
        whiteFoxStatus.blizzardCoolTime = defaulBlizzardCoolTime;
        StartCoroutine(RunAway(5f));
        yield return new WaitForSeconds(0.5f);

        HitDetection hitDetection = hitEffects[1].GetComponent<HitDetection>();
        hitDetection.user = this.gameObject;
        hitDetection.SetHitDetection(false, -1, true, 3, enemyStats.attackPower * 0.5f, 0, 0, 0, blizzardDebuff);

        hitEffects[1].SetActive(true);
        yield return new WaitForSeconds(3f);

        hitEffects[1].SetActive(false);
        yield return new WaitForSeconds(1f);

        whiteFoxStatus.isAttack = false;
        whiteFoxStatus.isAttackReady = true;


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
