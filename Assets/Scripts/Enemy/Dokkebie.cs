using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dokkebie : EnemyBasic
{
    [SerializeField] GameObject hammerArea;
    [SerializeField] int defaultCoolTime;
    float coolTime = 0;

    //도깨비불을 쏘면서 천천히 다가옴

    protected override void Update()
    {
        base.Update();
        coolTime -= Time.deltaTime;
    }

    protected override void MovePattern()
    {
        // 적이 공격 사정거리 내에 있을 시
        if (targetDis <= 3f)
        {
            isChase = false;
        }
        else
        {
            isChase = true;
        }
    }

    protected override void AttackPattern()
    {
        if (targetDis <= enemyStats.maxAttackRange && coolTime <= 0)
        {
            StartCoroutine(Fire());
        }
        else if (targetDis <= 3f)
        {
            StartCoroutine(Hammer());
        }
    }

    IEnumerator Fire()
    {
        //shot dokkebie fire

        isAttack = true;
        isAttackReady = false;
        yield return new WaitForSeconds(1f);

        GameObject bullet = Instantiate(ObjectPoolManager.instance.Get2("dokabbiFire"), transform.position,Quaternion.identity);
        bullet.GetComponent<Guiding>().guidingTarget = enemyTarget;
        
        yield return new WaitForSeconds(3f);

        isAttack = false;
        isAttackReady = true;
        coolTime = defaultCoolTime;
    }

    IEnumerator Hammer()
    {
        print("Hammer");

        isAttack = true;
        isAttackReady = false;

        // 물기 전 대기 시간
        yield return new WaitForSeconds(1f);

        HitDetection hitDetection = hammerArea.GetComponent<HitDetection>();
        hitDetection.user = this.gameObject;

        hammerArea.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(enemyTarget.transform.position.y - transform.position.y, enemyTarget.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90, Vector3.forward);
        hammerArea.SetActive(true);

        // 물기 판정 유지 시간
        yield return new WaitForSeconds(2f);

        hammerArea.SetActive(false);

        isAttack = false;
        isAttackReady = true;
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
