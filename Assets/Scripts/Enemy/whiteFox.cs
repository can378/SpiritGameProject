using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whiteFox : EnemyBasic
{
    [SerializeField] GameObject blizzardArea;
    [SerializeField] int defaulBlizzardCoolTime = 10;
    [SerializeField] int[] blizzardDebuff;
    [SerializeField] float blizzardTime = 5;

    [SerializeField] GameObject biteArea;
    [SerializeField] float biteTime = 5;

    float blizzardCoolTime;

    protected override void Update()
    {
        base.Update();
        blizzardCoolTime -= Time.deltaTime;
    }

    protected override void AttackPattern()
    {
        // 근거리 공격
        if (targetDis <= 3f)
        {
            StartCoroutine(HitAndRun());
        }
        // 원거리 공격
        else if (targetDis <= enemyStats.maxAttackRange && blizzardCoolTime <= 0f)
        {
            StartCoroutine(PeripheralAttack());
        }
    }

    IEnumerator HitAndRun()
    {
        isAttack = true;
        isAttackReady = false;

        yield return new WaitForSeconds(biteTime * 0.3f);

        HitDetection hitDetection = biteArea.GetComponent<HitDetection>();
        hitDetection.user = this.gameObject;
        hitDetection.SetHitDetection(false,-1,false,-1,enemyStats.attackPower,10,0,0,null);

        biteArea.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(enemyTarget.transform.position.y - transform.position.y, enemyTarget.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90, Vector3.forward);
        biteArea.SetActive(true);
        yield return new WaitForSeconds(biteTime * 0.4f);

        biteArea.SetActive(false);
        isAttack = false;
        yield return new WaitForSeconds(biteTime * 0.3f);

        isRun = true;
        yield return new WaitForSeconds(2f);

        isRun = false;
        isAttackReady = true;
    }

    IEnumerator PeripheralAttack()
    {
        isAttack = true;
        isAttackReady = false;

        yield return new WaitForSeconds(0.5f);

        HitDetection hitDetection = blizzardArea.GetComponent<HitDetection>();
        hitDetection.user = this.gameObject;
        hitDetection.SetHitDetection(false, -1, true, 3, enemyStats.attackPower * 0.5f, 0, 0, 0, blizzardDebuff);

        blizzardArea.SetActive(true);
        yield return new WaitForSeconds(blizzardTime);

        blizzardArea.SetActive(false);
        yield return new WaitForSeconds(1f);

        isAttack = false;
        isAttackReady = true;
        blizzardCoolTime = defaulBlizzardCoolTime;

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
