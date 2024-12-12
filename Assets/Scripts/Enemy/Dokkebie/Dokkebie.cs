using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dokkebie : EnemyBasic
{
    DokkebieStatus dokkebieStatus;

    protected override void Awake()
    {
        base.Awake();
        status = enemyStatus = dokkebieStatus = GetComponent<DokkebieStatus>();
    }

    protected override void Update()
    {
        base.Update();
        dokkebieStatus.fireCoolTime -= Time.deltaTime;
    }

    protected override void MovePattern()
    {
        // 적이 공격 사정거리 내에 있을 시
        if (dokkebieStatus.targetDis > 5f)
        {
            Chase();
        }
    }

    protected override void AttackPattern()
    {
        if (dokkebieStatus.targetDis <= enemyStats.maxAttackRange && dokkebieStatus.fireCoolTime <= 0)
        {
            dokkebieStatus.attackCoroutine = StartCoroutine(Fire());
        }
        else if (dokkebieStatus.targetDis <= 5f)
        {
            dokkebieStatus.attackCoroutine = StartCoroutine(Hammer());
        }
    }

    IEnumerator Fire()
    {
        //shot dokkebie fire

        dokkebieStatus.isAttack = true;
        dokkebieStatus.isAttackReady = false;
        yield return new WaitForSeconds(2f);

        GameObject bullet = ObjectPoolManager.instance.Get2("dokabbiFire");
        bullet.transform.position = transform.position;
        bullet.GetComponent<Rigidbody2D>().AddForce(dokkebieStatus.targetDirVec.normalized, ForceMode2D.Impulse);
        //Instantiate(ObjectPoolManager.instance.Get2("dokabbiFire"), transform.position,Quaternion.identity);

        yield return new WaitForSeconds(2f);

        dokkebieStatus.isAttack = false;
        dokkebieStatus.isAttackReady = true;
        dokkebieStatus.fireCoolTime = 10;
    }

    IEnumerator Hammer()
    {
        Vector2 attackTarget = dokkebieStatus.targetDirVec;

        dokkebieStatus.isAttack = true;
        dokkebieStatus.isAttackReady = false;
        yield return new WaitForSeconds(1f);
       
        hitEffects[0].GetComponent<HitDetection>().SetHitDetection(false, -1, false, -1, enemyStats.attackPower, 30);
        hitEffects[0].transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(attackTarget.y, attackTarget.x) * Mathf.Rad2Deg - 90);
        hitEffects[0].gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        hitEffects[0].gameObject.SetActive(false);
        dokkebieStatus.isAttack = false;
        dokkebieStatus.isAttackReady = true;
    }

    /*
    IEnumerator dokkebie()
    {
        if (index == 0)
        { 
            //shot dokkebie fire
            GameObject bullet = ObjectPoolManager.instance.Get2("dokabbiFire");
            bullet.transform.position = transform.position;


            targetDirVec = (enemyTarget.position - transform.position).normalized;
            bullet.GetComponent<Rigidbody2D>().
                AddForce(targetDirVec.normalized * 3, ForceMode2D.Impulse);
            yield return new WaitForSeconds(4f);
            
        }
        else 
        {
            //Chase
            targetDirVec = (enemyTarget.position - transform.position).normalized;
            targetDis = Vector2.Distance(transform.position, enemyTarget.position);
            rigid.AddForce(targetDirVec * GetComponent<EnemyStats>().defaultMoveSpeed * 20);
            yield return new WaitForSeconds(0.1f);
            
        }
        index++;
        
        if (index == 100) { index = 0; }
        StartCoroutine(dokkebie());

    }

    */
}
