using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whiteFox : EnemyBasic
{
    private int attackIndex = 0;
    private float radius=10;
    private float attackTime=5;


    private void OnEnable()
    {
        StartNamedCoroutine("whiteFox_", whiteFox_());
    }


    IEnumerator whiteFox_()
    {
        switch (attackIndex)
        {
            case 0: StartCoroutine(hitAndRun()); break;
            case 1: StartCoroutine(peripheralAttack());break;
            default:break;
        }
        yield return null;
    }


    public IEnumerator hitAndRun()
    {
        //print("hit and run=" + stats.damage);
        targetDis = Vector2.Distance(transform.position, enemyTarget.position);
        //if (targetDis < stats.detectionDis)
        //{
        //getting closer
        do
        {
            Chase();
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
        StartCoroutine(whiteFox_());

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




        for (int i = 0; i < 10; i++) { Chase(); }

        yield return new WaitForSeconds(3f);
        attackIndex = 0;
        StartCoroutine(whiteFox_());



    }



}
