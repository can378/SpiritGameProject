using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dokkebie : EnemyBasic
{
    [SerializeField] GameObject hammerArea;
    [SerializeField] int defaultFireCoolTime;
    float fireCoolTime = 0;


    private void Start()
    {
        base.Start();
        hitDetection = hammerArea.GetComponent<HitDetection>();
        hitDetection.user = this.gameObject;
    }

    protected override void Update()
    {
        base.Update();
        fireCoolTime -= Time.deltaTime;
    }

    protected override void MovePattern()
    {
        // 적이 공격 사정거리 내에 있을 시
        if (targetDis > 5f)
        {
            Chase();
        }
    }

    protected override void AttackPattern()
    {
        if (targetDis <= enemyStats.maxAttackRange && fireCoolTime <= 0)
        {
            StartCoroutine(Fire());
        }
        else if (targetDis <= 5f)
        {
            StartCoroutine(Hammer());
        }
    }

    IEnumerator Fire()
    {
        //shot dokkebie fire

        isAttack = true;
        isAttackReady = false;
        yield return new WaitForSeconds(0.5f);

        GameObject bullet = ObjectPoolManager.instance.Get2("dokabbiFire");
        bullet.transform.position = transform.position;
        bullet.GetComponent<Rigidbody2D>().AddForce(targetDirVec.normalized, ForceMode2D.Impulse);
        //Instantiate(ObjectPoolManager.instance.Get2("dokabbiFire"), transform.position,Quaternion.identity);

        yield return new WaitForSeconds(0.5f);

        isAttack = false;
        isAttackReady = true;
        fireCoolTime = defaultFireCoolTime;
    }

    IEnumerator Hammer()
    {
        
        isAttack = true;
        isAttackReady = false;
        yield return new WaitForSeconds(0.5f);

       
        hitDetection.SetHitDetection(false, -1, false, -1, enemyStats.attackPower, 30, 0, 0, null);
        hammerArea.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(targetDirVec.y, targetDirVec.x) * Mathf.Rad2Deg - 90);
        hammerArea.SetActive(true);
        yield return new WaitForSeconds(1.5f);
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
