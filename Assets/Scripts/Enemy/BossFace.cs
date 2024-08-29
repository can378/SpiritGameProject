using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFace : EnemyBasic
{
    public GameObject originalPose;
    public bool nowAttack=false;

    protected bool isFirst=true;
    private float approachTime=200;
    private float attackTime=100;
    private float countTime = 0;

    protected bool isFaceAttack;


    protected override void Update() 
    {
        base.Update();
        if (isFirst) 
        {
            isFirst = false;
            initAttack();
        }
    }
    protected override void AttackPattern()
    {
        if(nowAttack) { StartCoroutine(faceAttackStart()); }
    }

    protected virtual void initAttack() 
    { 
         
    }
    protected virtual void faceAttack() 
    {
       
    }

    public IEnumerator faceAttackStart() 
    {
        //start
        enemyStatus.isAttack = true;
        enemyStatus.isAttackReady = false;

        //Approaching
        countTime = approachTime;
        while (countTime>=0)
        {
            MoveTo(gameObject, 150, transform.position, enemyStatus.enemyTarget.transform.position);
            yield return new WaitForSeconds(0.01f);
            countTime--;
        }


        //Attack
        countTime = attackTime;
        while (countTime >=0) 
        {
            faceAttack();
            yield return new WaitForSeconds(0.01f);
            countTime--;
            print(countTime);
        }


       
        //Back
        while (Vector2.Distance(transform.position, originalPose.transform.position) >= 2f)
        {
            Vector3 vec = (originalPose.transform.position - transform.position).normalized;
            GetComponent<Rigidbody2D>().AddForce(vec * 200);
            yield return new WaitForSeconds(0.01f);
            print("go back to original pose");
        }

        //END
        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = true;
        isFirst = true;
        nowAttack = false;
    }



    bool MoveTo(GameObject obj, float speed, Vector3 from, Vector3 to)
    {
        Vector3 vec = (to - from).normalized;
        obj.GetComponent<Rigidbody2D>().AddForce(vec * speed);
        if (Vector2.Distance(obj.transform.position, to) < 5f)
        { return true; }
        return false;
    }
}
