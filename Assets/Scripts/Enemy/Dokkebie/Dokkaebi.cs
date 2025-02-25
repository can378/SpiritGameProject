using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dokkaebi : EnemyBasic
{
    DokkaebieStatus dokkeabieStatus;

    [SerializeField] Transform WispPos;

    protected override void Awake()
    {
        base.Awake();
        status = enemyStatus = dokkeabieStatus = GetComponent<DokkaebieStatus>();
    }

    protected override void Update()
    {
        base.Update();
        dokkeabieStatus.fireCoolTime -= Time.deltaTime;
    }

    protected override void MovePattern()
    {
        // ���� ���� �����Ÿ� ���� ���� ��
        if (dokkeabieStatus.targetDis > 5f)
        {
            Chase();
        }
    }

    protected override void AttackPattern()
    {
        if (dokkeabieStatus.targetDis <= enemyStats.maxAttackRange && dokkeabieStatus.fireCoolTime <= 0)
        {
            dokkeabieStatus.attackCoroutine = StartCoroutine(Fire());
        }
        else if (dokkeabieStatus.targetDis <= 5f)
        {
            dokkeabieStatus.attackCoroutine = StartCoroutine(Hammer());
        }
    }

    IEnumerator Fire()
    {
        //shot dokkebie fire

        // �ִϸ��̼� ����
        enemyAnim.animator.SetBool("isWisp",true);

        // ���� ��ƾ ����
        dokkeabieStatus.isAttack = true;
        dokkeabieStatus.isAttackReady = false;
        yield return new WaitForSeconds(1f);

        // ������ �� ��ȯ
        GameObject bullet = ObjectPoolManager.instance.Get("dokabbiFire");
        bullet.transform.position = WispPos.position;
        //bullet.GetComponent<Rigidbody2D>().AddForce(dokkeabieStatus.targetDirVec.normalized, ForceMode2D.Impulse);
        //Instantiate(ObjectPoolManager.instance.Get2("dokabbiFire"), transform.position,Quaternion.identity);

        // ���� ��ƾ ��
        dokkeabieStatus.isAttack = false;
        dokkeabieStatus.isAttackReady = true;
        dokkeabieStatus.fireCoolTime = 10;

        // �ִϸ��̼� ��
        enemyAnim.animator.SetBool("isWisp", false);
    }

    IEnumerator Hammer()
    {
        Vector2 attackTarget = dokkeabieStatus.targetDirVec;

        dokkeabieStatus.isAttack = true;
        dokkeabieStatus.isAttackReady = false;
        yield return new WaitForSeconds(1f);
       
        hitEffects[0].GetComponent<HitDetection>().SetHit_Ratio(10,1, enemyStats.AttackPower, 30);
        hitEffects[0].transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(attackTarget.y, attackTarget.x) * Mathf.Rad2Deg - 90);
        hitEffects[0].gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        hitEffects[0].gameObject.SetActive(false);
        dokkeabieStatus.isAttack = false;
        dokkeabieStatus.isAttackReady = true;
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
