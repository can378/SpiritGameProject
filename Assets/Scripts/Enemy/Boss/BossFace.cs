using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class BossFace : EnemyBasic
{
    public GameObject originalPose;
    public bool nowAttack = false;

    protected bool isFirst = true;
    private float approachTime = 1000;
    private float attackTime = 3000;
    protected float countTime = 0;

    protected bool isFaceAttack;
    protected bool isFaceApproach;
    protected bool isFaceBack;



    protected override void Update()
    {
        base.Update();
        if (isFirst&&nowAttack)
        {
            isFirst = false;
            init();
        }
    }

    protected virtual void init()
    {
        setApproach();
    }


    //MOVE////////////////////////////////////////
    protected override void Move()
    {
        if (isFaceApproach)
        {
            //Approaching
            if (countTime > 0)
            {
                MoveTo(gameObject, 2.5f, transform.position, FindObj.instance.Player.transform.position);
                countTime--;

            }
            else { setAttack(); }

        }
        else if (isFaceBack)
        {
            //Back
            if (Vector2.Distance(transform.position, originalPose.transform.position) >= 1f)
            {
                Vector3 vec = (originalPose.transform.position - transform.position).normalized;
                GetComponent<Rigidbody2D>().AddForce(vec * 2.5f);
                //print("go back to original pose");
            }
            else { Finish(); }
        }
        else if (isFaceAttack)
        {
            base.Move();
        }
    }


    protected bool MoveTo(GameObject obj, float speed, Vector3 from, Vector3 to)
    {
        Vector3 vec = (to - from).normalized;
        obj.GetComponent<Rigidbody2D>().AddForce(vec * speed);
        if (Vector2.Distance(obj.transform.position, to) < 5f)
        { return true; }
        return false;
    }


   
    //Set////////////////////////////////////////////////////////////
    protected void setApproach() 
    {
        isFaceApproach = true;
        isFaceBack = false;
        isFaceAttack = false;

        countTime = approachTime;
    }
    protected void setAttack() 
    {
        isFaceApproach = false;
        isFaceBack = false;
        isFaceAttack = true;

        countTime = attackTime;
    }
    protected virtual void setBack() 
    {
        isFaceApproach = false;
        isFaceBack = true;
        isFaceAttack = false;

        countTime = 0;
    }

    protected virtual void Finish() 
    {
        rigid.velocity= Vector3.zero;
        isFaceApproach = false;
        isFaceBack = false;
        isFaceAttack = false;

        countTime = 0;

        enemyStatus.isAttack = false;
        enemyStatus.isAttackReady = true;
        isFirst = true;
        nowAttack = false;
    }

    //ATTACK///////////////////////////////////////////////////////
    protected override void Attack()
    {
        if (!enemyStatus.EnemyTarget)
            return;

        enemyStatus.targetDis = Vector2.Distance(this.transform.position, enemyStatus.EnemyTarget.CenterPivot.position);
        enemyStatus.targetDirVec = (enemyStatus.EnemyTarget.CenterPivot.position - transform.position).normalized;

        //print(!isRun+" "+ !isFlinch+" "+!isAttack+" "+ isAttackReady+" "+ (targetDis <= enemyStats.maxAttackRange || enemyStats.maxAttackRange < 0));

        if (!enemyStatus.isRun && (0 >= enemyStatus.isFlinch) && !enemyStatus.isAttack && enemyStatus.isAttackReady && (enemyStatus.targetDis <= enemyStats.maxAttackRange || enemyStats.maxAttackRange < 0))
        {
            AttackPattern();
        }
    }

    protected override void AttackPattern()
    {
        if(nowAttack&& isFaceAttack)
        { 
            //Attack
            if (countTime > 0)
            {
                faceAttack();
                countTime--;                
            }
            else { setBack(); }
        }
    }

    
    protected virtual void faceAttack() {    }


   
}
